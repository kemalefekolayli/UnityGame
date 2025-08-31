using UnityEngine;

public abstract class GridObject :  MonoBehaviour
{
    
    protected Transform GridObjectTransform;
    protected Vector3 GridObjectPosition;
    protected Vector2Int gridPosition;
    protected int health;
    protected bool isDestroyed = false;
    protected SpriteRenderer spriteRenderer;

    
    
}