using UnityEngine;

public class GridManager : MonoBehaviour { // should be singleton
        
    private LevelController levelController;
    private GridStorage gridStorage;
    public int GridHeight;
    public int GridWidth;
    private GridStorage _gridStorage;
    
    void Start()
    {
        gridStorage = GetComponent<GridStorage>();
        levelController = FindFirstObjectByType<LevelController>();
        if (levelController != null && levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into Grid Object");
            GridHeight = levelController.GetLevelData().GetGridHeight();
            GridWidth = levelController.GetLevelData().GetGridWidth();
            
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
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos);
        cubeObj.SetColor(color);
        // Set sprite based on color...
        
        // Store in GridStorage
        gridStorage.SetObjectAt(gridPos, cubeObj);
    }

    
}
