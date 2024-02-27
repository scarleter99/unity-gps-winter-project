using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BattleScene : BaseScene
{
    protected override void Init()
    {
        base.Init();
        
        SceneType = Define.SceneType.BattleScene;
        
        // TODO - TEST CODE
        Managers.ObjectMng.SpawnHero(Define.HERO_KNIGHT_ID);
        Managers.ObjectMng.SpawnHero(Define.HERO_KNIGHT_ID);
        Managers.ObjectMng.SpawnHero(Define.HERO_KNIGHT_ID);

        Managers.BattleMng.InitBattle(Define.MONSTERSQUAD_Squad1_ID);
        Managers.UIMng.ShowSceneUI<UI_BattleScene>();
    }

    public override void Clear()
    {
        Debug.Log("BattleScene Clear!");
    }
}
