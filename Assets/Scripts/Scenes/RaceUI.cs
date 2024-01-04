using Sanicball.Logic;
using UnityEngine;

namespace Sanicball.Canvas
{
    public class RaceUI : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            //MatchManager.Update();        
            RaceManager.Update();
        }
    }
}
