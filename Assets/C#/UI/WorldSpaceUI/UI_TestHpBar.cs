using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TestHpBar : UI_Base
{
    enum GameObjects
    {
        HpBar
    }

    private Hero _hero;
    private Monster _monster;
    
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        var controller = transform.parent.GetComponent<Creature>();
        switch (controller.CreatureType)
        {
            case Define.CreatureType.Hero:
                _hero = controller as Hero;
                _monster = null;
                break;
            case Define.CreatureType.Monster:
                _monster = controller as Monster;
                _hero = null;
                break;
        }
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
        if (_hero != null)
            SetHpRatio(_hero.Stat.Hp / (float)_hero.Stat.MaxHp);
        else if (_monster != null)
            SetHpRatio(_monster.Stat.Hp / (float)_monster.Stat.MaxHp);
    }

    public void SetHpRatio(float ratio)
    {
        GetGameObject(GameObjects.HpBar).GetComponent<Slider>().value = ratio;
    }
}
