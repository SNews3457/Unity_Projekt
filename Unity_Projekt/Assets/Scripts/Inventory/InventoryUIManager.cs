using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject inventoryPanel;
    public Button closeButton;

    [Header("Tastenbelegung")]
    public KeyCode openInventoryKey = KeyCode.I;
    public KeyCode closeInventoryKey = KeyCode.Escape;

    private bool isOpen = false;

    void Start()
    {
        inventoryPanel.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseInventory);
    }

    void Update()
    {
        if (Input.GetKeyDown(openInventoryKey))
        {
            ToggleInventory();
        }

        if (isOpen && Input.GetKeyDown(closeInventoryKey))
        {
            CloseInventory();
        }
    }

    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
        Time.timeScale = isOpen ? 0f : 1f;
        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void CloseInventory()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

