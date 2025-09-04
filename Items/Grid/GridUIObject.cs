using UnityEngine;

public class GridUIObject : MonoBehaviour
{
    private LevelController levelController;
    public int GridHeight;
    public int GridWidth;

    public float newWidth;
    public float newHeight;
    
    private float CellSize = 70f;
    private float CellSpacing = 5f;

    [SerializeField] private GameObject gridObject;
    private RectTransform gridRect;
    
    void Start()
    {
        levelController = FindFirstObjectByType<LevelController>();
        if (levelController != null && levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into Grid Object");
            GridHeight = levelController.GetLevelData().GetGridHeight();
            GridWidth = levelController.GetLevelData().GetGridWidth();
            gridRect = gridObject.transform.Find("Grid").GetComponent<RectTransform>();

        }
        InitGridObject();
    }

    public void InitGridObject()
    {
        if (gridRect == null) return;

        newWidth = GridWidth * CellSize + (GridWidth - 1) * CellSpacing;
        newHeight = GridHeight * CellSize + (GridHeight - 1) * CellSpacing;

        gridRect.sizeDelta = new Vector2(newWidth, newHeight);
        SetPosition(0,-100);
        
        Debug.Log($"Grid resized: {newWidth} x {newHeight}");
    }
    
    public void SetPosition(float x, float y) // position the grid in a place of ur own choosing
    {
        if (gridRect != null)
        {
            Vector2 newPos = new Vector2(x, y);
            gridRect.anchoredPosition = newPos;
        }
        else
        {
            Debug.LogWarning("Target RectTransform is not assigned!");
        }
    }


    
}