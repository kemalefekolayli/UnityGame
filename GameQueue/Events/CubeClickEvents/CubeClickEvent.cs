using UnityEngine;

public class CubeClickEvent : GameEvent
{
    public Vector2Int Position { get; private set; }
    public CubeObject.CubeColor Color { get; private set; }
    public CubeObject Cube { get; private set; }
    
    public CubeClickEvent(CubeObject cube, int priority = 0) : base(priority)
    {
        Cube = cube;
        Position = cube.GridPosition;
        Color = cube.GetCubeColor();
    }
    
    public override void Execute()
    {
        Debug.Log($"Processing cube click at {Position} with color {Color}");
        
        // GridManager'a gönder (adjacency check, blast logic vs)
        // GridManager.Instance.ProcessCubeClick(Cube);
    }
}