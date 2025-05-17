using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.ComponentModel;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject movingItemCursor;

    [SerializeField] private GameObject slotHolder;
    [SerializeField] private ItemClass itemToAdd;
    public GameObject inventory;
    public bool InventoryisActiv;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private SlotClass[] startingItems;

    private SlotClass[] items;

    private GameObject[] slots;
    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;
    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }
        //Dagobert Slots zuweisen
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
        Add(itemToAdd,1);
        Remove(itemToRemove);
    }

    #region Inventory Utils
    public void RefreshUI()
    {
        for (int i = 0;i < slots.Length;i++) 
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if(items[i].GetItem().isStackable)
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = items[i].GetQuantity() + "";
                else
                    slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<Text>().text = "";
            }
        }
    }

    public bool Add(ItemClass item, int quantity)
    {
        //items.Add(item);
        //ist im Inventar das Item bereits


        SlotClass slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
            slot.AddQuantity(1);
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddIteem(item, quantity);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }
    
    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if(temp.GetQuantity() > 1) 
                temp.SubQuantity(1);
            else
            {

                int slotToRemoveIndex = 0;
                //items.Remove(item);

                for(int i = 0; i < items.Length;i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }    


        RefreshUI();
        return true;
    }
    
    public SlotClass Contains(ItemClass item)
    {
        for(int i = 0; i < items.Length;  i++)
        {
            if (items[i].GetItem() == item)
                return items[i];
        }
        return null;
    }

    private void Update()
    {
        movingItemCursor.SetActive(isMovingItem);
        movingItemCursor.transform.position = Input.mousePosition;
        if(isMovingItem)
            movingItemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;

        if(Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
            {
                EndItemMove();
            }
            else
                BeginItemMove();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
            InventoryisActiv = true;


            Time.timeScale = 1f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.SetActive(false);
            InventoryisActiv = false;

            
            Time.timeScale = 1f;


        }
    }
    #endregion Inventory Utils

    #region Moving

    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        
        if (originalSlot == null || originalSlot.GetItem() ==null)
            return false;
        Debug.Log(originalSlot.GetItem());
        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;    
        RefreshUI();
        return true;
    }
    private SlotClass GetClosestSlot()
    {
        
        for (int i = 0; i < slots.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 50)
                return items[i];
            
        }
        return null;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {

            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {

            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem().GetItem() == movingSlot.GetItem()) //gleiche Items
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                        return false;
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddIteem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddIteem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    RefreshUI();
                    return true;
                }
            }
            else
            {
                originalSlot.AddIteem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;
    }
    #endregion
}
