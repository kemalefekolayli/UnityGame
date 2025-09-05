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
        // Collect all objects first before destroying
        List<GameObject> objectsToDestroy = new List<GameObject>();
        
        foreach (var position in groupToDestroy)
        {
            var obj = gridStorage.GetObjectAt(position);
            if (obj != null)
            {
                objectsToDestroy.Add(obj.gameObject);
                gridStorage.RemoveObjectAt(position);
            }
            else
            {
                Debug.LogWarning($"No object found at position {position}");
            }
        }
        
        // Always register animation start/complete even if no objects
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
    }
    
    private void AnimateDestruction(GameObject obj, System.Action onComplete) // this will a separete event class later on
    {
        if (obj != null)
        {
            Debug.Log($"Destroying GameObject: {obj.name}");
            
            // For now, just destroy immediately
            // In real implementation, use DOTween for animation
            GameObject.Destroy(obj);
            
            //  Always call onComplete
            onComplete?.Invoke();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}