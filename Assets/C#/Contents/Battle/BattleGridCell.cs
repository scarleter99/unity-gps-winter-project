using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGridCell : MonoBehaviour
{   
    public Define.GridSide GridSide { get; protected set; }
    
    public CreatureController CellCreature { get; set; }
    
    public int Row { get; protected set; }
    public int Col { get; protected set; }

    public void SetRowCol(int row, int col)
    {
        Row = row;
        Col = col;
    }
}
