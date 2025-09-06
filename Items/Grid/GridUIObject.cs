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
            gridRect = gridObject.GetComponent<RectTransform>();
        }

        InitGridObject();
    }

    public void InitGridObject()
    {
        if (gridRect == null) return;

        // Use centralized settings for UI calculations
        newWidth = GridWidth * 55f + (GridWidth - 1) * 20f ;
        newHeight = GridHeight * 45f + (GridHeight - 1) * 17f ;
        if (GridHeight <= 8)
        {
            newHeight = newHeight + 8f; // this is a horrible practice but im just a boy
        }

        if (GridWidth <= 8)
        {
            newWidth = newWidth + 8f;
        }
        gridRect.sizeDelta = new Vector2(newWidth, newHeight);
        SetPosition(0, -1);
        
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
    
    public RectTransform GridRect => gridRect;
}