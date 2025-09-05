using UnityEngine;
using TMPro;

public abstract class GoalObject : MonoBehaviour 
{
    protected SpriteRenderer _spriteRenderer;
    protected TMP_Text _goalText;
    protected GoalTracker _goalTracker;
    protected bool isActiveInLevel;

    // Initialize is called by GoalTrackView after instantiation
    public void Initialize(GoalTracker tracker)
    {
        _goalTracker = tracker;
        
        // Get components
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _goalText = GetComponentInChildren<TMP_Text>();
        
        if (_goalText == null)
        {
            Debug.LogError($"No TMP_Text found in children of {gameObject.name}. Make sure the prefab has a child with TMP_Text component!");
        }
        
        // Set initial text
        SetGoalText();
    }

    public abstract void SetGoalText();
    
    // Helper method to safely set text
    protected void SetText(string text)
    {
        if (_goalText != null)
        {
            _goalText.text = text;
        }
        else
        {
            Debug.LogWarning($"Trying to set text but _goalText is null on {gameObject.name}");
        }
    }
}