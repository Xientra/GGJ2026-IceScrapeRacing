using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MainMenue : MonoBehaviour
{
    [SerializeField] private Transform mainMenuCamTra;
    [SerializeField] private GameObject HandL;
    [SerializeField] private GameObject HandR;
    [SerializeField] private DOTweenPath dotPath;
    [SerializeField] private float duration = 3;
    private Camera _camera;

    [SerializeField] private UnityEvent OnActivateNormalGame;

    void Start()
    {
        _camera = Camera.main;

        _camera.transform.position = mainMenuCamTra.position;
        _camera.transform.rotation = mainMenuCamTra.rotation;

        HandL.SetActive(false);
        HandR.SetActive(false);
    }

    public void StartPressed()
    {
        _camera.transform.DOPath(dotPath.GetDrawPoints(), duration, PathType.CatmullRom)
            .SetOptions(AxisConstraint.None, AxisConstraint.X)
            .SetEase(Ease.OutExpo).Play()
            .onComplete += ActivateNormalGame;
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void ActivateNormalGame()
    {
        HandL.SetActive(true);
        HandR.SetActive(true);
        OnActivateNormalGame.Invoke();
        
        gameObject.SetActive(false);
    }
}