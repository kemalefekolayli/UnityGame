#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class LevelEditorMenu : MonoBehaviour
{
    private const string LAST_PLAYED_LEVEL_KEY = "LastPlayedLevel";
    
    [MenuItem("Dream Games/Set Level/Level 1")]
    static void SetLevel1()
    {
        SetLastPlayedLevel(1);
    }
    
    [MenuItem("Dream Games/Set Level/Level 5")]
    static void SetLevel5()
    {
        SetLastPlayedLevel(5);
    }
    
    [MenuItem("Dream Games/Set Level/Level 10")]
    static void SetLevel10()
    {
        SetLastPlayedLevel(10);
    }
    
    [MenuItem("Dream Games/Set Level/All Levels Completed")]
    static void SetAllLevelsCompleted()
    {
        SetLastPlayedLevel(11); // 10'dan büyük = tüm leveller tamamlandı
    }
    
    [MenuItem("Dream Games/Reset Progress")]
    static void ResetProgress()
    {
        SetLastPlayedLevel(1);
    }
    
    [MenuItem("Dream Games/Show Current Level")]
    static void ShowCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt(LAST_PLAYED_LEVEL_KEY, 1);
        Debug.Log($"Current saved level: {currentLevel}");
        EditorUtility.DisplayDialog("Current Level", $"Last played level: {currentLevel}", "OK");
    }
    
    static void SetLastPlayedLevel(int level)
    {
        PlayerPrefs.SetInt(LAST_PLAYED_LEVEL_KEY, level);
        PlayerPrefs.Save();
        Debug.Log($"Last played level set to: {level}");
        
        // Editor'da anında görmek için Scene'i refresh et
        if (Application.isPlaying)
        {
            // Oyun çalışıyorsa, MainScene'deki text'leri güncelle
            var levelButtonText = GameObject.FindFirstObjectByType<LevelButtonText>();
            if (levelButtonText != null)
            {
                levelButtonText.RefreshText(); // Text'i yeniden yükle
            }
        }
    }
}
#endif