using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sanicball.Data;

namespace Sanicball.Scenes
{
    class IntroUI : MonoBehaviour
    {
        public Sprite box;
        private ImageElement notSega;
        private ImageElement sanicTeam;
        private InputGroup usernameForm;

        private void Start()
        {
            enabled = false;
            notSega = new ImageElement("Not Sega", TextureFactory.NotSega);
            sanicTeam = new ImageElement("Sanic Team", TextureFactory.SanicTeam);
            usernameForm = new InputGroup("Nickname input", box, "Enter your username (for online)");

            Parent.SetParent(notSega, gameObject);
            Parent.SetParent(sanicTeam, gameObject);
            Parent.SetParent(usernameForm, gameObject);

            Translation.Center(notSega);
            Translation.Center(sanicTeam);
            Translation.Center(usernameForm);

            notSega.Component.enabled = false;
            sanicTeam.Component.enabled = false;

            usernameForm.input.Component.onSubmit.AddListener(name =>
            {
                if (name.Trim() != "")
                {
                    GameSettings.Instance.nickname = name;
                    usernameForm.Component.Component.alpha = 0f;
                    enabled = true;
                    Animate();
                }
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(Constants.menuName);
            }
        }

        private async void Animate()
        {
            await Fade.FadeIn(notSega, 0.1f);
            await Task.Delay(100);
            await Fade.FadeOut(notSega, 0.1f);
            await Fade.FadeIn(sanicTeam, 0.1f);
            await Task.Delay(100);
            await Fade.FadeOut(sanicTeam, 0.1f);
            SceneManager.LoadScene(Constants.menuName);
        }
    }
}