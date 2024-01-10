using DG.Tweening;
using QuestExtention;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_QuestBoard : UI_Base
{
    private class UI_QuestBoard_Quest : UI_Base
    {
        enum Texts
        {
            QuestName,
        }

        public override void Init()
        {
            Bind<TextMeshProUGUI>(typeof(Texts));
        }

        public void SetQuest(Quest quest)
        {
            GetTextMeshProUGUI(Texts.QuestName).text = quest.Name;
        }
    }

    private class UI_Quest : UI_Base
    {
        enum Buttons
        {
            AcceptButton,
            CancelButton,
        }

        enum Texts
        {
            Title,
            Description,
            Reward,
        }

        public override void Init()
        {
            Bind<Button>(typeof(Buttons));
            Bind<TextMeshProUGUI>(typeof(Texts));

            GetButton(Buttons.CancelButton).gameObject.BindEvent(OnClickedCancelButton, Define.UIEvent.Click);
        }

        public void SetQuest(Quest _quest)
        {
            GetTextMeshProUGUI(Texts.Title).text = _quest.Name;

            GetTextMeshProUGUI(Texts.Description).text = _quest.Description + '\n';
            GetTextMeshProUGUI(Texts.Reward).text = "Reward: " + _quest.Reward.RewardToString();

            void OnClickedAccpetButton(PointerEventData eventData)
            {
                // 지구 이동
                this.gameObject.SetActive(false);
            }

            GetButton(Buttons.AcceptButton).gameObject.BindEvent(OnClickedAccpetButton, Define.UIEvent.Click);
        }

        private void OnClickedCancelButton(PointerEventData eventData)
        {
            this.gameObject.SetActive(false);
        }
    }

    enum GameObjects
    {
        QuestBoard_Content,
        UI_Quest,
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
    }

    private void OnEnable()
    {
        this.GetComponent<RectTransform>().DOLocalMoveY(1000, 0.5f).From(true).SetEase(Ease.OutCirc).SetDelay(0.05f);

        GameObject content = GetGameObject(GameObjects.QuestBoard_Content);
        foreach (Transform child in content.transform)
            Managers.ResourceMng.Destroy(child.gameObject);

        // 퀘스트 목록 받아오기
        for (int i = 0; i < 10; i++)
        {
            Quest quest = Managers.ResourceMng.Load<Quest>("ScriptableObjects/Quest/Quest"); // 테스트용 코드

            UI_QuestBoard_Quest questBoard_Quest = Managers.UIMng.MakeSubItemUI<UI_QuestBoard_Quest>(content.transform);
            questBoard_Quest.SetQuest(quest);

            void OnClicked(PointerEventData eventData)
            {
                GameObject ui_QuestObj = GetGameObject(UI_QuestBoard.GameObjects.UI_Quest);

                ui_QuestObj.SetActive(true);

                UI_Quest ui_Quest = ui_QuestObj.GetOrAddComponent<UI_Quest>();
                ui_Quest.SetQuest(quest);
            }

            questBoard_Quest.gameObject.BindEvent(OnClicked, Define.UIEvent.Click);
        }
    }
}
