using UnityEngine;

public abstract class AbstractGridObject : MonoBehaviour
{
    
    protected Transform GridObjectTransform;
    protected Vector3 GridObjectPosition;
    public Vector2Int GridPosition;
    protected int health = 1;
    protected bool isDestroyed = false;


    
    
}