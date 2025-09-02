using UnityEngine;

public class GridManager : MonoBehaviour 
{
    private LevelController levelController;
    private GridStorage gridStorage;
    
    [Header("Grid Settings")]
    public int GridHeight;
    public int GridWidth;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 gridOrigin = new Vector2(-3f, -4f); // Bottom-left corner in world space
    
    [Header("Prefabs")]
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private Transform gridParent; // Parent object for all cubes
    
    [Header("Sprites")]
    [SerializeField] private Sprite blueSprite;
    // Later add: redSprite, greenSprite, yellowSprite
    
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
        var gridData = gridStorage.grid;
        
        for(int i = 0; i < gridData.Count; i++)
        {
            Vector2Int gridPos = IndexToGridPosition(i);
            Vector2 worldPos = GridToWorldPosition(gridPos);
            
            string cellType = gridData[i];
            if(cellType == "b") // For now, only blue
            {
                CreateCube(worldPos, gridPos, "b");
            }
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
    
    void CreateCube(Vector2 worldPos, Vector2Int gridPos, string color)
    {
        GameObject cube = Instantiate(cubePrefab, worldPos, Quaternion.identity, gridParent);
        cube.transform.localScale = Vector3.one * (cellSize * 0.8f); // Slightly smaller than cell // no clue what this does btw
        
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos, color, blueSprite); // just blue for now
        cubeObj.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y; // Set sorting order based on row (higher rows render on top)
        
        // Store in GridStorage
        gridStorage.SetObjectAt(gridPos, cubeObj);
    }
}