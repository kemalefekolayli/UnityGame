using UnityEngine;
using System.Collections;

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
        float spacing = 100f;
        float currentX = -spacing;
        
        // Box obstacles
        if (_goalTracker.GetObstacleCount("bo") > 0)
        {
            boxGoal = CreateGoalObject<BoxGoalObject>(boxGoalPrefab, currentX);
            currentX += spacing;
        }
        
        // Stone obstacles  
        if (_goalTracker.GetObstacleCount("s") > 0)
        {
            stoneGoal = CreateGoalObject<StoneGoalObject>(stoneGoalPrefab, currentX);
            currentX += spacing;
        }
        
        // Vase obstacles
        if (_goalTracker.GetObstacleCount("v") > 0)
        {
            vaseGoal = CreateGoalObject<VaseGoalObject>(vaseGoalPrefab, currentX);
        }
    }
    
    T CreateGoalObject<T>(GameObject prefab, float xPosition) where T : GoalObject
    {
        GameObject obj = Instantiate(prefab, goalsContainer);
        T goalComponent = obj.GetComponent<T>();
        
        goalComponent.Initialize(_goalTracker);
        
        RectTransform rect = obj.GetComponent<RectTransform>();
        if (rect != null)
        {
            rect.anchoredPosition = new Vector2(xPosition, 0);
            rect.sizeDelta = new Vector2(80, 80);  
            rect.localScale = Vector3.one;  
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