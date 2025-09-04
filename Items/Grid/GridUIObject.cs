using UnityEngine;

public class GridUIObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GridSettings gridSettings;
    
    private LevelController levelController;
    public int GridHeight;
    public int GridWidth;

    public float newWidth;
    public float newHeight;

    [SerializeField] private GameObject gridObject;
    private RectTransform gridRect;
    
    void Start()
    {
        levelController = FindFirstObjectByType<LevelController>();
        if (levelController != null && levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into Grid UI Object");
            GridHeight = levelController.GetLevelData().GetGridHeight();
            GridWidth = levelController.GetLevelData().GetGridWidth();
            gridRect = gridObject.transform.Find("Grid").GetComponent<RectTransform>();
        }
        InitGridObject();
    }

    public void InitGridObject()
    {
        if (gridRect == null) return;

        // Use centralized settings for UI calculations
        newWidth = GridWidth * gridSettings.UICellSize + (GridWidth - 1) * gridSettings.UICellSpacing;
        newHeight = GridHeight * gridSettings.UICellSize + (GridHeight - 1) * gridSettings.UICellSpacing;

        gridRect.sizeDelta = new Vector2(newWidth, newHeight);
        SetPosition(0, -100);
        
        Debug.Log($"Grid UI resized: {newWidth} x {newHeight}");
    }
    
    public void SetPosition(float x, float y)
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
    
    // Helper method to preview UI settings
    [ContextMenu("Debug UI Info")]
    void DebugUIInfo()
    {
        Debug.Log($"UI Grid: {GridWidth}x{GridHeight}");
        Debug.Log($"UI Cell Size: {gridSettings.UICellSize}");
        Debug.Log($"UI Cell Spacing: {gridSettings.UICellSpacing}");
        Debug.Log($"Total UI Size: {newWidth} x {newHeight}");
    }
}