using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

#region Contents
    private GameManager _gameMng = new GameManager();
    
    public static GameManager GameMng => Instance._gameMng;
#endregion

#region Core
    private DataManager _dataMng = new DataManager();
    private InputManager _inputMng = new InputManager();
    private PoolManager _poolMng = new PoolManager();
    private ResourceManager _resourceMng = new ResourceManager();
    private SceneManagerEx _sceneMng = new SceneManagerEx();
    private SoundManager _soundMng = new SoundManager();
    private UIManager _uiMng = new UIManager();
    
    public static DataManager DataMng => Instance._dataMng;
    public static InputManager InputMng => Instance._inputMng;
    public static PoolManager PoolMng => Instance._poolMng;
    public static ResourceManager ResourceMng => Instance._resourceMng;
    public static SceneManagerEx SceneMng => Instance._sceneMng;
    public static SoundManager SoundMng => Instance._soundMng;
    public static UIManager UIMng => Instance._uiMng;
#endregion

    private void Start()
    {
        Init();
    }
    
    private void Update()
    {
        _inputMng.OnUpdate();
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._dataMng.Init();
            s_instance._soundMng.Init();
            s_instance._poolMng.Init();
        }
    }
    
    public static void Clear()
    {
        InputMng.Clear();
        SoundMng.Clear();
        SceneMng.Clear();
        UIMng.Clear();
        PoolMng.Clear();
    }
}
