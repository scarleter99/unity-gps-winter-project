using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TestClientButton : UI_Popup
{
    enum Buttons
    {
        ClientButton
    }

    private MatchmakerClient _mmc;
    public override void Init()
    {
        base.Init();
        
        Bind<Button>(typeof(Buttons));
        
        GetButton(Buttons.ClientButton).gameObject.BindEvent(OnButtonClicked, Define.UIEvent.Click);
    }
    
    public void OnButtonClicked(PointerEventData data)
    {
        Debug.Log("TestClientButton Clicked!");
    }
}
