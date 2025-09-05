using UnityEngine;

public class StoneGoalObject : GoalObject
{
    public override void SetGoalText()
    {
        if (_goalTracker == null)
        {
            Debug.LogError("GoalTracker is null in StoneGoalObject!");
            return;
        }
        
        int count = _goalTracker.GetObstacleCount("s");
        SetText(count.ToString());
    }
}