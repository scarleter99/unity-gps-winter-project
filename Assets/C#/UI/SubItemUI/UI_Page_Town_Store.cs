using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Page_Town_Store : UI_Page
{
    enum GameObjects
    {
        Button_Buy,
        Button_Sell,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }
}
