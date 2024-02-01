using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchasable
{
    public int SellingPrice { get; }
    public int PurchasePrice { get; }
}
