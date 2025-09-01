using UnityEngine;

public class MoveCounter : MonoBehaviour {
    
    public int MaxMoveCount;
    public int CurrentMoveCount;
    public LevelController levelController;
    public MovesLeftText movesLeftText;


    void Start()
    {
        levelController = FindFirstObjectByType<LevelController>();
        if (levelController != null && levelController.GetLevelData() != null)
        {
            Debug.Log("Loading level data into movecounter");
            MaxMoveCount = levelController.GetLevelData().GetMoveCount();
            CurrentMoveCount = MaxMoveCount;
        }
        movesLeftText.SetMovesLeftText(CurrentMoveCount);
    }
    
    
    public int GetMoveCountsLeft()
    {
        return CurrentMoveCount;
    }

    public bool DecreaseMoveCount()
    {
        if (CurrentMoveCount <= 0)
        {
            Debug.Log("Current move count is less than 0");
            return false;
        }
        this.CurrentMoveCount = this.CurrentMoveCount - 1;
        movesLeftText.SetMovesLeftText(this.CurrentMoveCount);
        return true;
    }
}
