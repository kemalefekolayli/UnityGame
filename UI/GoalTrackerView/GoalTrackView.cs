using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GoalTrackView : MonoBehaviour
{
    [Header("Goal Prefabs")]
    [SerializeField] private GameObject stoneGoalPrefab;
    [SerializeField] private GameObject boxGoalPrefab;
    [SerializeField] private GameObject vaseGoalPrefab;
    
    [Header("Parent Transform")]
    [SerializeField] private Transform goalsContainer;
    
    private GoalTracker _goalTracker;
    private BoxGoalObject boxGoal;
    private StoneGoalObject stoneGoal;
    private VaseGoalObject vaseGoal;

    void Start()
    {
        _goalTracker = FindFirstObjectByType<GoalTracker>();
        
        if (goalsContainer == null)
            goalsContainer = transform;
    }
    
    // Called by GridManager after grid is built
    public void InitializeGoals()
    {
        _goalTracker.SetGoals();
        CreateGoalUI();
    }

    void CreateGoalUI()
    {
        // Grid Layout Group ekle
        var gridLayout = goalsContainer.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
            gridLayout = goalsContainer.gameObject.AddComponent<GridLayoutGroup>();
    
        // Goal sayısını hesapla
        int goalCount = 0;
        if (_goalTracker.GetObstacleCount("bo") > 0) goalCount++;
        if (_goalTracker.GetObstacleCount("s") > 0) goalCount++;
        if (_goalTracker.GetObstacleCount("v") > 0) goalCount++;
    
        // Layout ayarları
        gridLayout.cellSize = new Vector2(80, 80);
        gridLayout.spacing = new Vector2(10, 10);
        gridLayout.childAlignment = TextAnchor.MiddleCenter;
    
        if (goalCount <= 2)
        {
            // 1 veya 2 goal: tek satırda
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            gridLayout.constraintCount = 1;
        }
        else
        {
            // 3 goal: 2 sütun (2+1 düzeni)
            gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridLayout.constraintCount = 2;
        }
    
        // Goalleri oluştur (eski kod aynı kalacak)
        if (_goalTracker.GetObstacleCount("bo") > 0)
        {
            boxGoal = CreateGoalObject<BoxGoalObject>(boxGoalPrefab, 0);
        }
    
        if (_goalTracker.GetObstacleCount("s") > 0)
        {
            stoneGoal = CreateGoalObject<StoneGoalObject>(stoneGoalPrefab, 0);
        }
    
        if (_goalTracker.GetObstacleCount("v") > 0)
        {
            vaseGoal = CreateGoalObject<VaseGoalObject>(vaseGoalPrefab, 0);
        }
    }
    
    T CreateGoalObject<T>(GameObject prefab, float xPosition) where T : GoalObject
    {
        GameObject obj = Instantiate(prefab, goalsContainer);
        T goalComponent = obj.GetComponent<T>();
    
        goalComponent.Initialize(_goalTracker);
    
        // Grid Layout Group pozisyonu otomatik ayarlayacak, 
        // sadece scale'i ayarlayalım
        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.localScale = Vector3.one;
            // anchoredPosition'ı kaldır - Grid Layout Group halleder
        }
        
        return goalComponent;
    }
    
    public void RefreshGoals()
    {
        _goalTracker.SetGoals();
        
        if (boxGoal != null) boxGoal.SetGoalText();
        if (stoneGoal != null) stoneGoal.SetGoalText();
        if (vaseGoal != null) vaseGoal.SetGoalText();
    }
}