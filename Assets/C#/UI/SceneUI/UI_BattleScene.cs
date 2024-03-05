using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_BattleScene : UI_Scene
{
	enum SubItemUI
	{
		UI_BattleOrder,
		UI_CoinToss,
	}

	public UI_BattleOrder BattleOrderUI { get; protected set; }
	public UI_CoinToss CoinTossUI { get; protected set; }
	
    public override void Init()
    {
        base.Init();

		Bind<UI_Base>(typeof(SubItemUI));

		BattleOrderUI = Get<UI_Base>(SubItemUI.UI_BattleOrder).GetOrAddComponent<UI_BattleOrder>();
		BattleOrderUI.gameObject.SetActive(false);
		CoinTossUI = Get<UI_Base>(SubItemUI.UI_CoinToss).GetOrAddComponent<UI_CoinToss>();
    }
}
