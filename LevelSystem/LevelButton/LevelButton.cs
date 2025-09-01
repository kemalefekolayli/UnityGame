using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour 
{
    [SerializeField] private Button button;
    [SerializeField] private int levelNumber = 1;
    [SerializeField] private LevelController levelController;
    [SerializeField] private BoxCollider2D buttonCollider;
    

    
    void LoadSelectedLevel()
    {
        Debug.Log("Loading selected level");
        levelController.SetLevelData(levelController.LoadLevelData(levelNumber)) ;
        
        if (levelController.GetLevelData() != null)
        {
            Debug.Log($"Level {levelNumber} yükleniyor...");
            
            // Level scene'ına geç
            // Burada iki seçenek var:
            
            // Seçenek 1: Belirli level scene'ına git
            SceneManager.LoadScene($"LevelScene");
            
            // Seçenek 2: Genel game scene'ına git (level datası ile)
            // PlayerPrefs.SetInt("SelectedLevel", levelNumber);
            // SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.LogError($"Level {levelNumber} bulunamadı!");
        }
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