using UnityEngine;
using TMPro;


public class MovesLeftText : MonoBehaviour
{
    [SerializeField] private TMP_Text movesLeftText;

    
    public void SetMovesLeftText(int movesLeft)
    {
        this.movesLeftText.text = movesLeft.ToString();
    }
}