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

    private PlayerController _player;
    private MonsterController _monster;
    
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        var controller = transform.parent.GetComponent<BaseController>();
        switch (controller.WorldObjectType)
        {
            case Define.WorldObject.Player:
                _player = controller as PlayerController;
                _monster = null;
                break;
            case Define.WorldObject.Monster:
                _monster = controller as MonsterController;
                _player = null;
                break;
        }
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;
        if (_player != null)
            SetHpRatio(_player.Stat.Hp / (float)_player.Stat.MaxHp);
        else if (_monster != null)
            SetHpRatio(_monster.Stat.Hp / (float)_monster.Stat.MaxHp);
    }

    public void SetHpRatio(float ratio)
    {
        GetGameObject(GameObjects.HpBar).GetComponent<Slider>().value = ratio;
    }
}
