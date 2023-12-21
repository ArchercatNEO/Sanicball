using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanicball.Data
{
    public class ActiveData : MonoBehaviour
    {
        #region Factory Methods

        //Pseudo-singleton pattern - this field accesses the current file.
        private static ActiveData file => Resources.Load<ActiveData>("Prefabs/ActiveData");

        /// <summary>
        /// Guarantee there is one and only ActiveData
        /// </summary>
        static ActiveData()
        {
            //GameJoltInfo.Init();
        }

        #endregion Factory Methods

        #region Fields
        //This data is set from the editor and remains constant
        [Header("Static data")]
        [SerializeField] private StageInfo[] stages;
        public static StageInfo[] Stages { get { return file.stages; } }


        [SerializeField] private CharacterInfo[] characters;
        public static CharacterInfo[] Characters { get { return file.characters; } }


        [SerializeField] private GameJoltInfo gameJoltInfo;
        public static GameJoltInfo GameJoltInfo { get { return file.gameJoltInfo; } }


        [SerializeField] private GameObject christmasHat;
        public static GameObject ChristmasHat { get { return file.christmasHat; } }


        [SerializeField] private Material eSportsTrail;
        public static Material ESportsTrail { get { return file.eSportsTrail; } }


        [SerializeField] private GameObject eSportsHat;
        public static GameObject ESportsHat { get { return file.eSportsHat; } }


        [SerializeField] private AudioClip eSportsMusic;
        public static AudioClip ESportsMusic { get { return file.eSportsMusic; } }

        #endregion Fields
    }
}
