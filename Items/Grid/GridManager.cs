using UnityEngine;

public class GridManager : MonoBehaviour 
{
    private LevelController levelController;
    private GridStorage gridStorage;


    [Header("Grid Settings")]
    public int GridHeight;
    public int GridWidth;
    [SerializeField] private float cellSize = 0.5f;
    [SerializeField] private Vector2 gridOrigin = new Vector2(0f,0f); // Bottom-left corner in world space
    
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
            InitializeGrid(); // Actually call it!
        }
    }
    
    void InitializeGrid()
    {
        
        CalculateGridStartPosition(50f);
        
        for(int i = 0; i < gridStorage.grid.Count; i++)
        {
            Vector2Int gridPos = IndexToGridPosition(i);
            Vector2 worldPos = GridToWorldPosition(gridPos);
            
            string cellType = gridStorage.grid[i];
            
            CreateCube(worldPos, gridPos, cellType); // bazılarını basamıyo henüz
            
        }
    }
    
    Vector2Int IndexToGridPosition(int index)
    {
        // Grid data starts from bottom-left, goes right then up
        int x = index % GridWidth;
        int y = index / GridWidth;
        return new Vector2Int(x, y);
    }
    
    Vector2 GridToWorldPosition(Vector2Int gridPos)
    {
        return gridOrigin + new Vector2(gridPos.x * cellSize, gridPos.y * cellSize);
    }
    
    void CreateCube(Vector2 worldPos, Vector2Int gridPos, string color) // bu eleman factory çağırmalı
    {
        cubeFactory.CreateCube(color,worldPos, gridParent, gridPos);
    }
    
    
    public void CalculateGridStartPosition(float cellSize)
    {
        float totalGridWidth = GridWidth * cellSize;
        float totalGridHeight = GridHeight * cellSize;
        Vector2 screenCenter = new Vector2(0, -1);

        gridOrigin = new Vector2(
            screenCenter.x - (totalGridWidth / 200) + cellSize / 200,
            screenCenter.y - (totalGridHeight / 200) + cellSize / 100
        );
    }
}