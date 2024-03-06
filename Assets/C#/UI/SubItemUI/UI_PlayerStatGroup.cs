using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class UI_PlayerStatGroup : UI_Base
{
    enum PlayerStatUI
    {
        UI_PlayerStat_Player1,
        UI_PlayerStat_Player2,
        UI_PlayerStat_Player3
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(PlayerStatUI));
    }

    void Start()
    {
        Clear();
        BindPlayerUIs();
    }

    private void BindPlayerUIs()
    {
        int index = 0;
        foreach (var hero in Managers.ObjectMng.Heroes.Values)
        {
            var go = GetGameObject((PlayerStatUI)index++);
            go.GetOrAddComponent<UI_PlayerStat>().BindPlayerStat(hero.HeroStat);
            go.SetActive(true);
        }
    }

    private void Clear()
    {
        foreach (PlayerStatUI playerUI in Enum.GetValues(typeof(PlayerStatUI)))
            GetGameObject(playerUI).SetActive(false);
    }
}
