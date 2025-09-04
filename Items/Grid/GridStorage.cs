using UnityEngine;
using System.Collections.Generic;

public class GridStorage : MonoBehaviour 
{
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
    
    public void SetObjectAt(Vector2Int position, AbstractGridObject obj)
    {
        GridObjects[position] = obj;
        if(obj is CubeObject cube)
        {
            GridTypes[position] = cube.GetCubeColor().ToString();
        }
    }
    
    public AbstractGridObject GetObjectAt(Vector2Int position) // also should not be here
    {
        return GridObjects.ContainsKey(position) ? GridObjects[position] : null;
    }
    
    public void RemoveObjectAt(Vector2Int position) // should not be here
    {
        if(GridObjects.ContainsKey(position))
        {
            GridObjects.Remove(position);
            GridTypes.Remove(position);
        }
    }

// In GridStorage.cs
    public CubeColor GetColorAt(Vector2Int position)
    {
        var obj = GetObjectAt(position);
        if (obj is CubeObject cube)
        {
            return cube.GetCubeColor();
        }
        return (CubeColor)99;
    }
}