using UnityEngine;
using System.Collections.Generic;

public class GridStorage : MonoBehaviour { // should be singleton
        
    public List<string> grid;
    private Dictionary<Vector2Int, AbstractGridObject> GridObjects = new Dictionary<Vector2Int, AbstractGridObject>();
    private Dictionary<Vector2Int, string> GridTypes = new Dictionary<Vector2Int, string>();
    private LevelController _levelController;


    void Start()
    {
        _levelController = FindFirstObjectByType<LevelController>();
        if (_levelController != null && _levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into Grid Storage");
            grid = _levelController.GetLevelData().GetGridVector();
        }
    }

    
}
