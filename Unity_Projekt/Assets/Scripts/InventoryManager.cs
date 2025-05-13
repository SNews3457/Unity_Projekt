using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventar-Einstellungen")]
    public int rows = 4;
    public int columns = 5;
    public GameObject slotPrefab;
    public Transform panelParent;
    public GameObject inventory;
    public bool InventoryisActiv;

    void Start()
    {
        CreateInventoryGrid();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventory.SetActive(true);
            InventoryisActiv = true;
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            inventory.SetActive(false);
            InventoryisActiv = false;
        }
    }

    void CreateInventoryGrid()
    {
        // GridLayoutGroup vorbereiten
        GridLayoutGroup grid = panelParent.GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = panelParent.gameObject.AddComponent<GridLayoutGroup>();
        }

        // Zellen berechnen
        RectTransform rt = panelParent.GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;
        float totalSpacingX = grid.spacing.x * (columns - 1);
        float totalSpacingY = grid.spacing.y * (rows - 1);

        float cellWidth = (width - totalSpacingX) / columns;
        float cellHeight = (height - totalSpacingY) / rows;
        grid.cellSize = new Vector2(cellWidth, cellHeight);


        grid.cellSize = new Vector2(cellWidth, cellHeight);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = new Vector2(0f, 0f);
        grid.padding = new RectOffset(0, 0, 0, 0);
        grid.childAlignment = TextAnchor.UpperLeft;


        // Alte Slots löschen
        foreach (Transform child in panelParent)
        {
            Destroy(child.gameObject);
        }

        // Neue Slots erstellen
        for (int i = 0; i < rows * columns; i++)
        {
            Instantiate(slotPrefab, panelParent);
        }
    }
}
