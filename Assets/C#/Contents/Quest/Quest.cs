using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
퀘스트 이름
퀘스트 설명
이동할 지구 이름
퀘스트 표시 조건
- 미정 
퀘스트 클리어 조건
- 특정 지구 클리어
퀘스트 보상
- 돈
- 아이템
 */
[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Object/Quest")]
public class Quest : ScriptableObject
{
    [field: Header("Information")]
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField, TextArea(3, 10)] public string Description { get; private set; }

    [field: Header("Reward")]
    [field: SerializeField] public SerializedDictionary<Define.QuestRewardType, int> Reward { get; private set; }
}

namespace QuestExtention
{
    public static class RewardExtension
    {
        public static string RewardToString(this SerializedDictionary<Define.QuestRewardType, int> reward)
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