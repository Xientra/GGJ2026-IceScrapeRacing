using System;
using DG.Tweening;
using EPOOutline;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{

    [Serializable]
    public class IntroElement
    {
        public RectTransform element;
        public RectTransform FadeInAnchor;
        public RectTransform FadeOutAnchor;
        public CanvasGroup canvasGroup;
        public float showTime;
    }
    
    
    public static IntroCutscene Instance { get; private set; }
    
    [Header("Intro References")]
    public IntroElement introText1;
    public IntroElement introText2;
    public IntroElement introText3;
    public Outlinable ignitionOutline;
    public CanvasGroup arrow;
    
    
    // public RectTransform introText;
    // public RectTransform introAnchor;
    // public RectTransform introAnchorEnd;
    // public CanvasGroup arrowAndButton;
    // public RectTransform timeStuff;
    // public RectTransform timeStuffAnchor;

    [Header("Tutorial References")] 
    public IntroElement tutorialText1;
    public IntroElement tutorialText2;
    public IntroElement tutorialText3;

    private bool _isIntroFinished = false;
    
    private Sequence introSequence;
    private Sequence tutorialSequence;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        StartIntro();
    }

    [ContextMenu("StartIntro")]
    public void StartIntro()
    {
        arrow.alpha = 0;
        
        introSequence = DOTween.Sequence();
        
        ShowText(introSequence, introText1);
        ShowText(introSequence, introText2);
        ShowText(introSequence, introText3);

        introSequence.onComplete = () =>
        {
            arrow.DOFade(1, 0.5f);
            ignitionOutline.enabled = true;
        };
    }

    private void ShowText(Sequence seq, IntroElement element)
    {
        seq.Append(
            element.element.DOAnchorPos(element.FadeInAnchor.anchoredPosition, 0.5f)
                .SetEase(Ease.OutBack)
        );
        seq.AppendInterval(element.showTime);
        
        seq.Append(
            element.element.DOAnchorPos(element.FadeOutAnchor.anchoredPosition, 0.5f)
                .SetEase(Ease.OutBack)
        );
    }
    
    
    [ContextMenu("EndIntro")]
    public void EndIntro()
    {
        if(_isIntroFinished) return;

        arrow.DOFade(0, 0.5f);
        Tutorial();

        _isIntroFinished = true;
    }

    public void KillAnimation()
    {
        introSequence.Kill(true);
    }
    

    [ContextMenu("STartTutorial")]
    public void Tutorial()
    {
        tutorialSequence = DOTween.Sequence();

        tutorialSequence.Append(tutorialText1.element.DOMove(tutorialText1.FadeInAnchor.position, 0.5f).SetEase(Ease.OutBack));
        ShowText(tutorialSequence, tutorialText2);
        ShowText(tutorialSequence, tutorialText3);
    }
}