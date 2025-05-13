using UnityEngine;

[CreateAssetMenu(fileName = "new Material Class", menuName = "Item/Material")]
public class MaterialClass : ItemClass
{
    [Header("Materials")]
    public MaterialType toolType;
    public enum MaterialType
    {
        Fire,
        Electric,
        ice,
        wind,
    }

    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override MaterialClass GetMaterial() { return this; }
}
