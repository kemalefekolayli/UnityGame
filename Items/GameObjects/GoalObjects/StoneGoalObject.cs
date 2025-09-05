using UnityEngine;

public class StoneGoalObject : GoalObject
{
    public override void SetGoalText()
    {
        _goalText.text =  _goalTracker.GetObstacleCount("s").ToString();
    }
}