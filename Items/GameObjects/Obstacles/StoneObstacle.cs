using UnityEngine;
public class StoneObstacle : AbstractGridObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void Initialize(Vector2Int Position, Sprite Sprite )
    {
        GridPosition = Position;
        SetSprite(Sprite);
    }
    
    public void SetSprite(Sprite sprite)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprite;
    }
    
}