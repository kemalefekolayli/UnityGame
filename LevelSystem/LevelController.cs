using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int LevelNumber;
    private LevelObject currentLevelData;
    
    
    void Awake() // we need this fella to persist between scenes and we only need 1 levelscene
    {
        if (FindObjectsByType<LevelController>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public LevelObject LoadLevelData(int levelNumber)
    {
        Debug.Log("Loading level");
        string levelName = $"levels/level_{levelNumber:D2}";
        TextAsset jsonFile = Resources.Load<TextAsset>(levelName);

        if (jsonFile != null)
        {
            return JsonUtility.FromJson<LevelObject>(jsonFile.text);
        }

        Debug.LogError($"Level {levelNumber} bulunamadı!");
        return null;
    }
    
    
    public void SetLevelData(LevelObject levelData)
    {
        currentLevelData = levelData;
    }

    public LevelObject GetLevelData()
    {
        return currentLevelData;
    }
}