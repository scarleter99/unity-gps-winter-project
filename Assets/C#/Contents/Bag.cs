using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;

public class Bag
{
    private List<Data.BagItem> _items;
    public List<Data.BagItem> Items { get => _items; set => _items = value; }

    public Bag(Transform player)
    {
        Items = new List<Data.BagItem>(6);
        for (int i = 0; i < 6; i++)
            Items.Add(new BagItem());

        GameObject itemCollection = new GameObject { name = "@Items" };
        itemCollection.transform.parent = player;

        // temp - for test
        StoreItem("Items/Item1", 0, 4, itemCollection.transform);
        StoreItem("Items/SampleItem", 1, 5, itemCollection.transform);
    }

    private void DestroyItemIfPossible(int index)
    {
        if (Items[index].IsNull() || Items[index].count > 0)
            return;
        
        Managers.ResourceMng.Destroy(Items[index].item.gameObject);
        Items[index] = new BagItem();
    }
    
    // Item 교체 혹은 새로 주울 때 호출
    public void StoreItem(string path, int index, int count = 1, Transform parent = null)
    {
        DestroyItemIfPossible(index);
        Items[index] = new BagItem(Managers.ResourceMng.Instantiate(path, parent).GetComponent<BaseItem>(), count);
    }
    
    // 아이템 사용
    public void UseItem(int index)
    {
        var selectedIndex = Items[index];
        if (selectedIndex.IsNull())
            return;
        
        //selectedIndex.item.Use(Managers.GameMng.Player.GetComponent<BaseController>()); // TODO - 주석 풀기
        selectedIndex.count--;
        
        // debug
        Debug.Log($"Item Used! Name : {selectedIndex.item.Name}, Count : {selectedIndex.count}");

        DestroyItemIfPossible(index);
    }
}
