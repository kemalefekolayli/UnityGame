using UnityEngine;
using TMPro;

public class LevelButtonText : MonoBehaviour
{
    [SerializeField] public TMP_Text LevelText;
    private const string LAST_PLAYED_LEVEL_KEY = "LastPlayedLevel";
    private const int TOTAL_LEVELS = 10;
    
    void Start()
    {
        RefreshText();
    }

    public void RefreshText()
    {

        int lastPlayedLevel = PlayerPrefs.GetInt(LAST_PLAYED_LEVEL_KEY, 1);
        
        if (LevelText == null)
            LevelText = GetComponent<TMP_Text>();

        if (lastPlayedLevel > TOTAL_LEVELS)
        {
            LevelText.text = "Finished";
        }
        else
        {
            LevelText.text = "Level " + lastPlayedLevel.ToString();
        }
    }
}