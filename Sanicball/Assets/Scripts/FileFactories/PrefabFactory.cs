using Sanicball;
using Sanicball.Gameplay;
using Sanicball.UI;
using UnityEngine;

public static class PrefabFactory
{


    

    public static Rigidbody ballPrefab => Resources.Load<Rigidbody>("Prefabs/Instantiated/Ball");
}
