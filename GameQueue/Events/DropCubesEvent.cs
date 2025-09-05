using UnityEngine;
using System.Collections.Generic;
using Project.Scripts.GameQueue.Events;

public class DropCubesEvent : GameEvent
{
    private GridStorage gridStorage;
    private int gridWidth;
    private int gridHeight;
    
    public DropCubesEvent(GridStorage storage, int width, int height, int priority = 3) : base(priority)
    {
        gridStorage = storage;
        gridWidth = width;
        gridHeight = height;
    }
    
    public override void Execute()
    {
        Debug.Log("Dropping cubes due to gravity");
        
        EventQueueManager.Instance.RegisterAnimationStart();
        
        bool anyMovement = false;
        int totalAnimations = 0;
        int completedAnimations = 0;
        
        // Process each column from bottom to top
        for (int x = 0; x < gridWidth; x++)
        {
            List<AbstractGridObject> columnObjects = new List<AbstractGridObject>();
            List<Vector2Int> originalPositions = new List<Vector2Int>();
            
            // Collect all objects in this column from bottom to top
            for (int y = 0; y < gridHeight; y++)
            {
                var obj = gridStorage.GetObjectAt(new Vector2Int(x, y));
                if (obj != null)
                {
                    columnObjects.Add(obj);
                    originalPositions.Add(new Vector2Int(x, y));
                    // Remove from current position in storage
                    gridStorage.RemoveObjectAt(new Vector2Int(x, y));
                }
            }
            
            // Place objects back from bottom, filling gaps
            for (int i = 0; i < columnObjects.Count; i++)
            {
                Vector2Int newPos = new Vector2Int(x, i);
                Vector2Int oldPos = originalPositions[i];
                
                if (newPos != oldPos)
                {
                    anyMovement = true;
                    totalAnimations++;
                    
                    // Update grid storage
                    gridStorage.SetObjectAt(newPos, columnObjects[i]);
                    columnObjects[i].GridPosition = newPos;
                    
                    // Animate the drop
                    AnimateDrop(columnObjects[i], newPos, () => {
                        completedAnimations++;
                        if (completedAnimations >= totalAnimations)
                        {
                            EventQueueManager.Instance.RegisterAnimationComplete();
                        }
                    });
                }
                else
                {
                    // No movement needed, just put back in storage
                    gridStorage.SetObjectAt(newPos, columnObjects[i]);
                }
            }
        }
        
        // If no movement occurred, complete immediately
        if (!anyMovement)
        {
            EventQueueManager.Instance.RegisterAnimationComplete();
        }
    }
    
    private void AnimateDrop(AbstractGridObject obj, Vector2Int targetGridPos, System.Action onComplete)
    {
        if (obj != null)
        {
            // Get proper world position from GridManager
            var gridManager = Object.FindFirstObjectByType<GridManager>();
            Vector3 targetWorldPos = gridManager.GridToWorldPosition(targetGridPos);
            // Mark as falling
            if (obj is CubeObject cube)
            {
                cube.IsFalling = true;
            }
            
            // Simple immediate movement (replace with smooth animation)
            // In real implementation, use DOTween:
            // obj.transform.DOMove(targetWorldPos, 0.5f).OnComplete(() => {
            //     if (obj is CubeObject cube) cube.IsFalling = false;
            //     onComplete?.Invoke();
            // });
            
            obj.transform.position = targetWorldPos;
            if (obj is CubeObject cube2)
            {
                cube2.IsFalling = false;
            }
            onComplete?.Invoke();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
    
}