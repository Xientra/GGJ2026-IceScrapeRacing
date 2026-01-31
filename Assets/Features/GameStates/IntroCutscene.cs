using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    [Header("GameManager")] 
    public GameManager GameManager;
    [Header("Intro References")]
    public RectTransform introText;
    public RectTransform introAnchor;
    public RectTransform introAnchorEnd;
    public CanvasGroup arrowAndButton;
    public RectTransform timeStuff;
    public RectTransform timeStuffAnchor;

    [Header("Tutorial References")] 
    public RectTransform scrapingTutorial;
    private Vector3 scrapingTutorialPosition;
    public RectTransform scrapingTutorialAnchor;
    public RectTransform DrivingTutorial;
    private Vector3 DrivingTutorialPosition;
    public RectTransform DrivingTutorialAnchor;

    private void Start()
    {
        Invoke(nameof(StartIntro), 2f);
        scrapingTutorialPosition = scrapingTutorial.position;
        DrivingTutorialPosition = DrivingTutorial.position;
    }

    [ContextMenu("StartIntro")]
    public void StartIntro()
    {
        arrowAndButton.alpha = 0;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            introText.DOMove(introAnchor.position, 1f)
                .SetEase(Ease.OutBack)
        );

        //seq.AppendInterval(3f);

        seq.Append(
            arrowAndButton.DOFade(1f, 1f)
        );
    }

    [ContextMenu("EndIntro")]
    public void EndIntro()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(
            introText.DOMove(introAnchorEnd.position, 0.5f)
                .SetEase(Ease.OutBack)
        );

        seq.Append(
            arrowAndButton.DOFade(0f, 0.5f)
        );

        seq.Append(
            timeStuff.DOMove(timeStuffAnchor.position, 0.5f).SetEase(Ease.OutBack)
        );

        seq.onComplete = () =>
        {
            GameManager.StartGame();
            Tutorial();
        };
    }

    public void Tutorial()
    {
        Sequence seq = DOTween.Sequence();
        
        seq.AppendInterval(1f);

        seq.Append(scrapingTutorial.DOMove(scrapingTutorialAnchor.position, 0.5f).SetEase(Ease.OutBack));
        seq.AppendInterval(8f);
        seq.Append(scrapingTutorial.DOMove(scrapingTutorialPosition, 0.5f).SetEase(Ease.OutQuad));
        
        seq.Append(DrivingTutorial.DOMove(DrivingTutorialAnchor.position, 0.5f).SetEase(Ease.OutBack));        
        seq.AppendInterval(5f);
        seq.Append(DrivingTutorial.DOMove(DrivingTutorialPosition, 0.5f).SetEase(Ease.OutQuad));
    }
}