using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    public GameObject inventory;
    public bool InventoryisActiv;


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
