using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Creature
{
    public Data.MonsterData MonsterData => CreatureData as Data.MonsterData;
    public MonsterStat MonsterStat => (MonsterStat)CreatureStat;

    public override Define.CreatureBattleState CreatureBattleState
    {
        get => base.CreatureBattleState;
        set
        {
            switch (value)
            {
                case Define.CreatureBattleState.Wait:
                    break;
                case Define.CreatureBattleState.Action:
                    DoAction(GetRandomHeroKey(Managers.ObjectMng.Heroes));
                    break;
                case Define.CreatureBattleState.Dead:
                    break;
            }
        }
    }
    
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
    
    ulong GetRandomHeroKey(Dictionary<ulong, Hero> heroDict)
    {
        List<ulong> keysList = new List<ulong>(heroDict.Keys);
        
        int randomIndex = Random.Range(0, keysList.Count);
        ulong randomKey = keysList[randomIndex];

        return randomKey;
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
