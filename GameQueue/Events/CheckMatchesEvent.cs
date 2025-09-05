using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class CheckMatchesEvent : GameEvent
{
    private GridStorage gridStorage;
    private GridGroups gridGroups;
    private int gridWidth;
    private int gridHeight;
    private bool isCascadeCheck;  // Flag to identify if this is a cascade check
    
    public CheckMatchesEvent(GridStorage storage, int width, int height, bool cascade = false, int priority = 5) : base(priority)
    {
        gridStorage = storage;
        gridWidth = width;
        gridHeight = height;
        gridGroups = new GridGroups(storage, width, height);
        isCascadeCheck = cascade;
    }
    
    public override void Execute()
    {
        if (!isCascadeCheck)
        {
            Debug.LogWarning("CheckMatchesEvent should only be used for cascade checking. Use CubeClickEvent for player input.");
            // End turn immediately if called incorrectly
            var endTurnEvent = new EndTurnEvent(priority: 0);
            EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
            return;
        }
        
        Debug.Log("Checking for cascade matches...");
        
        // Find all valid groups (2+ connected cubes)
        List<List<Vector2Int>> allValidGroups = gridGroups.GetAllGroups();
        
        // For now, we DON'T auto-destroy cascade matches
        // We just identify them and highlight them for the player
        if (allValidGroups.Count > 0)
        {
            Debug.Log($"Found {allValidGroups.Count} valid groups available for next move");
            
            // Don't destroy them! Just end the turn
            // The player needs to click to destroy groups
            var endTurnEvent = new EndTurnEvent(priority: 0);
            EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
        }
        else
        {
            Debug.Log("No matches found - turn complete");
            var endTurnEvent = new EndTurnEvent(priority: 0);
            EventQueueManager.Instance.EnqueueEvent(endTurnEvent);
        }
    }
}