using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace QuestClearCondition
{
    [System.Serializable]
    public abstract class ClearCondition
    {
        public abstract bool isClear();
        public abstract override string ToString(); // 현재 진행된 퀘스트 정보를 출력
        public abstract bool isNull();
    }

    [System.Serializable] // 몬스터 종류에 상관없이 몬스터 처치
    public class KillMonsters : ClearCondition
    {
        [SerializeField] private int _killCount;
        private int _currentCount;

        public override bool isClear()
        {
            return _currentCount >= _killCount;
        }

        // 해당 함수는 이벤트 콜백을 이용해서 호출
        private void AddCount()
        {
            _currentCount++;
        }

        public override string ToString()
        {
            return $"{_currentCount} / {_killCount}";
        }

        public override bool isNull() => _killCount == 0;
    }

    [System.Serializable] // 특정 몬스터 처치
    public class KillSpecificMonster : ClearCondition
    {
        [SerializedDictionary("MonsterName", "ConditionCount")]
        [SerializeField] private SerializedDictionary<Define.MonsterName, int[]> _killCount; // 0번 인덱스: 조건 카운트, 1번 인덱스: 현재 카운트

        public override bool isClear()
        {
            foreach (var counts in _killCount.Values)
                if (counts[0] > counts[1])
                    return false;

            return true;
        }

        public void AddCount(Define.MonsterName monsterName)
        {
            if (_killCount.TryGetValue(monsterName, out var counts))
            {
                counts[1]++;
            }
        }

        public override string ToString()
        {
            string toString = "";

            foreach (var item in _killCount)
                toString += $"{item.Key}: {item.Value[0]}\n";

            return toString;
        }

        public override bool isNull() => _killCount.Count == 0;
    }

    [System.Serializable]
    public class hasSpecificItem : ClearCondition
    {
        [SerializedDictionary("ItemName", "ConditionCount")]
        [SerializeField] private SerializedDictionary<Define.ItemName, int[]> _havingCount; // 0번 인덱스: 조건 카운트, 1번 인덱스: 현재 카운트

        public override bool isClear()
        {
            foreach (var counts in _havingCount.Values)
                if (counts[0] > counts[1])
                    return false;

            return true;
        }

        public override string ToString()
        {
            return "";
        }

        public override bool isNull() => _havingCount.Count == 0;
    }
}

