using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_TurnState : UI_Base
{
    [SerializeField] private float _interval = 5;
    private Vector2[] _pivots; // 해당 좌표들로 아이콘 이미지가 배치
    private Image[] _iconImages;
 
    enum Objects
    {
        IconGroup,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(Objects));

        _iconImages = GetGameObject(Objects.IconGroup).GetComponentsInChildren<Image>();

        // pivot 설정
        _pivots = new Vector2[_iconImages.Length];

        for (int i = 0; i < _pivots.Length; i++)
            _pivots[i] = _iconImages[i].rectTransform.anchoredPosition;
    }

    private void ChangeTurnStateUI()
    {
        // 맨 앞에 있는 아이콘 뒤로 이동
        Image firstImage = _iconImages[0];
        firstImage.rectTransform.anchoredPosition = _pivots[_pivots.Length - 1];

        // 앞으로 한칸식 앞당기기
        for (int i = 1; i < _iconImages.Length; i++)
        {
            _iconImages[i].rectTransform.DOAnchorPos(_pivots[i - 1], 0.25f);
            _iconImages[i - 1] = _iconImages[i];
        }
        _iconImages[_iconImages.Length - 1] = firstImage;

        // 첫번째 이미지 크기 늘려주기
        _iconImages[0].rectTransform.DOSizeDelta(firstImage.rectTransform.sizeDelta, 0.25f);
        // 마지막 이미지 크기 되돌리기
        firstImage.rectTransform.sizeDelta = _iconImages[_iconImages.Length - 2].rectTransform.sizeDelta;
    }

    // 테스트 코드
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ChangeTurnStateUI();
        }
    }
}
