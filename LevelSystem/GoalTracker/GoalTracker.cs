using System.Collections.Generic;
using UnityEngine;

public class GoalTracker : MonoBehaviour
{

    [SerializeField] GridStorage _gridStorage;
    
    public Dictionary<string, int> ObstacleCounts { get; private set; }

    void Start()
    {
        _gridStorage = GetComponent<GridStorage>();
    }

    public void SetGoals()
    {

        var gridTypes = _gridStorage.ReturnGridTypes();
        ObstacleCounts = new Dictionary<string, int>();

        foreach (var kvp in gridTypes)
        {
            string cellType = kvp.Value;

            if (string.IsNullOrEmpty(cellType))
                continue;

            if (!ObstacleCounts.ContainsKey(cellType))
                ObstacleCounts[cellType] = 0;

            ObstacleCounts[cellType]++;
        }
    }
    
    
    public int GetObstacleCount(string code)
    {
        return ObstacleCounts != null && ObstacleCounts.TryGetValue(code, out int count) 
            ? count 
            : 0;
    }
    
    public void DecreaseObstacle(string code, int amount = 1)
    {
        if (ObstacleCounts == null) return;

        if (ObstacleCounts.ContainsKey(code))
        {
            ObstacleCounts[code] -= amount;

            if (ObstacleCounts[code] <= 0)
            {
                ObstacleCounts[code] = 0;
            }
        }
        else
        {
            Debug.LogWarning($"Tried to decrease unknown obstacle '{code}'");
        }
    }

    
    //* example usage
    //int boxCount = goalTracker.GetObstacleCount("bo");
    //int vaseCount = goalTracker.GetObstacleCount("v");
    //int stoneCount = goalTracker.GetObstacleCount("s");
    //
    //*//
}