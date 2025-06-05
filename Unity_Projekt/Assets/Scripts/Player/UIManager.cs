using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class UIManager : MonoBehaviour
{
    public LevelUpManager levelUpManager;
    public MinimapToggle map;
    public InventoryManager inventoryManager;

    private enum MenuType { Minimap, SkillTree, Inventory }
    private MenuType currentMenu = MenuType.SkillTree;
    private bool isMenuOpen = false;



    private List<MenuType> menuOrder = new List<MenuType>
    {
        MenuType.SkillTree,
        MenuType.Inventory,
        MenuType.Minimap
    };

    public void OpenorClose()
    {
        if (isMenuOpen)
        {
            CloseAllMenus();
        }
        else if (!isMenuOpen)
        {
            OpenMenu(currentMenu);
        }
    }
    void Update()
    {
        Debug.Log("Aktuelles Menü: " + currentMenu);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuOpen)
                CloseAllMenus();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isMenuOpen)
            {
                OpenMenu(currentMenu);
            }
        }

        if (isMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchMenu(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                SwitchMenu(-1);
            }
        }
    }

    void OpenMenu(MenuType menu)
    {
        isMenuOpen = true;

        switch (menu)
        {
            case MenuType.Minimap:
                map.OpenMinimap();
                levelUpManager.CloseSkillTree();
                inventoryManager.CloseInventory();
                break;

            case MenuType.SkillTree:
                levelUpManager.GoToSkillTree();
                map.CloseMinimap();
                inventoryManager.CloseInventory(); 
                break;

            case MenuType.Inventory:
                inventoryManager.OpenInventory();
                map.CloseMinimap();
                levelUpManager.CloseSkillTree();
                break;
        }
    }

    void CloseAllMenus()
    {
        map.CloseMinimap();
        levelUpManager.CloseSkillTree();
        inventoryManager.CloseInventory();
        isMenuOpen = false;
    }

    public void SwitchMenu(int direction)
    {
        int currentIndex = menuOrder.IndexOf(currentMenu);
        currentIndex = (currentIndex + direction + menuOrder.Count) % menuOrder.Count;
        currentMenu = menuOrder[currentIndex];
        OpenMenu(currentMenu);
    }
}
