using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{
    private List<Dictionary<ulong, GameObject>> _gameObjectDics = new List<Dictionary<ulong, GameObject>>(); // Unknown타입의 Dic은 생성하지 않음
    private List<ulong> _lastIds = new List<ulong>(); // id는 1부터 시작
    
    public Dictionary<ulong, Hero> Heroes { get; } = new();
    public Dictionary<ulong, Monster> Monsters { get; } = new();
    
    public ulong HeroLastId = 1000;
    public ulong MonsterLastId = 2000;

    
    #region Roots
    public Transform GetRootTransform(string name)
    {
	    GameObject root = GameObject.Find(name);
	    if (root == null)
		    root = new GameObject { name = name };

	    return root.transform;
    }

    public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform GridRoot { get { return GetRootTransform("@Grid"); } }
    #endregion
    
    public T Spawn<T>(int dataId) where T : Creature
	{
		string prefabName = typeof(T).Name;

		GameObject go = Managers.ResourceMng.Instantiate(prefabName);
		go.name = prefabName;
		go.transform.position = Vector3.zero;
		
		Creature creature = go.GetComponent<Creature>();

		switch (creature.CreatureType)
		{
			case Define.CreatureType.Hero:
				creature.transform.parent = HeroRoot;
				Hero hero = creature as Hero;
				Heroes[++HeroLastId] = hero;
				creature.Id = HeroLastId;
				break;
			case Define.CreatureType.Monster:
				creature.transform.parent = MonsterRoot;
				Monster monster = creature as Monster;
				Monsters[++MonsterLastId] = monster;
				creature.Id = MonsterLastId;
				break;
		}

		creature.SetInfo(dataId);

		return creature as T;
	}

    public void Despawn(Define.CreatureType creatureType, ulong id)
    {
	    Creature creature = null;
		switch (creatureType)
		{
			case Define.CreatureType.Hero:
				creature = Heroes[id];
				Heroes.Remove(id);
				break;
			case Define.CreatureType.Monster:
				creature = Heroes[id];
				Monsters.Remove(id);
				break;
		}
		
		if (creature != null)
			Managers.ResourceMng.Destroy(creature.gameObject);
	}

    public void StatChange(Define.CreatureType creatureType, ulong id, IStat statStruct)
    {
	    switch (creatureType)
	    {
		    case Define.CreatureType.Hero:
			    Heroes[id].ChangeStat(statStruct);
			    break;
		    case Define.CreatureType.Monster:
			    Monsters[id].ChangeStat(statStruct);
			    break;
		    default:
			    Debug.Log("Failed to ChangeStat");
			    break;
	    }
    }
}
