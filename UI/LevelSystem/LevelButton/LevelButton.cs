using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour {
    
    void GoToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
