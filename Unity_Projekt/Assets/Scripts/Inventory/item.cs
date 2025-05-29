using UnityEngine;

public class item : MonoBehaviour
{
    public ItemClass thisitem;
    InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = FindAnyObjectByType<InventoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            inventoryManager.Add(thisitem, 1);
            Destroy(this.gameObject);
        }
    }
}
