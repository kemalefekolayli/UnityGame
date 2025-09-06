using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class DestroySpecificGroupEvent : GameEvent
{
    private List<Vector2Int> groupToDestroy;
    private GridStorage gridStorage;
    
    public DestroySpecificGroupEvent(List<Vector2Int> group, GridStorage storage, int priority = 9) : base(priority)
    {
        groupToDestroy = new List<Vector2Int>(group);
        gridStorage = storage;
    }
    
    public override void Execute()
    {
        // Collect all objects first before destroying
        List<(GameObject obj, CubeColor color, Vector3 position)> objectsToDestroy = new List<(GameObject, CubeColor, Vector3)>();
        
        foreach (var position in groupToDestroy)
        {
            var obj = gridStorage.GetObjectAt(position);
            if (obj != null)
            {
                // Get color if it's a cube
                CubeColor color = CubeColor.rand;
                if (obj is CubeObject cube)
                {
                    color = cube.GetCubeColor();
                }
                
                objectsToDestroy.Add((obj.gameObject, color, obj.transform.position));
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
            
            // Destroy all collected objects with particles
            foreach (var (obj, color, position) in objectsToDestroy)
            {
                AnimateDestruction(obj, color, position, () => {
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
    
    private void AnimateDestruction(GameObject obj, CubeColor color, Vector3 position, System.Action onComplete)
    {
        if (obj != null)
        {
            Debug.Log($"Destroying GameObject: {obj.name} with particles");
            
            // Play particle effect if it's a colored cube
            if (color != CubeColor.rand && CubeParticleController.Instance != null)
            {
                CubeParticleController.Instance.PlayDestructionParticles(position, color);
            }
            
            // For now, just destroy immediately
            // Later you can add scaling animation before destruction
            GameObject.Destroy(obj);
            
            // Always call onComplete
            onComplete?.Invoke();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}