using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BattleScene : UI_Scene
{
	enum SubItemUI
	{
		UI_BattleOrder,
		UI_CoinToss,
	}

    public override void Init()
    {
        base.Init();

		Bind<UI_Base>(typeof(SubItemUI));

		((UI_BattleOrder)Get<UI_Base>(SubItemUI.UI_BattleOrder)).SelectedActionChange += ((UI_CoinToss)Get<UI_Base>(SubItemUI.UI_CoinToss)).ChangeVisibility;
		((UI_BattleOrder)Get<UI_Base>(SubItemUI.UI_BattleOrder)).SelectedActionClick += ((UI_CoinToss)Get<UI_Base>(SubItemUI.UI_CoinToss)).ShowTossResult;
    }
}
