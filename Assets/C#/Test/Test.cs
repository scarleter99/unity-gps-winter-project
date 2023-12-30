using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TEST CODE
public class Test : MonoBehaviour
{
    private void Start()
    {
        //TestCube();
        //TestPopupUI();
        //TestSceneUI();
    }

    private void Update()
    {

    }

    public void TestCube()
    {
        List<GameObject> go = new List<GameObject>();
        for (int i = 0; i < 4; i++)
            go.Add(Managers.ResourceMng.Instantiate("Cube"));
    }

    public void TestPopupUI()
    {
        Managers.UIMng.ShowPopupUI<UI_TestPopup>();
    }

    public void TestSceneUI()
    {
        Managers.UIMng.ShowSceneUI<UI_TestInven>();
    }
}