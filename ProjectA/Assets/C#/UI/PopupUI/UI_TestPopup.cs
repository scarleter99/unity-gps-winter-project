using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TestPopup : UI_Popup
{
    enum Buttons
    {
        MoneyButton
    }

    enum Texts
    {
        MoneyButtonText,
        MoneyText
    }

    enum GameObjects
    {
        TestObject
    }

    enum Images
    {
        ItemIcon
    }

    private int _money = 0;

    public override void Init()
    {
        base.Init();
        
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        
        GetButton(Buttons.MoneyButton).gameObject.BindEvent(OnButtonClicked, Define.UIEvent.Click);
        GetImage(Images.ItemIcon).gameObject.BindEvent(OnDrag, Define.UIEvent.Drag);
    }
    
    public void OnButtonClicked(PointerEventData data)
    {
        _money++;
        GetText(Texts.MoneyText).text = $"Money: {_money}!";
    }

    public void OnDrag(PointerEventData data)
    {
        GetImage(Images.ItemIcon).transform.position = data.position;
    }
}
