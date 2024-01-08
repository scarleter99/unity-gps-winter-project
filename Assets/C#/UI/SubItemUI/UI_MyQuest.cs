using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MyQuest : UI_Base
{
    public override void Init()
    {

    }

    private void OnEnable()
    {
        this.GetComponent<RectTransform>().DOLocalMoveY(600, 0.5f).From(true).SetEase(Ease.OutCirc).SetDelay(0.05f);
    }
}
