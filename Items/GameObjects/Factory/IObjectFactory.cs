using UnityEngine;
public interface ObjectFactory<T> where T : AbstractGridObject
{

    public AbstractGridObject CreateCube(string color, Vector3 worldPos, Transform gridParent, Vector2Int gridPos);
}