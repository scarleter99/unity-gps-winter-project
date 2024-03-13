using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ObjectManager
{
	public Dictionary<ulong, Hero> Heroes { get; protected set; }
    public Dictionary<ulong, Monster> Monsters { get; protected set; }
    
    public ulong NextHeroId;
    public ulong NextMonsterId;

    public Transform HeroRoot => GetRootTransform("@Heroes");
    public Transform MonsterRoot => GetRootTransform("@Monsters");
    
    public void Init()
    {
	    Heroes = new Dictionary<ulong, Hero>();
	    Monsters = new Dictionary<ulong, Monster>();
	    NextHeroId = 10000;
	    NextMonsterId = 20000;
    }

    public Transform GetRootTransform(string name)
    {
	    GameObject root = GameObject.Find(name);
	    if (root == null)
		    root = new GameObject { name = name };

	    return root.transform;
    }

    public Hero SpawnHero(int heroDataId)
    {
	    string className = Managers.DataMng.HeroDataDict[heroDataId].Name;
		GameObject go = Managers.ResourceMng.Instantiate($"{Define.HERO_PATH}/{className}");
		Hero hero = go.GetComponent<Hero>();
		
		hero.SetInfo(heroDataId);
		go.transform.position = Vector3.zero;
		hero.transform.parent = HeroRoot;
		hero.Id = NextHeroId;
		Heroes[NextHeroId++] = hero;
		
		return hero;
	}
    
    public Monster SpawnMonster(int monsterDataId)
    {
	    string className = Managers.DataMng.MonsterDataDict[monsterDataId].Name;
	    GameObject go = Managers.ResourceMng.Instantiate($"{Define.MONSTER_PATH}/{className}");
	    Monster monster = go.GetComponent<Monster>();
		
	    monster.SetInfo(monsterDataId);
	    go.transform.position = Vector3.zero;
	    monster.transform.parent = MonsterRoot;
	    monster.Id = NextMonsterId;
	    Monsters[NextMonsterId++] = monster;
		
	    return monster;
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

    public Creature GetCreatureWithId(ulong id)
    {
	    Creature creature = null;
	    if (Managers.ObjectMng.Heroes.TryGetValue(id, out Hero hero))
		    creature = hero;
	    if (Managers.ObjectMng.Monsters.TryGetValue(id, out Monster monster))
		    creature = monster;

	    return creature;
    }
    
    public CreatureData GetCreatureDataWithDataId(int dataId)
    {
	    CreatureData creatureData = null;
	    if (Managers.DataMng.HeroDataDict.TryGetValue(dataId, out HeroData heroData))
		    creatureData = heroData;
	    if (Managers.DataMng.MonsterDataDict.TryGetValue(dataId, out MonsterData monsterData))
		    creatureData = monsterData;

	    return creatureData;
    }
}
