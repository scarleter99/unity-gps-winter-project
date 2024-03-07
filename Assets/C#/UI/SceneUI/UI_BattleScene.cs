using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UI_BattleScene : UI_Scene
{
	enum SubItemUI
	{
		UI_BattleOrder,
		UI_CoinToss,
		UI_TurnState,
        UI_BattleVictory
	}

    public override void Init()
    {
        base.Init();

		Bind<UI_Base>(typeof(SubItemUI));

		((UI_BattleOrder)Get<UI_Base>(SubItemUI.UI_BattleOrder)).SelectedActionChange += ((UI_CoinToss)Get<UI_Base>(SubItemUI.UI_CoinToss)).ChangeVisibility;
		((UI_BattleOrder)Get<UI_Base>(SubItemUI.UI_BattleOrder)).SelectedActionClick += ((UI_CoinToss)Get<UI_Base>(SubItemUI.UI_CoinToss)).ShowTossResult;
    }

    public void EndBattle(Define.BattleResultType battleResult)
    {
        switch (battleResult)
        {
            case Define.BattleResultType.Victory:
                Get<UI_Base>(SubItemUI.UI_BattleOrder).gameObject.SetActive(false);
                Get<UI_Base>(SubItemUI.UI_CoinToss).gameObject.SetActive(false);

                // Turn 상태바 움직임을 통해 자연스럽게 숨기기
                Transform turnStateUI = Get<UI_Base>(SubItemUI.UI_TurnState).gameObject.transform;
                turnStateUI.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 90), 1f).OnComplete(() =>
                {
                    turnStateUI.gameObject.SetActive(false);
                    Get<UI_Base>(SubItemUI.UI_BattleVictory).gameObject.SetActive(true);
                });
                break;
            case Define.BattleResultType.Defeat:
                break;
            case Define.BattleResultType.Flee:
                break;
        }
    }
}
