using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class CubeClickEvent : GameEvent
{
    public Vector2Int Position { get; private set; }
    public CubeColor Color { get; private set; }
    public CubeObject Cube { get; private set; }
    
    public CubeClickEvent(CubeObject cube, int priority = 10) : base(priority)
    {
        Cube = cube;
        Position = cube.GridPosition;
        Color = cube.GetCubeColor();
    }
    
    public override void Execute()
    {
        Debug.Log($"Processing cube click at {Position} with color {Color}");
        
        var gridStorage = Object.FindFirstObjectByType<GridStorage>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (gridStorage == null || gridManager == null)
        {
            Debug.LogError("Missing GridStorage or GridManager");
            return;
        }
        
        var gridGroups = new GridGroups(gridStorage, gridManager.GridWidth, gridManager.GridHeight);
        
        // Check if this position has a valid group (2+ connected cubes)
        if (!gridGroups.HasValidGroup(Position))
        {
            Debug.Log("No valid group at clicked position - ignoring click");
            return;
        }
        
        // Get the group for this position
        List<Vector2Int> clickedGroup = gridGroups.GetGroup(Position);
        
        if (!gridGroups.IsValidGroup(clickedGroup))
        {
            Debug.Log("Clicked group is not valid for matching");
            return;
        }
        
        Debug.Log($"Valid group found with {clickedGroup.Count} cubes");
        
        // Check if this should create a rocket (4+ cubes)
        if (gridGroups.IsValidGroupOfFour(clickedGroup))
        {
            Debug.Log("Group eligible for rocket creation!");
            // Handle rocket creation logic here later
            // For now, just process as normal match
        }
        
        // Directly destroy ONLY the clicked group
        var destroyEvent = new DestroySpecificGroupEvent(clickedGroup, gridStorage, priority: 9);
        EventQueueManager.Instance.EnqueueEvent(destroyEvent);
        
        // After destruction, drop cubes
        var dropEvent = new DropCubesEvent(gridStorage, gridManager.GridWidth, gridManager.GridHeight, priority: 8);
        EventQueueManager.Instance.EnqueueEvent(dropEvent);
        
        // After dropping, spawn new cubes
        var spawnEvent = new SpawnCubesEvent(gridStorage, gridManager.GridWidth, gridManager.GridHeight, priority: 7);
        EventQueueManager.Instance.EnqueueEvent(spawnEvent);
        
        // End turn - NO automatic cascade checking
        var endTurnEvent = new EndTurnEvent(priority: 6);
        EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
    }
}