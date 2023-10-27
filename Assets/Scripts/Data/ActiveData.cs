using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using SanicballCore;
using System.Linq;

namespace Sanicball.Data
{
    public class ActiveData : MonoBehaviour
    {
        #region Fields

        public List<RaceRecord> raceRecords = new();

        //Pseudo-singleton pattern - this field accesses the current instance.
        private static ActiveData instance;

        //This data is saved to a json file
        private GameSettings gameSettings = new();

        private KeybindCollection keybinds = new();
        private MatchSettings matchSettings = MatchSettings.CreateDefault();

        //This data is set from the editor and remains constant
        [Header("Static data")]
        [SerializeField] private StageInfo[] stages;

        [SerializeField] private CharacterInfo[] characters;

        [SerializeField] private GameJoltInfo gameJoltInfo;
        [SerializeField] private GameObject christmasHat;
        [SerializeField] private Material eSportsTrail;
        [SerializeField] private GameObject eSportsHat;
        [SerializeField] private AudioClip eSportsMusic;
        [SerializeField] private ESportMode eSportsPrefab;

        #endregion Fields

        #region Properties

        public static GameSettings GameSettings { get { return instance.gameSettings; } }
        public static KeybindCollection Keybinds { get { return instance.keybinds; } }
        public static MatchSettings MatchSettings { get { return instance.matchSettings; } set { instance.matchSettings = value; } }
        public static List<RaceRecord> RaceRecords { get { return instance.raceRecords; } }

        public static StageInfo[] Stages { get { return instance.stages; } }
        public static CharacterInfo[] Characters { get { return instance.characters; } }
        public static GameJoltInfo GameJoltInfo { get { return instance.gameJoltInfo; } }
        public static GameObject ChristmasHat { get { return instance.christmasHat; } }
        public static Material ESportsTrail {get{return instance.eSportsTrail;}}
        public static GameObject ESportsHat {get{return instance.eSportsHat;}}
        public static AudioClip ESportsMusic {get{return instance.eSportsMusic;}}
        public static ESportMode ESportsPrefab {get{return instance.eSportsPrefab;}}

        public static bool ESportsFullyReady {
            get {
                if (!GameSettings.eSportsReady) return false;
            
                Logic.MatchManager m = FindObjectOfType<Logic.MatchManager>();
                if (!m) return false;
                
                return !m.Players.Any(
                    player => player.CtrlType != ControlType.None && 
                    player.CharacterId != 13
                );
            }
        }

        #endregion Properties

        #region Unity functions

        //Make sure there is never more than one GameData object
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            Load("GameSettings.json", ref gameSettings);
            Load("GameKeybinds.json", ref keybinds);
            Load("MatchSettings.json", ref matchSettings);
            Load("Records.json", ref raceRecords);
            gameJoltInfo.Init();
        }

        private void OnApplicationQuit()
        {
            Save("GameSettings.json", gameSettings);
            Save("GameKeybinds.json", keybinds);
            Save("MatchSettings.json", matchSettings);
            Save("Records.json", raceRecords);
        }

        #endregion Unity functions

        #region Saving and loading

        private void Load<T>(string filename, ref T output)
        {
            //Make sure file exists
            string fullPath = $"{Application.persistentDataPath}/{filename}";
            if (!File.Exists(fullPath))
            {
                Debug.Log(filename + " has not been loaded - file not found.");
                return;
            }
            
            //Load file contents
            string dataString = new StreamReader(fullPath).ReadToEnd();
            
            //Deserialize from JSON into a data object
            try
            {
                T dataObj = JsonConvert.DeserializeObject<T>(dataString);
                //Make sure an object was created, this would't end well with a null value
                if (dataObj == null)
                {
                    Debug.LogError("Failed to load " + filename + ": file is empty.");
                    return;
                }

                output = dataObj;
                Debug.Log(filename + " loaded successfully.");
            }
            catch (JsonException ex)
            {
                Debug.LogError($"Failed to parse {filename}! JSON converter info: {ex.Message}");
            }
        }

        private void Save(string filename, object objToSave)
        {
            string fullPath = $"{Application.persistentDataPath}/{filename}";
            string data = JsonConvert.SerializeObject(objToSave);
            
            new StreamWriter(fullPath).Write(data);
            Debug.Log(filename + " saved successfully.");
        }

        #endregion Saving and loading
    }
}
