using Project.Scripts.GameQueue.Events;
using UnityEngine;

public enum CubeColor
{
    r, g, b, y, rand
}

[RequireComponent(typeof(BoxCollider2D))]
public class CubeObject : AbstractGridObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CubeColor cubeColor;
    [SerializeField] private Sprite rocketHintSprite;
    [SerializeField] private Sprite regularSprite;

    private bool isGrouped;
    private static float lastClickTime = 0f; // FIX 1: Track last click time
    private const float CLICK_COOLDOWN = 0.1f; // FIX 2: 100ms cooldown between clicks
    
    public CubeColor Color => cubeColor;
    public bool IsFalling { get; set; }
    
    void OnMouseDown()
    {
        // FIX 3: Implement click cooldown to prevent double-clicking
        float currentTime = Time.time;
        if (currentTime - lastClickTime < CLICK_COOLDOWN)
        {
            Debug.Log("Click ignored - too soon after last click");
            return;
        }
        lastClickTime = currentTime;
        
        Debug.Log($"OnMouseDown at position {GridPosition}");
        
        // Input blocked kontrolü
        if (EventQueueManager.Instance.InputBlocked)
        {
            Debug.Log("Input blocked - animation or processing in progress");
            return;
        }
        
        // FIX 4: Check if we're already processing
        if (EventQueueManager.Instance.IsProcessing)
        {
            Debug.Log("Already processing an event - ignoring click");
            return;
        }
        
        // Düşüyor mu kontrolü
        if (IsFalling)
        {
            Debug.Log("Cannot click falling cube");
            return;
        }
        
        // FIX 5: Verify this cube still exists in storage
        var gridStorage = FindFirstObjectByType<GridStorage>();
        if (gridStorage != null)
        {
            var storedObj = gridStorage.GetObjectAt(GridPosition);
            if (storedObj != this)
            {
                Debug.LogWarning($"This cube is not in storage at {GridPosition} - may be orphaned");
                return;
            }
        }
        
        // Event oluştur ve queue'ya ekle
        var clickEvent = new CubeClickEvent(this);
        EventQueueManager.Instance.EnqueueEvent(clickEvent);
    }
    
    public void Initialize(Vector2Int Position, string ColorT, Sprite RegularSprite, Sprite RocketHintSprite)
    {
        rocketHintSprite = RocketHintSprite;
        regularSprite = RegularSprite;
        GridPosition = Position;
        SetColor(ColorT);
        SetSprite(regularSprite);
    }

    public CubeColor GetCubeColor()
    {
        return cubeColor;
    }

    public void SetRocketSprite()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = rocketHintSprite;
    }

    public void SetRegularSprite()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = regularSprite;
    }
    
    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprite;
    }

    public void SetColor(string colorT)
    {
        if (colorT == "r")
            cubeColor = CubeColor.r;
        else if (colorT == "g")
            cubeColor = CubeColor.g;
        else if (colorT == "b")
            cubeColor = CubeColor.b;
        else if (colorT == "y")
            cubeColor = CubeColor.y;
        else if (colorT == "rand")
        {
            // Choose a random color
            cubeColor = (CubeColor)Random.Range(0, 4);
        }
        else
        {
            Debug.LogWarning($"Invalid color: {colorT}");
            cubeColor = CubeColor.r;
        }
    }

    public Vector2Int GetGridPos()
    {
        return GridPosition;
    }
    
    // FIX 6: Clean up when destroyed
    void OnDestroy()
    {
        // Make sure we're removed from storage if we're being destroyed
        var gridStorage = FindFirstObjectByType<GridStorage>();
        if (gridStorage != null)
        {
            var storedObj = gridStorage.GetObjectAt(GridPosition);
            if (storedObj == this)
            {
                gridStorage.RemoveObjectAt(GridPosition);
            }
        }
    }
}