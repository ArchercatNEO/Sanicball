using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Sanicball.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SlideCanvasGroup : MonoBehaviour
    {
        //editor variables 
        [SerializeField] private bool isOpen = false;
        [SerializeField] private float time = 1f;
        
        //events
        public UnityEvent onOpen;
        public UnityEvent onClose;

        //components
        private CanvasGroup cg;
        private RectTransform rectTransform;
        
        //start positions
        private Vector2 startPosition;
        [SerializeField] private Vector2 closedPosition;

        // Use this for initialization
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            startPosition = rectTransform.anchoredPosition;
            
            cg = GetComponent<CanvasGroup>();
            
            if (isOpen) { Open(); }
            else { Close(); }
        }

        public void Open()
        {
            StopCoroutine(CloseInternal());
            StartCoroutine(OpenInternal());
        }

        private IEnumerator OpenInternal()
        {
            gameObject.SetActive(true);
            cg.alpha = 1f;
            cg.interactable = true;
            onOpen.Invoke();
            
            for (float pos = 0; pos < time; pos += Time.deltaTime)
            {
                var smoothedPos = Mathf.SmoothStep(0f, 1f, pos / time);
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition + closedPosition, startPosition, smoothedPos);
                yield return new WaitForFixedUpdate();
            }
        }

        public void Close()
        {
            StopCoroutine(OpenInternal());
            StartCoroutine(CloseInternal());
        }

        private IEnumerator CloseInternal()
        {
            cg.interactable = false;
            onClose.Invoke();
            
            for (float pos = 0; pos < time; pos += Time.deltaTime)
            {
                var smoothedPos = Mathf.SmoothStep(0f, 1f, pos / time);
                rectTransform.anchoredPosition = Vector2.Lerp(startPosition + closedPosition, startPosition, smoothedPos);
                yield return new WaitForFixedUpdate();
            }

            cg.alpha = 0f;
            gameObject.SetActive(false);
        }
    }
}
