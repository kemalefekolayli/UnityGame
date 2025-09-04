using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour 
{
    [SerializeField] private Button button;
    [SerializeField] private LevelController levelController;
    [SerializeField] private BoxCollider2D buttonCollider;
    
    private int levelNumber;
    private const string LAST_PLAYED_LEVEL_KEY = "LastPlayedLevel";
    private const int TOTAL_LEVELS = 10;
    
    void Start()
    {
        levelNumber = PlayerPrefs.GetInt(LAST_PLAYED_LEVEL_KEY, 1);
        if (levelNumber > TOTAL_LEVELS)
        {
            // Button'u deaktif etmek veya farklı bir davranış ekleyebilirsiniz
            Debug.Log("All levels completed!");
            levelNumber = TOTAL_LEVELS; // Son level'ı tekrar oynayabilir
        }
    }
    
    void LoadSelectedLevel()
    {
        Debug.Log($"Loading level {levelNumber}");
        
        levelController.SetLevelData(levelController.LoadLevelData(levelNumber));
        
        if (levelController.GetLevelData() != null)
        {
            Debug.Log($"Level {levelNumber} yükleniyor...");
            SaveLastPlayedLevel(levelNumber);
            SceneManager.LoadScene("LevelScene");
        }
        else
        {
            Debug.LogError($"Level {levelNumber} bulunamadı!");
        }
    }
    
    public void SaveLastPlayedLevel(int level)
    {
        PlayerPrefs.SetInt(LAST_PLAYED_LEVEL_KEY, level);
        PlayerPrefs.Save();
    }
    
    public void SetLevelNumber(int number)
    {
        levelNumber = number;
    }
    
    void OnMouseDown()
    {
        LoadSelectedLevel();
    }
}