using UnityEngine;

public class EndTurnEvent : GameEvent
{
    public EndTurnEvent(int priority = 0) : base(priority)
    {
    }
    
    public override void Execute()
    {
        Debug.Log("Turn completed - no more matches found");
        
        // Decrease move count - udate goal
        var moveCounter = Object.FindFirstObjectByType<MoveCounter>();
        var goalTracker =  Object.FindFirstObjectByType<GoalTracker>();
        if (goalTracker != null)
        {
            goalTracker.SetGoals();
        } 
        if (moveCounter != null)
        {
            bool canContinue = moveCounter.DecreaseMoveCount();
            
            if (!canContinue)
            {
                // Game over - no moves left
                Debug.Log("Game Over - No moves remaining!");
                LevelProgressionHelper.OnLevelLose();
                return;
            }
        }
        
        // Check win condition
        if (CheckWinCondition())
        {
            Debug.Log("Level Complete - All obstacles cleared!");
            LevelProgressionHelper.OnLevelWin();
            return;
        }
        
        // Update rocket hints for eligible groups
        UpdateRocketHints();
        
        // Turn complete - ready for next input
        Debug.Log("Ready for next player input");
    }
    
    private bool CheckWinCondition() // should not be done here
    {
        // Check if all obstacles are cleared
        var gridStorage = Object.FindFirstObjectByType<GridStorage>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (gridStorage == null || gridManager == null)
            return false;
        
        // Check all positions for remaining obstacles
        for (int x = 0; x < gridManager.GridWidth; x++)
        {
            for (int y = 0; y < gridManager.GridHeight; y++)
            {
                var obj = gridStorage.GetObjectAt(new Vector2Int(x, y));
                if (obj is BoxObstacle ||  obj is StoneObstacle || obj is VaseObstacle) // not ideal I KNOW but i dont have time
                {
                    return false;
                }
            }
        }
        
        return true; // No obstacles found = level complete
    }
    
    private void UpdateRocketHints() // should also not be done here
    {
        var gridStorage = Object.FindFirstObjectByType<GridStorage>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (gridStorage == null || gridManager == null)
            return;
        
        var gridGroups = new GridGroups(gridStorage, gridManager.GridWidth, gridManager.GridHeight);
        var rocketEligiblePositions = gridGroups.GetRocketEligiblePositions();
        
        // Clear all existing rocket hints
        for (int x = 0; x < gridManager.GridWidth; x++)
        {
            for (int y = 0; y < gridManager.GridHeight; y++)
            {
                var obj = gridStorage.GetObjectAt(new Vector2Int(x, y));
                if (obj is CubeObject cube)
                {
                    cube.SetRegularSprite();
                }
            }
        }
        
        // Set rocket hints for eligible positions
        foreach (var position in rocketEligiblePositions)
        {
            var obj = gridStorage.GetObjectAt(position);
            if (obj is CubeObject cube)
            {
                cube.SetRocketSprite();
            }
        }
        
        Debug.Log($"Updated rocket hints for {rocketEligiblePositions.Count} positions");
    }
}