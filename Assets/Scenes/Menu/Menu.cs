using System;
using System.Collections;
using System.Collections.Generic;
using Sanicball;
using Sanicball.Logic;
using Sanicball.Scenes;
using SanicballCore;
using UnityEngine;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    private void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        root.RegisterCallback<MouseDownEvent>(evt => {
            Label pressKey = root.Q<Label>("any-key");
            pressKey.visible = false;

            Label title = root.Q<Label>("sanicball");
            StartCoroutine(title.Expand(200));
            StartCoroutine(title.Translate(200, new(-200, 0)));

            MenuCamera? camera = MenuCamera.Instance;
            StartCoroutine(camera?.Resize(200, 400f * root.style.scale.value.value.x));
            
            Label panel = root.Q<Label>("menu-panel");
            panel.Q<Label>("version").text = GameVersion.AS_STRING;
            panel.Q<Label>("tagline").text = GameVersion.TAGLINE;

            
            ListView list = panel.Q<ListView>();
            list.focusable = true;
            list.makeItem = () => new Button(() => Debug.Log("Unbound button"));
            list.bindItem = MenuListItem.BindItem;
            list.itemsSource = MenuListItem.items;
            list.selectionType = SelectionType.Single;

            list.onItemsChosen += Debug.Log;
            list.onSelectionChange += Debug.Log;
        });
    }
}

public record MenuListItem(string Name, Action OnClicked)
{
    public static MenuListItem[] items = {
            new("Local Race", () => MatchStarter.BeginLocalGame()),
            new("Online Race", () => Debug.Log("Unimplemented"))
        };
    
    public static void BindItem(VisualElement el, int i)
    {
        if (el is not Button button) { return; }
        MenuListItem item = items[i];
        
        button.text = item.Name;
        button.clickable = new(item.OnClicked);
    }
}

namespace System.Runtime.CompilerServices
{
    class IsExternalInit {}
}
