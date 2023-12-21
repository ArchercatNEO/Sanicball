using UnityEngine;

namespace Sanicball.Scenes
{
    public class MenuUI : MonoBehaviour
    {
        private Title title;
        private ImageElement segaCredit;
        private ImageElement sanicCredit;
        private TextElement pressAnyKey;

        private bool started = false;

        // Start is called before the first frame update
        void Start()
        {
            title = new Title(gameObject);
            segaCredit = new ImageElement("Not Sega", TextureFactory.NotSega);
            sanicCredit = new ImageElement("Sanic Team", TextureFactory.SanicTeam);


            pressAnyKey = new TextElement("Press any key", "Press Any Key (Or click)");

            Parent.SetParent(segaCredit, gameObject);
            Parent.SetParent(sanicCredit, gameObject);
            Parent.SetParent(pressAnyKey, gameObject);

            Translation.Center(pressAnyKey);


            /*
            * On key press
            * Start spinning title
            * Open slide canvas
            * Resize Menu cameras
            */
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKeyDown)
            {
                started = true;
                new MenuPanel(gameObject);
                Destroy(pressAnyKey.gameObject);
            }

            if (started)
            {
                title.Spin();
            }
        }
    }
}
