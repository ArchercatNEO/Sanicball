using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanicball.Data
{
    public class ActiveData : MonoBehaviour
    {
        #region Factory Methods

        //Pseudo-singleton pattern - this field accesses the current file.
        private static LazyFile<ActiveData> file => new("Prefabs/ActiveData");

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
        [SerializeField] private StageInfo[] stages = {};
        public static StageInfo[] Stages { get { return file.File.stages; } }


        [SerializeField] private CharacterInfo[] characters = {};
        public static CharacterInfo[] Characters { get { return file.File.characters; } }


        [SerializeField] private GameJoltInfo gameJoltInfo;
        public static GameJoltInfo GameJoltInfo { get { return file.File.gameJoltInfo; } }


        [SerializeField] private GameObject christmasHat;
        public static GameObject ChristmasHat { get { return file.File.christmasHat; } }
        #endregion Fields
    }
}
