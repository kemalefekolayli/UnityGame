using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;


public class CheckMatchesEvent : GameEvent
{
    private GridStorage gridStorage;
    private GridGroups gridGroups;
    private int gridWidth;
    private int gridHeight;
    
    public CheckMatchesEvent(GridStorage storage, int width, int height, int priority = 5) : base(priority)
    {
        gridStorage = storage;
        gridWidth = width;
        gridHeight = height;
        gridGroups = new GridGroups(storage, width, height);
    }
    
    public override void Execute()
    {
        Debug.Log("Checking for matches...");
        
        // Find all valid groups (2+ connected cubes)
        List<List<Vector2Int>> allValidGroups = gridGroups.GetAllGroups();
        
        if (allValidGroups.Count > 0)
        {
            Debug.Log($"Found {allValidGroups.Count} valid groups for matching");
            
            // Enqueue destroy event for all matched groups
            var destroyEvent = new DestroyCubesEvent(allValidGroups, gridStorage, priority: 4);
            EventQueueManager.Instance.EnqueueEvent(destroyEvent);
            
            // After destruction, we'll need to drop cubes
            var dropEvent = new DropCubesEvent(gridStorage, gridWidth, gridHeight, priority: 3);
            EventQueueManager.Instance.EnqueueEvent(dropEvent);
            
            // After dropping, spawn new cubes
            var spawnEvent = new SpawnCubesEvent(gridStorage, gridWidth, gridHeight, priority: 2);
            EventQueueManager.Instance.EnqueueEvent(spawnEvent);
            
            // After spawning, check for cascade matches
            var cascadeCheckEvent = new CheckMatchesEvent(gridStorage, gridWidth, gridHeight, priority: 1);
            EventQueueManager.Instance.EnqueueEvent(cascadeCheckEvent);
        }
        else
        {
            Debug.Log("No matches found - turn complete");
            // Enqueue end turn event
            var endTurnEvent = new EndTurnEvent(priority: 0);
            EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
        }
    }
}