using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using UnityEngine.SceneManagement;
using Sanicball.Data;

namespace Sanicball.Scenes
{
    public class Intro : MonoBehaviour
    {
        private static readonly KeyCode[] shortcutKeys = { KeyCode.Space, KeyCode.Return, KeyCode.Escape };
        private void OnEnable()
        {
            UIDocument document = GetComponent<UIDocument>();
            VisualElement root = document.rootVisualElement;
            
            TextField field = root.Q<TextField>("name-input");
            field.RegisterCallback<NavigationSubmitEvent>(evt => {
                string name = field.value;
                if (name.Trim() == "") { return; }

                GameSettings.Instance.nickname = name;

                root.RegisterCallback<KeyDownEvent>(evt => {
                    
                    if (!shortcutKeys.Contains(evt.keyCode)) { return; }

                    SceneManager.LoadScene(Constants.menuName);
                });
                root.RegisterCallback<MouseDownEvent>(evt => {
                    if (evt.button != 0) { return; }

                    SceneManager.LoadScene(Constants.menuName);
                });

                field.visible = false;
                StartCoroutine(IntroCards(root));
            });
        }

        private IEnumerator IntroCards(VisualElement root)
        {
            foreach (Label card in root.Query<Label>().Build())
            {
                StartCoroutine(card.FadeInOut(200));
                yield return new WaitForSeconds(0.4f);
            }

            SceneManager.LoadScene(Constants.menuName);
        }
    }
}

public static class UxmlExtensions
{
    public static IEnumerator FadeInOut(this VisualElement element, int ms)
    {
        element.experimental.animation.Start(0, 1, ms, (img, op) => 
        {
            img.style.opacity = op;
        }).Ease(Easing.InBounce);

        yield return new WaitForSeconds(ms / 1000);

        element.experimental.animation.Start(1, 0, ms, (img, op) => 
        {
            img.style.opacity = op;
        });
    }

    public static IEnumerator Expand(this VisualElement element, int ms)
    {
        for (float time = 0; time < ms; time += Time.deltaTime)
        {
            element.style.width = 20f + element.style.width.value.value;
            element.style.height = 20f + element.style.height.value.value;
            yield return new WaitForFixedUpdate();
        }
    }

    public static IEnumerator Translate(this VisualElement element, int ms, Vector2 translator)
    {
        for (float time = 0; time < ms; time += Time.deltaTime)
        {
            element.style.left = translator.x + element.style.left.value.value;
            element.style.right = translator.x + element.style.right.value.value;
            element.style.top = translator.y + element.style.top.value.value;
            element.style.bottom = translator.y + element.style.bottom.value.value;
            yield return new WaitForFixedUpdate();
        }
    }
}
