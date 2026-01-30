using TMPro;
using UnityEngine;

public class WinLoseUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text winText;
    [SerializeField]
    private TMP_Text loseText;
    
    private void Start()
    {
        GameManager.Instance.GameOver += OnGameOver; 
    }

    private void OnGameOver(object sender, bool playerWon)
    {
        winText.gameObject.SetActive(playerWon);
        loseText.gameObject.SetActive(!playerWon);
    }
}
