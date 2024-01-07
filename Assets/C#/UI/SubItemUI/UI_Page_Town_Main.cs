using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Page_Town_Main : UI_Page
{
    enum GameObjects
    {
        Button_Quest,
        Button_Store,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }
}
