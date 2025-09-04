using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class DestroyCubesEvent : GameEvent
{
    private List<List<Vector2Int>> groupsToDestroy;
    private GridStorage gridStorage;
    
    public DestroyCubesEvent(List<List<Vector2Int>> groups, GridStorage storage, int priority = 4) : base(priority)
    {
        groupsToDestroy = groups;
        gridStorage = storage;
    }
    
    public override void Execute()
    {
        Debug.Log($"Destroying {groupsToDestroy.Count} groups of cubes");
        
        // Track animation count for proper sequencing
        EventQueueManager.Instance.RegisterAnimationStart();
        
        int totalCubes = 0;
        int animationsCompleted = 0;
        
        // Count total cubes to destroy
        foreach (var group in groupsToDestroy)
        {
            totalCubes += group.Count;
        }
        
        // Destroy each group
        foreach (var group in groupsToDestroy)
        {
            foreach (var position in group)
            {
                var obj = gridStorage.GetObjectAt(position);
                if (obj != null)
                {
                    // Remove from grid storage immediately
                    gridStorage.RemoveObjectAt(position);
                    
                    // Animate destruction (you can add tween animations here)
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
        }
        
        // If no objects to destroy, immediately complete
        if (totalCubes == 0)
        {
            EventQueueManager.Instance.RegisterAnimationComplete();
        }
    }
    
    private void AnimateDestruction(GameObject obj, System.Action onComplete)
    {
        // Simple scale-down animation (you can replace with more sophisticated animation)
        if (obj != null)
        {
            // For now, just destroy immediately and call completion
            // In a real implementation, you'd use DOTween or similar:
            // obj.transform.DOScale(0, 0.3f).OnComplete(() => {
            //     GameObject.Destroy(obj);
            //     onComplete?.Invoke();
            // });
            
            GameObject.Destroy(obj);
            onComplete?.Invoke();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}