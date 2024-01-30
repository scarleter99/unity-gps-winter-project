using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene => GameObject.FindObjectOfType<BaseScene>();

    // type의 이름을 string으로 반환
    private string GetSceneName(Define.SceneType type)
    {
        return System.Enum.GetName(typeof(Define.SceneType), type);
    }

    // type에 해당하는 Scene을 로드
    public void LoadScene(Define.SceneType type)
    {
        Managers.Clear();
        
        SceneManager.LoadScene(GetSceneName(type));
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
