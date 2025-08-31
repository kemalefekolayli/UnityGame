using UnityEngine;
using TMPro;
public class LevelButtonText :  MonoBehaviour
{
    private int LevelNumber = 1;
    [SerializeField] public TMP_Text LevelText;
    
    void Start()
    {
        LevelText = GetComponent<TMP_Text>();
        LevelText.text = "Level " + LevelNumber.ToString();
    }
    
}