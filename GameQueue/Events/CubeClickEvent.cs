using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class CubeClickEvent : GameEvent
{
    public Vector2Int Position { get; private set; }
    public CubeColor Color { get; private set; }
    public CubeObject Cube { get; private set; }
    private static bool isProcessingClick = false; // FIX 1: Static flag to prevent duplicate processing
    
    public CubeClickEvent(CubeObject cube, int priority = 10) : base(priority)
    {
        Cube = cube;
        Position = cube.GridPosition;
        Color = cube.GetCubeColor();
    }
    
    public override void Execute()
    {
        // FIX 2: Prevent duplicate execution
        if (isProcessingClick)
        {
            Debug.LogWarning("Already processing a click event - skipping duplicate");
            return;
        }
        
        isProcessingClick = true;
        
        Debug.Log($"Processing cube click at {Position} with color {Color}");
        
        var gridStorage = Object.FindFirstObjectByType<GridStorage>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (gridStorage == null || gridManager == null)
        {
            Debug.LogError("Missing GridStorage or GridManager");
            isProcessingClick = false;
            return;
        }
        
        // FIX 3: Verify cube still exists before processing
        var currentObj = gridStorage.GetObjectAt(Position);
        if (currentObj == null)
        {
            Debug.LogWarning($"Cube at {Position} no longer exists - likely already processed");
            isProcessingClick = false;
            return;
        }
        
        var gridGroups = new GridGroups(gridStorage, gridManager.GridWidth, gridManager.GridHeight);
        
        // Check if this position has a valid group (2+ connected cubes)
        if (!gridGroups.HasValidGroup(Position))
        {
            Debug.Log("No valid group at clicked position - ignoring click");
            isProcessingClick = false;
            return;
        }
        
        // Get the group for this position
        List<Vector2Int> clickedGroup = gridGroups.GetGroup(Position);
        
        if (!gridGroups.IsValidGroup(clickedGroup))
        {
            Debug.Log("Clicked group is not valid for matching");
            isProcessingClick = false;
            return;
        }
        
        Debug.Log($"Valid group found with {clickedGroup.Count} cubes");
        
        // Debug: Log all positions in the group
        foreach (var pos in clickedGroup)
        {
            Debug.Log($"Group contains cube at: {pos}");
        }
        
        // Check if this should create a rocket (4+ cubes)
        if (gridGroups.IsValidGroupOfFour(clickedGroup))
        {
            Debug.Log("Group eligible for rocket creation!");
            // Handle rocket creation logic here later
            // For now, just process as normal match
        }
        
        // FIX 4: Create events with proper timing
        // Directly destroy ONLY the clicked group
        var destroyEvent = new DestroySpecificGroupEvent(clickedGroup, gridStorage, priority: 9);
        EventQueueManager.Instance.EnqueueEvent(destroyEvent);
        
        // After destruction, drop cubes
        var dropEvent = new DropCubesEvent(gridStorage, gridManager.GridWidth, gridManager.GridHeight, priority: 8);
        EventQueueManager.Instance.EnqueueEvent(dropEvent);
        
        // REMOVED SPAWN EVENT - No new cubes for now
        
        // End turn (this will decrease move count)
        var endTurnEvent = new EndTurnEvent(priority: 6);
        EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
        
        // FIX 5: Reset flag after all events are queued
        isProcessingClick = false;
    }
}