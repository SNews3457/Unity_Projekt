using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable;
    public abstract ItemClass GetItem();
    public abstract ToolClass GetTool();
    public abstract MaterialClass GetMaterial();
}
