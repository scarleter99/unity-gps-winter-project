using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : UI_Base
{
    private event Action OnClaer;

    enum Text
    {
        Text_Name,

        Text_HP,
        Text_Attack,
        Text_Defense,
        Text_Speed,

        Text_Strength,
        Text_Vitality,
        Text_Intelligence,
        Text_Dexterity,

        Text_Gold,
    }

    enum Button
    {
        Button_Inventory,
    }

    enum Slider
    {
        Slider_HP,
    }

    enum Image
    {
        UserPicture,
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Text));
        Bind<UnityEngine.UI.Button>(typeof(Button));
        Bind<UnityEngine.UI.Slider>(typeof(Slider));
        Bind<UnityEngine.UI.Image>(typeof(Image));
    }

    public void ConnectPlayerStat(PlayerStat stat)
    {
        stat.OnStatChanged += ChangePlayerStatUI;

        OnClaer?.Invoke();
        OnClaer = null;
        OnClaer += () => stat.OnStatChanged -= ChangePlayerStatUI;
    }

    private void ChangePlayerStatUI(PlayerStat playerStat)
    {
        GetText(Texts.Text_Name).text = playerStat.Name;

        Get<Slider>(Sliders.Slider_HP).value = playerStat.Hp / playerStat.MaxHp;
        GetText(Texts.Text_HP).text = $"{playerStat.Hp}/{playerStat.MaxHp}";
        GetText(Texts.Text_Attack).text = playerStat.Attack.ToString();
        GetText(Texts.Text_Defense).text = playerStat.Defense.ToString();
        GetText(Texts.Text_Speed).text = playerStat.Speed.ToString();

        GetText(Texts.Text_Strength).text = playerStat.Strength.ToString();
        GetText(Texts.Text_Vitality).text = playerStat.Vitality.ToString();
        GetText(Texts.Text_Intelligence).text = playerStat.Intelligence.ToString();
        GetText(Texts.Text_Dexterity).text = playerStat.Dexterity.ToString();
        //GetText(Texts.Text_Gold).text = playerStat.Gold.ToString();
        //Get<Image>(Images.UserPicture).sprite = playerStat.Texture;
    }

    private void OnDestroy()
    {
        OnClaer?.Invoke();
    }
}
