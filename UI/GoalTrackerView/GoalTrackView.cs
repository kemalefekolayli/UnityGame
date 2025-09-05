using UnityEngine;
using TMPro;
public class GoalTrackView : MonoBehaviour
{
    [SerializeField] private GameObject stoneGoalPrefab;
    [SerializeField] private GameObject boxGoalPrefab;
    [SerializeField] private GameObject vaseGoalPrefab;
    
    
    private GoalTracker _goalTracker;
    private GoalObject stoneGoal;
    private GoalObject boxGoal;
    private GoalObject vaseGoal;
    
    private Transform parentTransform;


    void Start()
    {
        _goalTracker = FindFirstObjectByType<GoalTracker>();
        boxGoalPrefab.SetActive(true);
        stoneGoalPrefab.SetActive(true);
        vaseGoalPrefab.SetActive(true);

        InitializeGoals();
        UpdateText();
        
    }

    void InitializeGoals()
    {
        Vector3 worldPos = transform.position + new Vector3(1, 0, 0);
        GameObject StoneGoal = Instantiate(stoneGoalPrefab, worldPos, Quaternion.identity, parentTransform);
        stoneGoal = StoneGoal.GetComponent<GoalObject>();
        Vector3 worldPos2 = transform.position + new Vector3(0, 1, 0);
        GameObject BoxGoal = Instantiate(boxGoalPrefab, worldPos2, Quaternion.identity, parentTransform);
        boxGoal = BoxGoal.GetComponent<GoalObject>();
        Vector3 worldPos3 = transform.position + new Vector3(0, 0, 1);
        GameObject VaseGoal = Instantiate(vaseGoalPrefab, worldPos3, Quaternion.identity, parentTransform);
        vaseGoal = VaseGoal.GetComponent<GoalObject>();
  
    }
    void UpdateText()
    {
        stoneGoal.SetGoalText();
        boxGoal.SetGoalText();
        vaseGoal.SetGoalText();
    }
}