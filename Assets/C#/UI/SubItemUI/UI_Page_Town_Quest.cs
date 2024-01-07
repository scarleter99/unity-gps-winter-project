using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Page_Town_Quest : UI_Page
{
    enum GameObjects
    {
        Button_QuestList,
        Button_MyQuestList,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }
}
