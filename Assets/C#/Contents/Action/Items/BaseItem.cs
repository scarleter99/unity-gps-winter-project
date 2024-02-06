using System;
using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    public ulong Id { get; set; }
    public int DataTemplateId { get; protected set; }
    public Data.ItemData ItemData { get; protected set; }
    public Define.ItemType ItemType { get; protected set; }
    public string Description { get => ItemData.description; }

    private void Start()
    {
        
    }

    protected virtual void Init()
    {
        
    }

    public virtual void SetInfo(int templateId)
    {
        DataTemplateId = templateId;

        ItemData = Managers.DataMng.ItemDataDict[templateId];
        
        gameObject.name = $"{ItemData.dataId}_{ItemData.name}";
    }
    
    public abstract void Use(Creature owner);
}