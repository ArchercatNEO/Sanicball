using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class Fade
{
    public static async Task FadeIn(IElement<Image> element, float time)
    {
        element.Component.enabled = true;
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / time;
            element.Component.color = new Color(1f, 1f, 1f, alpha);
            await Task.Delay(33);
        }
    }

    public static async Task FadeOut(IElement<Image> element, float time)
    {
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / time;
            element.Component.color = new Color(1f, 1f, 1f, alpha);
            await Task.Delay(33);
        }
        element.Component.enabled = false;
    }
}
