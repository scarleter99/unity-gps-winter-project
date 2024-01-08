using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Page_Town_Quest : UI_Page
{
    private GameObject _currentOpenedObject;

    enum Buttons
    {
        Button_QuestBoard,
        Button_MyQuest,
    }

    enum GameObjects
    {
        QuestBoard,
        MyQuestBoard,
        UI_Quest,
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        GetButton(Buttons.Button_QuestBoard).gameObject.BindEvent(OnClickedQuestBoard);
        GetButton(Buttons.Button_MyQuest).gameObject.BindEvent(OnClickedMyQuestBoard);
    }

    private void OnClickedQuestBoard(PointerEventData data)
    {
        if (_currentOpenedObject == GetGameObject(GameObjects.QuestBoard))
            return;

        _currentOpenedObject?.SetActive(false);

        _currentOpenedObject = GetGameObject(GameObjects.QuestBoard);
        _currentOpenedObject.SetActive(true);
    }

    private void OnClickedMyQuestBoard(PointerEventData data)
    {
        if (_currentOpenedObject == GetGameObject(GameObjects.MyQuestBoard))
            return;

        _currentOpenedObject?.SetActive(false);

        _currentOpenedObject = GetGameObject(GameObjects.MyQuestBoard);
        _currentOpenedObject.SetActive(true);
    }

    private void OnDisable()
    {
        GetGameObject(GameObjects.UI_Quest).SetActive(false);

        _currentOpenedObject?.SetActive(false);
        _currentOpenedObject = null;
    }
}
