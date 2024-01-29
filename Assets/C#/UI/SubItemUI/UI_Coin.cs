using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Coin : UI_Base
{
    enum Icon
    {
        SuccessIcon,
        FailedIcon,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(Icon));
    }

    public void DisplayIcon(bool isSuccess)
    {
        Clear();

        if (isSuccess)
            GetGameObject(Icon.SuccessIcon).SetActive(true);

        else
            GetGameObject(Icon.FailedIcon).SetActive(true);
    }

    public void Clear()
    {
        GetGameObject(Icon.SuccessIcon).SetActive(false);
        GetGameObject(Icon.FailedIcon).SetActive(false);
    }
}
