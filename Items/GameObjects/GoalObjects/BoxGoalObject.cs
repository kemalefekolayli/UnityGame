using UnityEngine;

public class BoxGoalObject : GoalObject
{
    public override void SetGoalText()
    {
        if (_goalTracker == null)
        {
            Debug.LogError("GoalTracker is null in BoxGoalObject!");
            return;
        }
        
        int count = _goalTracker.GetObstacleCount("bo");
        SetText(count.ToString());
    }
}