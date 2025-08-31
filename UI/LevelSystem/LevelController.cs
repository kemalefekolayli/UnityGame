using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int LevelNumber;
    
    public LevelObject LoadLevel(int levelNumber)
    {
        string levelName = $"levels/level_{levelNumber:D2}";
        TextAsset jsonFile = Resources.Load<TextAsset>(levelName);

        if (jsonFile != null)
        {
            return JsonUtility.FromJson<LevelObject>(jsonFile.text);
        }

        Debug.LogError($"Level {levelNumber} bulunamadı!");
        return null;
    }
}