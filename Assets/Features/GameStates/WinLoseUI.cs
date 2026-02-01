using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseUI : MonoBehaviour
{
    public CanvasGroup winCanvasGroup;
    public CanvasGroup loseCanvasGroup;

    public RectTransform winText;
    public RectTransform loseText;
    
    private void Start()
    {
        GameManager.Instance.GameOver += OnGameOver;
        winCanvasGroup.gameObject.SetActive(false);
        winCanvasGroup.alpha = 0;
        loseCanvasGroup.gameObject.SetActive(false);
        loseCanvasGroup.alpha = 0;
        
        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
    }

    [ContextMenu("GameWon")]
    public void GameWon()
    {
        OnGameOver(null, true);
    }
    
    [ContextMenu("GameLost")]
    public void GameLost()
    {
        OnGameOver(null, false);
    }
    
    private void OnGameOver(object sender, bool playerWon)
    {
        if (playerWon)
        {
            winCanvasGroup.gameObject.SetActive(true);
            winCanvasGroup.DOFade(1f, 0.5f);
        }
        else
        {
            loseCanvasGroup.gameObject.SetActive(true);
            loseCanvasGroup.DOFade(1f, 0.5f);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DisableUI()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(true);
        
        
        if (winCanvasGroup.alpha != 0)
        {
            winCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                winCanvasGroup.gameObject.SetActive(false);
            });

        }

        if (loseCanvasGroup.alpha != 0)
        {
            loseCanvasGroup.DOFade(0, 0.5f).OnComplete(() =>
            {
                loseCanvasGroup.gameObject.SetActive(false);
            });
        }
    }
}
