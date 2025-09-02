using Project.Scripts.GameQueue.Events;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CubeObject : AbstractGridObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CubeColor cubeColor;
    [SerializeField] private Sprite[] colorSprites;

    
    public enum CubeColor
    {
        r, g, b, y, rand
    }
    
    public CubeColor Color => cubeColor;
    public bool IsFalling { get; set; }
    
    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        // Input blocked kontrolü
        if (EventQueueManager.Instance.InputBlocked)
        {
            Debug.Log("Input blocked - animation in progress");
            return;
        }
        
        // Düşüyor mu kontrolü
        if (IsFalling)
        {
            Debug.Log("Cannot click falling cube");
            return;
        }
        
        // Event oluştur ve queue'ya ekle
        var clickEvent = new CubeClickEvent(this);
        EventQueueManager.Instance.EnqueueEvent(clickEvent);
    }
    
    public void Initialize(Vector2Int Position, string ColorT, Sprite Sprite )
    {
        GridPosition = Position;
        SetColor(ColorT);
        SetSprite(Sprite);
        UpdateSprite();
    }

    
    public void UpdateSprite()
    {
        if (colorSprites != null && colorSprites.Length > (int)cubeColor)
        {
            GetComponent<SpriteRenderer>().sprite = colorSprites[(int)cubeColor];
        }
    }

    public CubeColor GetCubeColor()
    {
        return cubeColor;
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
        else {
            Debug.LogWarning($"Invalid color: {colorT}");
            cubeColor = CubeColor.r;
        }
    }
}