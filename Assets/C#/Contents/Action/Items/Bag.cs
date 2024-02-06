using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bag
{ 
    public List<Data.BagItem> Items { get; protected set; }
    public int Gold { get; protected set; }

    public Bag(Transform player)
    {
     Items = new List<Data.BagItem>(6);
     for (int i = 0; i < 6; i++)
         Items.Add(new Data.BagItem());

     GameObject itemCollection = new GameObject { name = "ItemBag" };
     itemCollection.transform.parent = player;

     // TODO - TEST CODE
     StoreItem("Items/Item1", 0, 4, itemCollection.transform);
     StoreItem("Items/SampleItem", 1, 5, itemCollection.transform);
    }

    private void DestroyItemIfPossible(int index)
    {
        if (Items[index].IsNull() || Items[index].count > 0)
            return;
        
        Managers.ResourceMng.Destroy(Items[index].item.gameObject);
        Items[index] = new Data.BagItem();
    }
    
    // ItemData 교체 혹은 새로 주울 때 호출
    public void StoreItem(string path, int index, int count = 1, Transform parent = null)
    {
        DestroyItemIfPossible(index);
        Items[index] = new Data.BagItem(Managers.ResourceMng.Instantiate(path, parent).GetComponent<BaseItem>(), count);
    }
    
    // 아이템 사용
    public void UseItem(Creature user, int index)
    {
        var selectedIndex = Items[index];
        if (selectedIndex.IsNull())
            return;
        
        selectedIndex.item.Use(user);
        selectedIndex.count--;
        
        // debug
        Debug.Log($"ItemData Used! Name : {selectedIndex.item.ItemData.name}, Count : {selectedIndex.count}");

        DestroyItemIfPossible(index);
    }
}
