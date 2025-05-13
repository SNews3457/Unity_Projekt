using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.ComponentModel;
using UnityEngine.UI;
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    public GameObject inventory;
    public bool InventoryisActiv;
    [SerializeField] private List<ItemClass> items = new List<ItemClass>();
    [SerializeField] private ItemClass itemToRemove;

    private GameObject[] slots;

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        //Dagobert Slots zuweisen
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
        Add(itemToAdd);
    }

    public void RefreshUI()
    {
        for (int i = 0;i < slots.Length;i++) 
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemIcon;
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }

    public void Add(ItemClass item)
    {
        items.Add(item);
        RefreshUI();
    }

    public void Remove(ItemClass item)
    {
        items.Remove(item);
        RefreshUI();
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
