using UnityEngine;

public class GridObject : MonoBehaviour
{
    private LevelController levelController;
    public int GridHeight;
    public int GridWidth;
    
    private float CellSize = 50f;
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

        float newWidth = GridWidth * CellSize + (GridWidth - 1) * CellSpacing;
        float newHeight = GridHeight * CellSize + (GridHeight - 1) * CellSpacing;

        gridRect.sizeDelta = new Vector2(newWidth, newHeight);

        Debug.Log($"Grid resized: {newWidth} x {newHeight}");
    }
}