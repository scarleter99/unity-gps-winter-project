using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; } = false;
    
    private static Managers s_instance;
    public static Managers Instance { get { Init(); return s_instance; } }

    #region Contents
    private ObjectManager _objectMng = new ObjectManager();
    private BattleManager _battleMng = new BattleManager();
    
    public static ObjectManager ObjectMng => Instance._objectMng;
    public static BattleManager BattleMng => Instance._battleMng;
    #endregion

    #region Core
    private DataManager _dataMng = new DataManager();
    private InputManager _inputMng = new InputManager();
    private PoolManager _poolMng = new PoolManager();
    private ResourceManager _resourceMng = new ResourceManager();
    private SceneManagerEx _sceneMng = new SceneManagerEx();
    private SoundManager _soundMng = new SoundManager();
    private UIManager _uiMng = new UIManager();
    private NetworkManagerEx _networkMng = new NetworkManagerEx();
    private ServerManager _serverMng = new ServerManager();
    private RpcManager _rpcMng = new RpcManager();
    
    public static DataManager DataMng => Instance._dataMng;
    public static InputManager InputMng => Instance._inputMng;
    public static PoolManager PoolMng => Instance._poolMng;
    public static ResourceManager ResourceMng => Instance._resourceMng;
    public static SceneManagerEx SceneMng => Instance._sceneMng;
    public static SoundManager SoundMng => Instance._soundMng;
    public static UIManager UIMng => Instance._uiMng;
    public static NetworkManagerEx NetworkMng => Instance._networkMng;
    public static ServerManager ServerMng => Instance._serverMng;
    public static RpcManager RpcMng => Instance._rpcMng;
    #endregion

    static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;
            
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(go);
            
            s_instance = go.GetComponent<Managers>();
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
