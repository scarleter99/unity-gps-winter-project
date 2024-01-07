using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class AreaManager : MonoBehaviour
{
       
    public AreaName _areaName;
    private AreaGenerator _areaGenerator;

    void Start()
    {
        switch (_areaName)
        {
            case AreaName.Forest:
                _areaGenerator = new ForestAreaGenerator(7, 17, Vector3.zero);
                break;
            default:
                _areaGenerator = new ForestAreaGenerator(5, 7, Vector3.zero);
                break;
        }
    }

    void Update()
    {   
        // test
        if (Input.GetKeyDown(KeyCode.M))
        {
            _areaGenerator.GenerateMap();
        }
    }
}
