using Project.Scripts.GameQueue.Events;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class SpawnCubesEvent : GameEvent
{
    private GridStorage gridStorage;
    private int gridWidth;
    private int gridHeight;
    
    public SpawnCubesEvent(GridStorage storage, int width, int height, int priority = 7) : base(priority)
    {
        gridStorage = storage;
        gridWidth = width;
        gridHeight = height;
    }
    
    public override void Execute()
    {
        Debug.Log("Spawning new cubes to fill empty spaces at top");
        
        var cubeFactory = Object.FindFirstObjectByType<CubeFactory>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (cubeFactory == null || gridManager == null)
        {
            Debug.LogError("Missing CubeFactory or GridManager for spawning");
            return;
        }
        
        List<(AbstractGridObject obj, Vector3 startPos, Vector3 endPos)> spawnAnimations = 
            new List<(AbstractGridObject, Vector3, Vector3)>();
        
        // Check each column for empty spaces from TOP
        for (int x = 0; x < gridWidth; x++)
        {
            // Count how many empty spaces at the top of this column
            int emptyCount = 0;
            
            // Start from the top and count consecutive empty spaces
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                var existingObj = gridStorage.GetObjectAt(gridPos);
                
                if (existingObj == null)
                {
                    emptyCount++;
                }
                else
                {
                    // Hit a non-empty cell, stop counting
                    break;
                }
            }
            
            // Now spawn cubes for the empty spaces at the top
            for (int i = 0; i < emptyCount; i++)
            {
                // Calculate the grid position (from top down)
                int y = gridHeight - 1 - i;
                Vector2Int gridPos = new Vector2Int(x, y);
                
                // Get the final world position for this grid cell
                Vector3 finalWorldPos = gridManager.GridToWorldPosition(gridPos);
                
                // Calculate spawn position above the grid
                // Spawn higher for cubes that need to fall further
                float spawnHeightOffset = 2f + (i * 0.5f); // Stagger spawn heights
                Vector3 spawnPos = new Vector3(finalWorldPos.x, finalWorldPos.y + spawnHeightOffset, finalWorldPos.z);
                string randomColor = cubeFactory.GetRandomColor();
                var newCube = cubeFactory.CreateCube(randomColor, spawnPos, gridManager.transform, gridPos);
                
                if (newCube != null)
                {
                    // Mark as falling initially
                    if (newCube is CubeObject cube)
                    {
                        cube.IsFalling = true;
                    }
                    
                    // Add to animation list
                    spawnAnimations.Add((newCube, spawnPos, finalWorldPos));
                }
            }
        }
        
        // Animate all spawned cubes
        if (spawnAnimations.Count > 0)
        {
            EventQueueManager.Instance.RegisterAnimationStart();
            
            int totalAnimations = spawnAnimations.Count;
            int completedAnimations = 0;
            
            Debug.Log($"Spawning {totalAnimations} new cubes");
            
            foreach (var (obj, startPos, endPos) in spawnAnimations)
            {
                AnimateSpawn(obj, endPos, () => {
                    completedAnimations++;
                    if (completedAnimations >= totalAnimations)
                    {
                        Debug.Log("All spawn animations complete");
                        EventQueueManager.Instance.RegisterAnimationComplete();
                    }
                });
            }
        }
        else
        {
            Debug.Log("No empty spaces at top - no cubes to spawn");
        }
    }
    
    private void AnimateSpawn(AbstractGridObject obj, Vector3 targetPos, System.Action onComplete)
    {
        if (obj != null)
        {
            // Smooth falling animation with DOTween
            float fallDuration = 0.8f;
            
            // Create a sequence for more control
            Sequence fallSequence = DOTween.Sequence();
            
            // Move with bounce effect
            fallSequence.Append(
                obj.transform.DOMove(targetPos, fallDuration)
                    .SetEase(Ease.Linear) // TODO TRY A BUNCH AND PICK A CUTE ONE
            );
            
            // Optional: Add a subtle scale effect while falling
            fallSequence.Join(
                obj.transform.DOScale(1f, fallDuration * 0.3f)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine)
            );
            
            // When animation completes
            fallSequence.OnComplete(() => {
                if (obj is CubeObject cube)
                    cube.IsFalling = false;
                onComplete?.Invoke();
            });
            
            // Play the sequence
            fallSequence.Play();
        }
        else
        {
            onComplete?.Invoke();
        }
    }
}