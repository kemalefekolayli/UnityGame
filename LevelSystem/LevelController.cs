using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int LevelNumber;
    private LevelObject currentLevelData;
    
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