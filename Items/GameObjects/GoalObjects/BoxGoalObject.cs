
public class BoxGoalObject : GoalObject
{
    public override void SetGoalText()
    {
        _goalText.text =  _goalTracker.GetObstacleCount("bo").ToString();
    }
}