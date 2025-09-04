using Project.Scripts.GameQueue.Events;
using UnityEngine;

public class SpawnCubesEvent : GameEvent
{
    private GridStorage gridStorage;
    private int gridWidth;
    private int gridHeight;
    
    public SpawnCubesEvent(GridStorage storage, int width, int height, int priority = 2) : base(priority)
    {
        gridStorage = storage;
        gridWidth = width;
        gridHeight = height;
    }
    
    public override void Execute()
    {
        Debug.Log("Spawning new cubes to fill empty spaces");
        
        EventQueueManager.Instance.RegisterAnimationStart();
        
        var cubeFactory = Object.FindFirstObjectByType<CubeFactory>();
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        
        if (cubeFactory == null || gridManager == null)
        {
            Debug.LogError("Missing CubeFactory or GridManager for spawning");
            EventQueueManager.Instance.RegisterAnimationComplete();
            return;
        }
        
        int totalSpawned = 0;
        int animationsCompleted = 0;
        
        // Check top row for empty spaces
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                var existingObj = gridStorage.GetObjectAt(gridPos);
                
                if (existingObj == null)
                {
                    // Empty space found - spawn a new random cube
                    Vector3 worldPos = GridToWorldPosition(gridPos);
                    Vector3 spawnPos = new Vector3(worldPos.x, worldPos.y + 2f, worldPos.z); // Spawn above
                    
                    string randomColor = cubeFactory.GetRandomColor();
                    var newCube = cubeFactory.CreateCube(randomColor, spawnPos, gridManager.transform, gridPos);
                    
                    totalSpawned++;
                    
                    // Animate falling into position
                    AnimateSpawn(newCube, worldPos, () => {
                        animationsCompleted++;
                        if (animationsCompleted >= totalSpawned)
                        {
                            EventQueueManager.Instance.RegisterAnimationComplete();
                        }
                    });
                }
            }
        }
        
        // If nothing was spawned, complete immediately
        if (totalSpawned == 0)
        {
            EventQueueManager.Instance.RegisterAnimationComplete();
        }
    }
    
    private void AnimateSpawn(AbstractGridObject obj, Vector3 targetPos, System.Action onComplete)
    {
        if (obj != null)
        {
            // Mark as falling during animation
            if (obj is CubeObject cube)
            {
                cube.IsFalling = true;
            }
            
            // Simple immediate movement (replace with smooth animation)
            // In real implementation:
            // obj.transform.DOMove(targetPos, 0.8f).SetEase(Ease.OutBounce).OnComplete(() => {
            //     if (obj is CubeObject cube) cube.IsFalling = false;
            //     onComplete?.Invoke();
            // });
            
            obj.transform.position = targetPos;
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
    
    private Vector3 GridToWorldPosition(Vector2Int gridPos)
    {
        // This should match your GridManager's conversion logic
        var gridManager = Object.FindFirstObjectByType<GridManager>();
        if (gridManager != null)
        {
            // You'll need to expose this method in GridManager
            // For now, using a simple calculation
            return new Vector3(gridPos.x * 0.5f, gridPos.y * 0.5f, 0);
        }
        return Vector3.zero;
    }
}