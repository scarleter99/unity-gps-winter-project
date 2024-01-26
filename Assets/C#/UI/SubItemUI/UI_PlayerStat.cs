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
        Text_Strength,
        Text_Vitality,
        Text_Intelligence,
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
        Get<UnityEngine.UI.Slider>(Slider.Slider_HP).value = playerStat.Hp / playerStat.MaxHp;
        GetText(Text.Text_HP).text = $"{playerStat.Hp}/{playerStat.MaxHp}";
        GetText(Text.Text_Name).text = playerStat.Name;
        GetText(Text.Text_Strength).text = playerStat.Strength.ToString();
        GetText(Text.Text_Vitality).text = playerStat.Vitality.ToString();
        GetText(Text.Text_Intelligence).text = playerStat.Intelligence.ToString();
        //GetText(Texts.Text_Gold).text = playerStat.Gold.ToString();
        //Get<Image>(Images.UserPicture).sprite = playerStat.Texture;
    }

    private void OnDestroy()
    {
        OnClaer?.Invoke();
    }
}
