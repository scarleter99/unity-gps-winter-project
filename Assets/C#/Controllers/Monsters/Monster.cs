using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
{
    public Data.MonsterData MonsterData => CreatureData as Data.MonsterData;
    public MonsterStat MonsterStat => (MonsterStat)CreatureStat;
    
    protected override void Init()
    {
        base.Init();

        // TODO - TEST CODE
        Managers.InputMng.KeyAction -= OnKeyboardClick;
        Managers.InputMng.KeyAction += OnKeyboardClick;
    }
    
    public override void SetInfo(int templateId)
    {
        CreatureType = Define.CreatureType.Monster;
        
        base.SetInfo(templateId);
        
        CreatureStat = new MonsterStat(MonsterData);
    }

    public override void DoAction()
    {
        TargetCell = GetRandomHeroCell();

        switch (CurrentAction.ActionAttribute)
        {
            case Define.ActionAttribute.JumpAttack:
                AnimState = Define.AnimState.Attack;
                Debug.Log("JumpAttack");
                break;
        }
    }
    
    BattleGridCell GetRandomHeroCell()
    {
        List<ulong> keysList = new List<ulong>(Managers.ObjectMng.Heroes.Keys);
        ulong randomKey = keysList[Random.Range(0, keysList.Count)];

        return Managers.ObjectMng.Heroes[randomKey].Cell;
    }

    /*----------------------
        TODO - TEST CODE
    ----------------------*/
    protected void OnKeyboardClick()
    {
        if (Input.GetKeyDown(KeyCode.A))
            AnimState = Define.AnimState.Attack;
    }
}
