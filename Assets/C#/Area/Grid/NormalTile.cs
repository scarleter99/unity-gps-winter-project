using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public sealed class NormalTile : HexGridCell
{
    public NormalTile(int x, int z, GameObject cellObject, float size = 1) : base(x, z, cellObject, size)
    {
        Init();
    }

    public override void Init()
    {
        return;
    }
}
