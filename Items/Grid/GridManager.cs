using UnityEngine;

public class GridManager : MonoBehaviour { // should be singleton
        
    private LevelController levelController;
    public int GridHeight;
    public int GridWidth;
    private GridStorage _gridStorage;
    
    void Start()
    {
        levelController = FindFirstObjectByType<LevelController>();
        if (levelController != null && levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into Grid Object");
            GridHeight = levelController.GetLevelData().GetGridHeight();
            GridWidth = levelController.GetLevelData().GetGridWidth();
            
        }
    }
    
}
