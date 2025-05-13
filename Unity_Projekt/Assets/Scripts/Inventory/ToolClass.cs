using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        Range,
        Melee, 
        Magic
    }
    public  override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return this; }
    public  override MaterialClass GetMaterial() { return null; }
}
