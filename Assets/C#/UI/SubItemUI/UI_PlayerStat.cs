using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerStat : UI_Base
{
    private event Action OnClaer;

    enum Texts
    {
        Text_Name,
        Text_HP,
        Text_Strength,
        Text_Vitality,
        Text_Intelligence,
        Text_Gold,
    }

    enum Buttons
    {
        Button_Inventory,
    }

    enum Sliders
    {
        Slider_HP,
    }

    enum Images
    {
        UserPicture,
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<Slider>(typeof(Sliders));
        Bind<Image>(typeof(Images));
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
        Get<Slider>(Sliders.Slider_HP).value = playerStat.Hp / playerStat.MaxHp;
        GetText(Texts.Text_HP).text = $"{playerStat.Hp}/{playerStat.MaxHp}";
        GetText(Texts.Text_Name).text = playerStat.Name;
        GetText(Texts.Text_Strength).text = playerStat.Strength.ToString();
        GetText(Texts.Text_Vitality).text = playerStat.Vitality.ToString();
        GetText(Texts.Text_Intelligence).text = playerStat.Intelligence.ToString();
        //GetText(Texts.Text_Gold).text = playerStat.Gold.ToString();
        //Get<Image>(Images.UserPicture).sprite = playerStat.Texture;
    }

    private void OnDestroy()
    {
        OnClaer?.Invoke();
    }
}
