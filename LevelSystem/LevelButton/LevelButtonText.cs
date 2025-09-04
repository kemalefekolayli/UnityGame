using UnityEngine;
using TMPro;
public class LevelButtonText :  MonoBehaviour
{
    private LevelController _levelController;
    [SerializeField] public TMP_Text LevelText;
    
    void Start()
    {
        _levelController = FindFirstObjectByType<LevelController>();
        if (_levelController != null && _levelController.GetLevelData() != null)
        {
            int LevelNumber = _levelController.GetLevelData().GetLevelNumber();
            LevelText = GetComponent<TMP_Text>();
            LevelText.text = "Level " + LevelNumber.ToString();
        }
        
    }
    
}