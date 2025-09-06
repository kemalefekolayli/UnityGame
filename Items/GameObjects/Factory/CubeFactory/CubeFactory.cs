using UnityEngine;

public class CubeFactory : MonoBehaviour , ObjectFactory<AbstractGridObject> {
    
    [Header("Settings")]
    [SerializeField] private GridSettings gridSettings;
    
    [Header("Prefabs and Sprites")]
    public GameObject cubePrefab;
    public GameObject boxObstaclePrefab;
    public GameObject vaseObstaclePrefab;
    
    [Header("Obstacle Sprites")]
    [SerializeField] Sprite BoxObstacleSprite;
    [SerializeField] Sprite VaseObstacleSprite;
    [SerializeField] Sprite StoneObstacleSprite;
    
    [Header("Regular Sprites")]
    [SerializeField] GridStorage gridStorage;
    [SerializeField] Sprite BlueCubeSprite;
    [SerializeField] Sprite RedCubeSprite;
    [SerializeField] Sprite GreenCubeSprite;
    [SerializeField] Sprite YellowCubeSprite;
    
    [Header("Rocket Hint Sprites")]
    [SerializeField] Sprite rocketYellowCubeSprite;
    [SerializeField] Sprite rocketBlueCubeSprite;
    [SerializeField] Sprite rocketGreenCubeSprite;
    [SerializeField] Sprite rocketRedCubeSprite;

    public AbstractGridObject CreateCube(string color,Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        switch (color)
        {
            case "b" : return CreateBlueCube(worldPos, gridParent, gridPos);
            case "r" : return CreateRedCube(worldPos, gridParent, gridPos);
            case "g" : return CreateGreenCube(worldPos, gridParent, gridPos);
            case "y"  : return CreateYellowCube(worldPos, gridParent, gridPos);
            case "rand": return (CreateCube(GetRandomColor(), worldPos, gridParent, gridPos));
            case "bo" : return CreateBoxCube(worldPos, gridParent, gridPos ); // for now might wanna change this logic later
            case "v" : return CreateVaseCube(worldPos, gridParent, gridPos);
            default: return null;
        }
    }
    
    private AbstractGridObject CreateVaseCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        GameObject vase = Instantiate(vaseObstaclePrefab, worldPos, Quaternion.identity, gridParent);
        vase.transform.localScale = Vector3.one * gridSettings.CubeScale;
        
        VaseObstacle vaseObstacle = vase.GetComponent<VaseObstacle>();
        vaseObstacle.Initialize(gridPos, VaseObstacleSprite);
        vaseObstacle.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, vaseObstacle);
        
        return vaseObstacle;
    }

    private AbstractGridObject CreateBoxCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        GameObject box = Instantiate(boxObstaclePrefab, worldPos, Quaternion.identity, gridParent);
        // Use centralized settings
        box.transform.localScale = Vector3.one * gridSettings.CubeScale;
        
        BoxObstacle boxObstacle = box.GetComponent<BoxObstacle>();
        boxObstacle.Initialize(gridPos, BoxObstacleSprite);
        boxObstacle.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, boxObstacle);
        
        return boxObstacle;
    }
    private AbstractGridObject CreateColoredCube(string color, Sprite sprite, Vector3 worldPos, Transform gridParent, Vector2Int gridPos, Sprite rocketHintSprite)
    {
        GameObject cube = Instantiate(cubePrefab, worldPos, Quaternion.identity, gridParent);
        
        // Use centralized settings
        cube.transform.localScale = Vector3.one * gridSettings.CubeScale;
        
        CubeObject cubeObj = cube.GetComponent<CubeObject>();
        cubeObj.Initialize(gridPos, color, sprite, rocketHintSprite );
        cubeObj.GetComponent<SpriteRenderer>().sortingOrder = gridPos.y + 1;
        gridStorage.SetObjectAt(gridPos, cubeObj);
        
        return cubeObj;
    }
    
    private AbstractGridObject CreateBlueCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        return CreateColoredCube("b", BlueCubeSprite, worldPos, gridParent, gridPos, rocketBlueCubeSprite);
    }
    
    private AbstractGridObject CreateRedCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        return CreateColoredCube("r", RedCubeSprite, worldPos, gridParent, gridPos, rocketRedCubeSprite);
    }
    
    private AbstractGridObject CreateYellowCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        return CreateColoredCube("y", YellowCubeSprite, worldPos, gridParent, gridPos, rocketYellowCubeSprite);
    }
    
    private AbstractGridObject CreateGreenCube(Vector3 worldPos, Transform gridParent, Vector2Int gridPos)
    {
        return CreateColoredCube("g", GreenCubeSprite, worldPos, gridParent, gridPos, rocketGreenCubeSprite);
    }
    
    public string GetRandomColor() 
    {
        string[] options = { "b", "r", "g", "y" };
        int index = UnityEngine.Random.Range(0, options.Length);
        return options[index];
    }

}