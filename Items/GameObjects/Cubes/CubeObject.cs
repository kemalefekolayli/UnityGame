using Project.Scripts.GameQueue.Events;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CubeObject : AbstractGridObject
{
    [SerializeField] private CubeColor cubeColor;
    [SerializeField] private Sprite[] colorSprites;

    
    public enum CubeColor
    {
        Red, Green, Blue, Yellow
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
    
    public void Initialize(CubeColor color, Vector2Int position)
    {
        cubeColor = color;
        GridPosition = position;
        UpdateSprite();
    }
    
    void UpdateSprite()
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
}