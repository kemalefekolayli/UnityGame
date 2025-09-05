
using UnityEngine;

public class BoxGoalObject : GoalObject
{
    public override void SetGoalText()
    {
        if (_goalText == null)
        {
            Debug.Log("No goal text set - 1234");
        }
        _goalText.text =  _goalTracker.GetObstacleCount("bo").ToString();
    }
}