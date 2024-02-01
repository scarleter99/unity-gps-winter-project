using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyStat : UI_Base
{
    private event Action OnClaer;

    enum Text
    {
        Text_Name,

        Text_HP,
        Text_Attack,
        Text_Defense,
        Text_Speed,
    }

    enum Sliders
    {
        Slider_HP,
    }

    enum Image
    {
        MonsterPicture,
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Text));
        Bind<UnityEngine.UI.Slider>(typeof(Slider));
        Bind<UnityEngine.UI.Image>(typeof(Image));
    }

    public void ConnectPlayerStat(MonsterStat stat)
    {
        stat.OnStatChanged += ChangeMonsterStatUI;

        OnClaer?.Invoke();
        OnClaer = null;
        OnClaer += () => stat.OnStatChanged -= ChangeMonsterStatUI;
    }

    private void ChangeMonsterStatUI(MonsterStat monsterStat)
    {
        GetText(Text.Text_Name).text = monsterStat.Name;

        Get<Slider>(Sliders.Slider_HP).value = monsterStat.Hp / monsterStat.MaxHp;
        GetText(Text.Text_HP).text = $"{monsterStat.Hp}/{monsterStat.MaxHp}";
        GetText(Text.Text_Attack).text = monsterStat.Attack.ToString();
        GetText(Text.Text_Defense).text = monsterStat.Defense.ToString();
        GetText(Text.Text_Speed).text = monsterStat.Speed.ToString();

        //Get<Image>(Images.UserPicture).sprite = monsterStat.Texture;
    }

    private void OnDestroy()
    {
        OnClaer?.Invoke();
    }
}
