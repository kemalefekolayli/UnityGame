using UnityEngine;

public abstract class IGridObject : MonoBehaviour
{
    protected CubeColor cubeColor;
    protected Transform GridObjectTransform;
    protected Vector3 GridObjectPosition;
    protected Vector2Int gridPosition;
    protected int health = 1;
    protected bool isDestroyed = false;

    
    public enum CubeColor
    {
        Red,
        Green,
        Blue,
        Yellow
    }
}