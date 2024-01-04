using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Sanicball.Logic;

namespace Sanicball.UI
{
    public class RaceCountdown : MonoBehaviour
    {
        private static RaceCountdown prefab => Resources.Load<RaceCountdown>("Prefabs/Instantiated/RaceCountdown");
        public static RaceCountdown Create(float offset)
        {
            RaceCountdown instance = Instantiate(prefab);
            instance.StartCoroutine(instance.Countdown(offset));
            return instance;
        }

        private float currentFontSize = 60;
        private float targetFontSize = 60;

        [SerializeField] private AudioClip countdown1;
        [SerializeField] private AudioClip countdown2;
        [SerializeField] private Text countdownLabel;

        private void Update()
        {
            currentFontSize = Mathf.Lerp(currentFontSize, targetFontSize, Time.deltaTime * 10);
            countdownLabel.fontSize = (int)currentFontSize;
        }

        private IEnumerator Countdown(float offset)
        {
            yield return new WaitForSeconds(4 + offset);

            countdownLabel.text = "READY";
            targetFontSize = 80;
            UISound.Play(countdown1);
            yield return new WaitForSeconds(1);
            
            countdownLabel.text = "STEADY";
            targetFontSize = 100;
            UISound.Play(countdown1);
            yield return new WaitForSeconds(1);

            countdownLabel.text = "GET SET";
            targetFontSize = 120;
            UISound.Play(countdown1);
            yield return new WaitForSeconds(1);

            countdownLabel.text = "GO FAST";
            targetFontSize = 160;
            UISound.Play(countdown2);
            RaceManager.StartRace();
            ESportMode.StartEMode();
            yield return new WaitForSeconds(2);

            Destroy(gameObject);
        }
    }
}