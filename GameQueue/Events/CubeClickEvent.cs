using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class CubeClickEvent : GameEvent
{
    public Vector2Int Position { get; private set; }
    public CubeColor Color { get; private set; }
    public CubeObject Cube { get; private set; }
    private static bool isProcessingClick = false; 
    
    public CubeClickEvent(CubeObject cube, int priority = 10) : base(priority)
    {
        Cube = cube;
        Position = cube.GridPosition;
        Color = cube.GetCubeColor();
    }
    
    public override void Execute()
    {
        if (isProcessingClick)
        {
            return;
        }
        
        isProcessingClick = true;
        var gridStorage = Object.FindFirstObjectByType<GridStorage>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (gridStorage == null || gridManager == null)
        {
            isProcessingClick = false;
            return;
        }
        
        // Verify cube still exists before processing
        var currentObj = gridStorage.GetObjectAt(Position);
        if (currentObj == null)
        {
            isProcessingClick = false;
            return;
        }
        
        var gridGroups = new GridGroups(gridStorage, gridManager.GridWidth, gridManager.GridHeight);
        
        // Check if this position has a valid group (2+ connected cubes)
        if (!gridGroups.HasValidGroup(Position))
        {
            isProcessingClick = false;
            return;
        }
        
        List<Vector2Int> clickedGroup = gridGroups.GetGroup(Position);
        
        if (!gridGroups.IsValidGroup(clickedGroup))
        {
            isProcessingClick = false;
            return;
        }
        
        // Check if this should create a rocket (4+ cubes)
        if (gridGroups.IsValidGroupOfFour(clickedGroup))
        {
            // Handle rocket creation logic here later
            // For now, just process as normal match
        }
        
        // Create events with proper timing
        // Directly destroy ONLY the clicked group
        var destroyEvent = new DestroySpecificGroupEvent(clickedGroup, gridStorage, priority: 9);
        EventQueueManager.Instance.EnqueueEvent(destroyEvent);
        
        // After destruction, drop cubes
        var dropEvent = new DropCubesEvent(gridStorage, gridManager.GridWidth, gridManager.GridHeight, priority: 8);
        EventQueueManager.Instance.EnqueueEvent(dropEvent);
        
        // UNCOMMENTED: Spawn new cubes to fill empty spaces at top
        var spawnEvent = new SpawnCubesEvent(gridStorage, gridManager.GridWidth, gridManager.GridHeight, priority: 7);
        EventQueueManager.Instance.EnqueueEvent(spawnEvent);
        
        // End turn (this will decrease move count)
        var endTurnEvent = new EndTurnEvent(priority: 6);
        EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
        
        // Reset flag after all events are queued
        isProcessingClick = false;
    }
}