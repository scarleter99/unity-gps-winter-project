using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QuestClearCondition;
using System;
using System.Runtime.InteropServices;

/*
퀘스트 이름
퀘스트 설명
퀘스트 표시 조건
- 특정 레벨 도달
- 특정 퀘스트 클리어
- 특정 지역 도달
퀘스트 클리어 조건
- 몬스터 n마리 잡기
- 특정 몬스터 n마리 잡기
- 특정 아이템 소지
- 특정 지역 조사
퀘스트 보상
- 돈
- 경험치
- 아이템
- 스킬
 */
[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest")]
public class Quest : ScriptableObject
{
    [field: Header("Information")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField, TextArea(3, 10)] public string Description { get; private set; }

    [field: Header("Condition")]
    [SerializeField] private KillSpecificMonster _killSpecificMonster;
    [SerializeField] private KillMonsters _killMonsters;
    [SerializeField] private hasSpecificItem _hasSpecificItem;

    private ClearCondition[] _clearCondition;
    public ReadOnlySpan<ClearCondition> ClearCondition { get => new ReadOnlySpan<ClearCondition>(_clearCondition); }

    [field: Header("Reward")]
    [field: SerializeField] public SerializedDictionary<Define.QuestReward, int> Reward { get; private set; }

    private void Awake()
    {
        List<ClearCondition> clearConditions = new List<ClearCondition>()
        {
            _killSpecificMonster,
            _killMonsters,
            _hasSpecificItem,
        };

        _clearCondition = clearConditions.FindAll(x => x.isNull() == false).ToArray();
    }

    public bool isClear()
    {
        foreach (var condition in _clearCondition)
            if (!condition.isClear())
                return false;

        return true;
    }
}

namespace QuestExtention
{
    public static class RewardExtension
    {
        public static string RewardToString(this SerializedDictionary<Define.QuestReward, int> reward)
        {
            string toString = "";

            foreach (var item in reward)
            {
                toString += $"{item.Key} : {item.Value} ";
            }

            return toString;
        }
    }
}