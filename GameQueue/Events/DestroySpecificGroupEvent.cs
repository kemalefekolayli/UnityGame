using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class DestroySpecificGroupEvent : GameEvent
{
    private List<Vector2Int> groupToDestroy;
    private GridStorage gridStorage;
    
    public DestroySpecificGroupEvent(List<Vector2Int> group, GridStorage storage, int priority = 9) : base(priority)
    {
        groupToDestroy = group;
        gridStorage = storage;
    }
    
    public override void Execute()
    {
        Debug.Log($"Destroying specific group of {groupToDestroy.Count} cubes");
        
        // Track animation count for proper sequencing
        EventQueueManager.Instance.RegisterAnimationStart();
        
        int animationsCompleted = 0;
        int totalCubes = groupToDestroy.Count;
        
        // Destroy only the specific group
        foreach (var position in groupToDestroy)
        {
            var obj = gridStorage.GetObjectAt(position);
            if (obj != null)
            {
                // Remove from grid storage immediately
                gridStorage.RemoveObjectAt(position);
                
                // Animate destruction
                AnimateDestruction(obj.gameObject, () => {
                    animationsCompleted++;
                    if (animationsCompleted >= totalCubes)
                    {
                        // All destruction animations complete
                        EventQueueManager.Instance.RegisterAnimationComplete();
                    }
                });
            }
        }
        
        // If no objects to destroy, immediately complete
        if (totalCubes == 0)
        {
            EventQueueManager.Instance.RegisterAnimationComplete();
        }
    }
    
    private void AnimateDestruction(GameObject obj, System.Action onComplete)
    {
        if (obj != null)
        {
            // For now, just destroy immediately
            // In real implementation, use DOTween for animation
            GameObject.Destroy(obj);
            onComplete?.Invoke();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}