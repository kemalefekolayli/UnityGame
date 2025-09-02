using UnityEngine;
public interface ObjectFactory<T> where T : AbstractGridObject
{
    public AbstractGridObject CreateObject(Vector2 location, Transform parent, float cellSize, GridManager manager, Vector2Int gridPos);
}