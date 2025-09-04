using UnityEngine;

[CreateAssetMenu(fileName = "GridSettings", menuName = "Dream Games/Grid Settings")]
public class GridSettings : ScriptableObject
{
    [Header("Visual Settings")]
    [SerializeField] private float cubeScale = 0.35f;
    [SerializeField] private float cellSpacing = 0.5f;
    
    [Header("UI Settings")]
    [SerializeField] private float uiCellSize = 70f;
    [SerializeField] private float uiCellSpacing = 5f;
    
    [Header("Layout")]
    [SerializeField] private Vector2 gridOriginOffset = new Vector2(0f, -1f);
    
    // Public getters
    public float CubeScale => cubeScale;
    public float CellSpacing => cellSpacing;
    public float UICellSize => uiCellSize;
    public float UICellSpacing => uiCellSpacing;
    public Vector2 GridOriginOffset => gridOriginOffset;
    
    public float ActualCubeGap => cellSpacing - cubeScale;
    
    [Header("Debug Info")]
    [SerializeField, TextArea(2, 3)] 
    private string debugInfo = "Gap between cubes will be calculated as: CellSpacing - CubeScale";
    
    private void OnValidate()
    {
        if (cellSpacing < cubeScale)
        {
            cellSpacing = cubeScale + 0.05f;
            Debug.LogWarning("CellSpacing adjusted to be larger than CubeScale to prevent overlapping");
        }
        
        debugInfo = $"Current gap: {ActualCubeGap:F2}f units";
    }
}