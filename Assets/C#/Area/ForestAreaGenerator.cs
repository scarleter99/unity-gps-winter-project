using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestAreaGenerator : AreaGenerator
{
    public ForestAreaGenerator(int width, int height, Vector3 originPosition) : base(width, height, originPosition)
    {
        _areaName = Define.AreaName.Forest;   
    }

    public override void GenerateMap()
    {
        GenerateStartTile();
        GenerateBossTile();
        GenerateCommonTiles();
    }

    protected override void GenerateCommonTiles()
    {
        for (int z = 0; z < _height; z++)
        {
            for (int x = 0; x < _width; x++)
            {   
                if (_grid.isGridPositionValid(x, z) && _grid.GetGridObject(x, z) == null)
                    _grid.SetGridObject(x, z, Managers.ResourceMng.Instantiate(_testprefabPath, _tileParent));
            }
        }
    }
}
