using UnityEngine;
public class VaseObstacle : AbstractGridObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Sprite spriteDamaged;
    public void Initialize(Vector2Int Position, Sprite Sprite )
    {
        this.health = 2;
        GridPosition = Position;
        SetSprite(Sprite);
    }
    
    public void SetSprite(Sprite sprite) // use this to switch to damaged
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = sprite;
    }
    
}