using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ServerManager
{
    private TargetFPS _targetFPS;
    private ServerStartUp _serverStartUp;
    
    public void Init()
    {
        _targetFPS = new TargetFPS();
        _serverStartUp = new ServerStartUp();
        
        _targetFPS.Init();
        _serverStartUp.Init();
    }
}
