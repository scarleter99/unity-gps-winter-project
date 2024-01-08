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
        _areaGenerator = _areaName switch
        {
            AreaName.Forest => new ForestAreaGenerator(7, 16, Vector3.zero),
            _ => new ForestAreaGenerator(5, 7, Vector3.zero),
        };
    }

    private void Update()
    {   
        // test
        if (Input.GetKeyDown(KeyCode.M))
        {
            _areaGenerator.GenerateMap();
        }
    }
}
