using UnityEngine;

public class VaseGoalObject : GoalObject
{
    public override void SetGoalText()
    {
        if (_goalTracker == null)
        {
            Debug.LogError("GoalTracker is null in VaseGoalObject!");
            return;
        }
        
        int count = _goalTracker.GetObstacleCount("v");
        SetText(count.ToString());
    }
}