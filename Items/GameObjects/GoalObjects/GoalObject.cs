using UnityEngine;
using TMPro;
public abstract class GoalObject : MonoBehaviour {
        
        protected SpriteRenderer _spriteRenderer;
        protected bool isActiveInLevel;
        protected TMP_Text _goalText;
        protected GoalTracker _goalTracker;

        void Start()
        {
                _spriteRenderer = GetComponent<SpriteRenderer>();
                _goalText = GetComponentInChildren<TMP_Text>();
        }

        public virtual void SetGoalText()
        {
               _goalText.text =  _goalTracker.GetObstacleCount("string").ToString();
        }
}
