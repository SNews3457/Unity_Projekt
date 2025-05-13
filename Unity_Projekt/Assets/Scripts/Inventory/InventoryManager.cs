using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.ComponentModel;
public class InventoryManager : MonoBehaviour
{
    public ItemClass itemToAdd;
    public GameObject inventory;
    public bool InventoryisActiv;
    public List<ItemClass> items = new List<ItemClass>();
    public ItemClass itemToRemove;

    private void Start()
    {
        Add(itemToAdd);
    }

    public void Add(ItemClass item)
    {
        items.Add(item);
    }

    public void Remove(ItemClass item)
    {
        items.Remove(item);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
            InventoryisActiv = true;


            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.SetActive(false);
            InventoryisActiv = false;

            
            Time.timeScale = 1f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

        }
    }
}
