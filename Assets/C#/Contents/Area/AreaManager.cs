using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AreaManager : MonoBehaviour
{
       
    public AreaName _areaName;
    private AreaGenerator _areaGenerator;

    private void Start()
    {
        _areaGenerator = new AreaGenerator(_areaName, Vector3.zero);
        _areaGenerator.GenerateMap();
    }

    private void Update()
    {   
        // test
        if (Input.GetKeyDown(KeyCode.M))
        {   
            Destroy(GameObject.Find("Area_1_Map"));
            _areaGenerator = new AreaGenerator(_areaName, Vector3.zero);
            _areaGenerator.GenerateMap();
        }
    }
}
 