using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Lidgren.Network;
using Sanicball.Logic;
using Sanicball.Data;

namespace Sanicball.UI
{
    public class OnlinePanel : MonoBehaviour
    {
        public Transform targetServerListContainer;
        public Text errorField;
        public Text serverCountField;
        public ServerListItem serverListItemPrefab;
        public Selectable aboveList;
        public Selectable belowList;

        private List<ServerListItem> servers = new List<ServerListItem>();

        // Stores server browser IPs, so they can be differentiated from LAN servers
        private List<string> serverBrowserIPs = new List<string>();

        private NetClient discoveryClient;
        private UnityWebRequest serverBrowserRequester;
        private DateTime latestLocalRefreshTime;
        private DateTime latestBrowserRefreshTime;

        public void RefreshServers()
        {
            serverBrowserIPs.Clear();

            discoveryClient.DiscoverLocalPeers(25000);
            latestLocalRefreshTime = DateTime.Now;

            string serverListURL = ActiveData.GameSettings.serverListURL;
            //StartCoroutine(FetchServerList(serverListURL));
        }

        private System.Collections.IEnumerator FetchServerList(string serverListURL)
        {
            serverCountField.text = "Refreshing servers, hang on...";
            errorField.enabled = false;

            // Clear old servers
            foreach (var serv in servers)
            {
                Destroy(serv.gameObject);
            }
            servers.Clear();

            serverBrowserRequester = UnityWebRequest.Get(serverListURL);
            yield return serverBrowserRequester.SendWebRequest();

            if (serverBrowserRequester.result == UnityWebRequest.Result.Success)
            {
                latestBrowserRefreshTime = DateTime.Now;

                string result = serverBrowserRequester.downloadHandler.text;
                string[] entries = result.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string entry in entries)
                {
                    int separationPoint = entry.LastIndexOf(':');
                    string ip = entry.Substring(0, separationPoint);
                    string port = entry.Substring(separationPoint + 1, entry.Length - (separationPoint + 1));

                    if (int.TryParse(port, out int portInt))
                    {
                        System.Threading.Thread discoverThread = new System.Threading.Thread(() => { discoveryClient.DiscoverKnownPeer(ip, portInt); });
                        discoverThread.Start();
                        serverBrowserIPs.Add(ip);
                    }
                }
                serverCountField.text = "0 servers";
            }
            else
            {
                Debug.LogError("Failed to receive servers - " + serverBrowserRequester.error);
                serverCountField.text = "Cannot access server list URL!";
            }
        }

        private void Awake()
        {
            errorField.enabled = false;

            NetPeerConfiguration config = new NetPeerConfiguration(OnlineMatchMessenger.APP_ID);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            discoveryClient = new NetClient(config);
            discoveryClient.Start();
        }

        private void Update()
        {
            // Refresh on F5 (pretty nifty)
            if (Input.GetKeyDown(KeyCode.F5))
            {
                RefreshServers();
            }

            // Check for response from the server browser requester
            if (serverBrowserRequester != null && serverBrowserRequester.isDone)
            {
                if (string.IsNullOrEmpty(serverBrowserRequester.error))
                {
                    latestBrowserRefreshTime = DateTime.Now;

                    string result = serverBrowserRequester.downloadHandler.text;
                    string[] entries = result.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string entry in entries)
                    {
                        int separationPoint = entry.LastIndexOf(':');
                        string ip = entry.Substring(0, separationPoint);
                        string port = entry.Substring(separationPoint + 1, entry.Length - (separationPoint + 1));

                        if (int.TryParse(port, out int portInt))
                        {
                            System.Threading.Thread discoverThread = new System.Threading.Thread(() => { discoveryClient.DiscoverKnownPeer(ip, portInt); });
                            discoverThread.Start();
                            serverBrowserIPs.Add(ip);
                        }
                    }
                    serverCountField.text = "0 servers";
                }
                else
                {
                    Debug.LogError("Failed to receive servers - " + serverBrowserRequester.error);
                    serverCountField.text = "Cannot access server list URL!";
                }

                serverBrowserRequester = null;
            }

            // Check for messages on the discovery client
            NetIncomingMessage msg;
            while ((msg = discoveryClient.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DiscoveryResponse:
                        ServerInfo info;
                        try
                        {
                            info = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerInfo>(msg.ReadString());
                        }
                        catch (Newtonsoft.Json.JsonException ex)
                        {
                            Debug.LogError("Failed to deserialize info for a server: " + ex.Message);
                            continue;
                        }

                        //double timeDiff = (DateTime.UtcNow - info.Timestamp).TotalMilliseconds;
                        bool isLocal = !serverBrowserIPs.Contains(msg.SenderEndPoint.Address.ToString());

                        DateTime timeToCompareTo = isLocal ? latestLocalRefreshTime : latestBrowserRefreshTime;
                        double timeDiff = (DateTime.Now - timeToCompareTo).TotalMilliseconds;

                        var server = Instantiate(serverListItemPrefab);
                        server.transform.SetParent(targetServerListContainer, false);
                        server.Init(info, msg.SenderEndPoint, (int)timeDiff, isLocal);
                        servers.Add(server);
                        RefreshNavigation();

                        serverCountField.text = servers.Count + (servers.Count == 1 ? " server" : " servers");

                        break;

                    default:
                        Debug.Log("Server discovery client received an unhandled NetMessage (" + msg.MessageType + ")");
                        break;
                }
            }
        }

        private void RefreshNavigation()
        {
            for (var i = 0; i < servers.Count; i++)
            {
                var button = servers[i].GetComponent<Button>();
                if (button)
                {
                    var nav = new Navigation() { mode = Navigation.Mode.Explicit };
                    // Up navigation
                    if (i == 0)
                    {
                        nav.selectOnUp = aboveList;
                        var nav2 = aboveList.navigation;
                        nav2.selectOnDown = button;
                        aboveList.navigation = nav2;
                    }
                    else
                    {
                        nav.selectOnUp = servers[i - 1].GetComponent<Button>();
                    }
                    // Down navigation
                    if (i == servers.Count - 1)
                    {
                        nav.selectOnDown = belowList;
                        var nav2 = belowList.navigation;
                        nav2.selectOnUp = button;
                        belowList.navigation = nav2;
                    }
                    else
                    {
                        nav.selectOnDown = servers[i + 1].GetComponent<Button>();
                    }

                    button.navigation = nav;
                }
            }
        }
    }
}
