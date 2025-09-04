using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressionHelper : MonoBehaviour
{
    private const string LAST_PLAYED_LEVEL_KEY = "LastPlayedLevel";
    private const int TOTAL_LEVELS = 10;
    
    // Level kazanıldığında çağrılacak
    public static void OnLevelWin()
    {
        int currentLevel = PlayerPrefs.GetInt(LAST_PLAYED_LEVEL_KEY, 1);
        
        if (currentLevel < TOTAL_LEVELS)
        {
            // Bir sonraki level'e geç
            int nextLevel = currentLevel + 1;
            PlayerPrefs.SetInt(LAST_PLAYED_LEVEL_KEY, nextLevel);
            PlayerPrefs.Save();
            
            Debug.Log($"Level {currentLevel} completed! Moving to level {nextLevel}");
        }
        else
        {
            // Tüm leveller tamamlandı
            PlayerPrefs.SetInt(LAST_PLAYED_LEVEL_KEY, TOTAL_LEVELS + 1); // "Finished" göstermek için
            PlayerPrefs.Save();
            
            Debug.Log("All levels completed!");
        }
        
        // Celebration particles göster (sonra eklenecek)
        // ShowCelebrationParticles();
        
        // MainScene'e dön
        SceneManager.LoadScene("MainScene");
    }

    public static void OnLevelLose()
    {
        // Fail popup göster (sonra eklenecek)
        Debug.Log("Level failed! Show fail popup");
    }
    
    public static void RetryLevel()
    {
        // Mevcut level'ı tekrar yükle
        SceneManager.LoadScene("LevelScene");
    }

    public static void ReturnToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}