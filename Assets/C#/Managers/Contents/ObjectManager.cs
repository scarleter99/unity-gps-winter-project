using System.Collections.Generic;
using UnityEngine;

public class ObjectManager
{ 
	public Dictionary<ulong, Hero> Heroes { get; protected set; }
    public Dictionary<ulong, Monster> Monsters { get; protected set; }
    
    public ulong NextHeroId;
    public ulong NextMonsterId;
    
    public void Init()
    {
	    Heroes = new Dictionary<ulong, Hero>();
	    Monsters = new Dictionary<ulong, Monster>();
	    NextHeroId = 10000;
	    NextMonsterId = 20000;
    }
    
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
    #endregion
    
    public T Spawn<T>(int dataId, string path) where T : Creature
	{
		string prefabName = typeof(T).Name;

		GameObject go = Managers.ResourceMng.Instantiate($"{path}/{prefabName}");
		go.name = prefabName;
		go.transform.position = Vector3.zero;
		
		Creature creature = go.GetComponent<Creature>();
		creature.SetInfo(dataId);
		
		switch (creature.CreatureType)
		{
			case Define.CreatureType.Hero:
				creature.transform.parent = HeroRoot;
				Hero hero = creature as Hero;
				Heroes[NextHeroId++] = hero;
				creature.Id = NextHeroId;
				break;
			case Define.CreatureType.Monster:
				creature.transform.parent = MonsterRoot;
				Monster monster = creature as Monster;
				Monsters[NextMonsterId++] = monster;
				creature.Id = NextMonsterId;
				break;
		}
		
		return creature as T;
	}

    public T SpawnHero<T>(int dataId) where T : Hero
    {
	    return Spawn<T>(dataId, "Heroes");
    }
    
    public T SpawnMonster<T>(int dataId) where T : Monster
    {
	    return Spawn<T>(dataId, "Monsters");
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
