using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public sealed class NormalTile : HexGridCell
{
    public NormalTile(int x, int z, float size, GameObject cellObject) : base(x, z, size, cellObject)
    {
        Init();
    }

    public override void Init()
    {
        return;
    }
}
