using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class DestroySpecificGroupEvent : GameEvent
{
    private List<Vector2Int> groupToDestroy;
    private GridStorage gridStorage;
    
    public DestroySpecificGroupEvent(List<Vector2Int> group, GridStorage storage, int priority = 9) : base(priority)
    {
        groupToDestroy = new List<Vector2Int>(group); // FIX 1: Create a copy to avoid reference issues
        gridStorage = storage;
    }
    
    public override void Execute()
    {
        Debug.Log($"Destroying specific group of {groupToDestroy.Count} cubes");
        
        // FIX 2: Collect all objects first before destroying
        List<GameObject> objectsToDestroy = new List<GameObject>();
        
        foreach (var position in groupToDestroy)
        {
            Debug.Log($"Checking position {position}");
            var obj = gridStorage.GetObjectAt(position);
            
            if (obj != null)
            {
                Debug.Log($"Found cube at {position}: {obj.GetType().Name}");
                objectsToDestroy.Add(obj.gameObject);
                
                // Remove from grid storage immediately
                gridStorage.RemoveObjectAt(position);
            }
            else
            {
                Debug.LogWarning($"No object found at position {position}");
            }
        }
        
        // FIX 3: Always register animation start/complete even if no objects
        if (objectsToDestroy.Count > 0)
        {
            EventQueueManager.Instance.RegisterAnimationStart();
            
            int animationsCompleted = 0;
            int totalAnimations = objectsToDestroy.Count;
            
            // Destroy all collected objects
            foreach (var obj in objectsToDestroy)
            {
                AnimateDestruction(obj, () => {
                    animationsCompleted++;
                    Debug.Log($"Destruction animation completed: {animationsCompleted}/{totalAnimations}");
                    
                    if (animationsCompleted >= totalAnimations)
                    {
                        Debug.Log("All destruction animations complete");
                        EventQueueManager.Instance.RegisterAnimationComplete();
                    }
                });
            }
        }
        else
        {
            // FIX 4: No objects to destroy - don't block with animations
            Debug.LogWarning("No objects were found to destroy");
        }
    }
    
    private void AnimateDestruction(GameObject obj, System.Action onComplete)
    {
        if (obj != null)
        {
            Debug.Log($"Destroying GameObject: {obj.name}");
            
            // For now, just destroy immediately
            // In real implementation, use DOTween for animation
            GameObject.Destroy(obj);
            
            // FIX 5: Always call onComplete
            onComplete?.Invoke();
        }
        else
        {
            Debug.LogWarning("Tried to animate null GameObject");
            onComplete?.Invoke();
        }
    }
}