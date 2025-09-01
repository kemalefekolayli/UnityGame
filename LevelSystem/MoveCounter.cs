using UnityEngine;

public class MoveCounter : MonoBehaviour {
    [SerializeField] LevelController levelController;
    public int MaxMoveCount;
    public int CurrentMoveCount;


    void Start()
    {
        MaxMoveCount = levelController.GetLevelData().GetMoveCount();
    }
    
    
    public int GetMoveCountsLeft()
    {
        return CurrentMoveCount;
    }

    public bool DecreaseMoveCount()
    {
        if (CurrentMoveCount < 0)
        {
            Debug.Log("Current move count is less than 0");
            return false;
        }
        this.CurrentMoveCount = this.CurrentMoveCount - 1;
        return true;
    }
}
