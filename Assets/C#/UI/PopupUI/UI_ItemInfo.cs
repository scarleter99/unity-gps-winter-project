using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemInfo : UI_Popup
{
    public RectTransform RectTransform {  get; private set; }

    enum GameObjects
    {
        Panel,
    }

	enum Text
	{
		ItemName,
		ItemDescription,
	}

    public override void Init()
    {
        base.Init();

        Bind<TextMeshProUGUI>(typeof(Text));
        Bind<GameObject>(typeof(GameObjects));
        RectTransform = GetGameObject(GameObjects.Panel).GetOrAddComponent<RectTransform>();
    }

    public void Init(UI_PurchaseBoard.TestItem testItem)
    {
        GetText(Text.ItemName).text = testItem.ItemName;
        GetText(Text.ItemDescription).text = testItem.ItemDescription;
    }
}
