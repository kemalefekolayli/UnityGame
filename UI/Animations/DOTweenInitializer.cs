using UnityEngine;
using DG.Tweening;

public class DOTweenInitializer : MonoBehaviour
{
    void Awake()
    {
        // Initialize DOTween with custom settings
        DOTween.Init(
            recycleAllByDefault: true,  // Better performance
            useSafeMode: false,         // Faster but less error checking
            logBehaviour: LogBehaviour.ErrorsOnly
        );
        
        // Set default ease for all tweens (optional)
        DOTween.defaultEaseType = Ease.OutQuad;
        
        Debug.Log("DOTween initialized!");
    }
}