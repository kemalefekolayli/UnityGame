using System.Collections.Generic;
using UnityEngine;

public class GoalTracker : MonoBehaviour
{

    private GridStorage _gridStorage;
    private LevelController _levelController;
    
    public Dictionary<string, int> ObstacleCounts { get; private set; }

    public void SetGoals()
    {
        _levelController = FindFirstObjectByType<LevelController>();
        List<string> levelGrid = _levelController.GetLevelData().GetGridVector();
        ObstacleCounts = new Dictionary<string, int>();
        for (int i = 0; i < levelGrid.Count; i++)
        {
            if (levelGrid[i].Equals("bo"))
            {
               
                if (!ObstacleCounts.ContainsKey("bo"))
                    ObstacleCounts["bo"] = 0;

                ObstacleCounts["bo"]++;
            }
        }
        Debug.LogError(GetObstacleCount("bo"));
    }
    
    
    public int GetObstacleCount(string code)
    {
        return ObstacleCounts != null && ObstacleCounts.TryGetValue(code, out int count) 
            ? count 
            : 0;
    }
    
    public void DecreaseObstacle(string code, int amount = 1) // when an obstacle is destroyed we will be calling an obstacle destroy method and will call this
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