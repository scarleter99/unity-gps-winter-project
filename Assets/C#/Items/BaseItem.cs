using UnityEngine;

public abstract class BaseItem : MonoBehaviour
{
    protected string _name;
    protected Define.ItemType _itemType = Define.ItemType.None;
    
    public string Name { get => _name; set => _name = value; }
    public Define.ItemType ItemType { get => _itemType; set => _itemType = value; }
    public string Description { get => Managers.DataMng.ItemDict[_name].description; }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        Name = name;
    }
    
    public abstract void Use(BaseController control);
}