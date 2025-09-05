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


    void Start()
    {
        _goalTracker = FindFirstObjectByType<GoalTracker>();
        boxGoalPrefab.SetActive(true);
        stoneGoalPrefab.SetActive(true);
        vaseGoalPrefab.SetActive(true);
        stoneGoal = stoneGoalPrefab.GetComponent<StoneGoalObject>(); 
        vaseGoal = vaseGoalPrefab.GetComponent<VaseGoalObject>();
        boxGoal = boxGoalPrefab.GetComponent<BoxGoalObject>();
        
    }

    void UpdateText()
    {
        stoneGoal.SetGoalText();
        boxGoal.SetGoalText();
        vaseGoal.SetGoalText();
    }
}