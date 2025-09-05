using UnityEngine;

public class GridManager : MonoBehaviour 
{
    [Header("Settings")]
    [SerializeField] private GridSettings gridSettings;
    
    private LevelController levelController;
    private GridStorage gridStorage;

    [Header("Grid Info")]
    public int GridHeight;
    public int GridWidth;
    private Vector2 gridOrigin; // Bottom-left corner in world space
    private GoalTrackView goalTrackView;
    
    [Header("Prefabs")]
    [SerializeField] GameObject cubePrefab;
    [SerializeField] Transform gridParent; // Parent object for all cubes
    [SerializeField] CubeFactory cubeFactory;
    
    
    void Start()
    {
        
        gridStorage = GetComponent<GridStorage>();
        levelController = FindFirstObjectByType<LevelController>();
        
        if (levelController != null && levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into GridManager");
            GridHeight = levelController.GetLevelData().GetGridHeight();
            GridWidth = levelController.GetLevelData().GetGridWidth();
            InitializeGrid(); 
        }
    }
    
    void InitializeGrid()
    {
        
        CalculateGridStartPosition();
        
        for(int i = 0; i < gridStorage.grid.Count; i++)
        {
            Vector2Int gridPos = IndexToGridPosition(i);
            Vector2 worldPos = GridToWorldPosition(gridPos);
            string cellType = gridStorage.grid[i];
            
            CreateCube(worldPos, gridPos, cellType); // bazılarını basamıyo henüz
        }
        goalTrackView = FindFirstObjectByType<GoalTrackView>();
        if (goalTrackView != null)
        {
            goalTrackView.InitializeGoals();
        }
    }
    
    Vector2Int IndexToGridPosition(int index)
    {
        int x = index % GridWidth;
        int y = index / GridWidth;
        return new Vector2Int(x, y);
    }
    
    public Vector2 GridToWorldPosition(Vector2Int gridPos)
    {
        return gridOrigin + new Vector2(
            gridPos.x * gridSettings.CellSpacing, 
            gridPos.y * gridSettings.CellSpacing
        );
    }
    
    void CreateCube(Vector2 worldPos, Vector2Int gridPos, string color) // bu eleman factory çağırmalı
    {
        cubeFactory.CreateCube(color,worldPos, gridParent, gridPos);
    }
    
    public void CalculateGridStartPosition()
    {
        float totalGridWidth = GridWidth * gridSettings.CellSpacing;
        float totalGridHeight = GridHeight * gridSettings.CellSpacing;
        
        gridOrigin = new Vector2(
            gridSettings.GridOriginOffset.x - (totalGridWidth / 200) + gridSettings.CellSpacing / 200,
            gridSettings.GridOriginOffset.y - (totalGridHeight / 200) + gridSettings.CellSpacing / 100
        );
    }
    
    
    // helper for debugging
    [ContextMenu("Debug Grid Info")]
    void DebugGridInfo()
    {
        Debug.Log($"Grid: {GridWidth}x{GridHeight}");
        Debug.Log($"Cube Scale: {gridSettings.CubeScale}");
        Debug.Log($"Cell Spacing: {gridSettings.CellSpacing}");
        Debug.Log($"Gap between cubes: {gridSettings.ActualCubeGap}");
        Debug.Log($"Grid Origin: {gridOrigin}");
    }
}