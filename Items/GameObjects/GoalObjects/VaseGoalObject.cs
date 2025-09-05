using UnityEngine;

public class VaseGoalObject : GoalObject {
    
    public override void SetGoalText()
    {
        _goalText.text =  _goalTracker.GetObstacleCount("v").ToString();
    }
}
