using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Page_Town_Store : UI_Page
{
    enum Buttons
    {
        Button_Purchase,
        Button_Sell,
    }

    enum GameObjects
    {
        UI_PurchaseBoard,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        void OnClickedPurchaseButton()
        {
            GetGameObject(GameObjects.UI_PurchaseBoard).SetActive(true);
        }

        GetButton(Buttons.Button_Purchase).onClick.AddListener(OnClickedPurchaseButton);
   }
}
