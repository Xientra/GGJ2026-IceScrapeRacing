using DG.Tweening;
using Features.BrumBrum;
using UnityEngine;
using UnityEngine.Events;

public class Ignishen : MonoBehaviour, IInteracttable
{
    [SerializeField] private Transform key;
    [SerializeField] private Transform keyHole;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration;

    public float minTime = 2f;
    public float maxTime = 5f;
    private float randomTime;

    private bool isHolding = false;
    private float holdTimer = 0f;

    private void Start()
    {
        randomTime = Random.Range(minTime, maxTime);
    }

    public void OnInteract()
    {
        isHolding = true;
        holdTimer = 0f;

        key.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, -90), duration).SetAutoKill(true).SetEase(ease);
        keyHole.DOLocalRotateQuaternion(Quaternion.Euler(-180, 0, 90), duration).SetAutoKill(true).SetEase(ease);
        //CarCameraShake.Instance.StartLoopShake(0.01f);
    }

    public void OnEndInteract()
    {
        isHolding = false;
        holdTimer = 0f;

        key.DOLocalRotateQuaternion(Quaternion.Euler(0, 0, 0), 0.5f).SetAutoKill(true).SetEase(ease);
        keyHole.DOLocalRotateQuaternion(Quaternion.Euler(-180, 0, 0), 0.5f).SetAutoKill(true).SetEase(ease);
    }

    public void OnHover()
    {
    }


    private void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;

            if (holdTimer >= randomTime)
            {
                isHolding = false;
                holdTimer = 0f;
                randomTime = Random.Range(minTime, maxTime);
                OnIgnitionStarted();
                OnEndInteract();
            }
        }
    }

    private void OnIgnitionStarted()
    {
        IntroCutscene.Instance.EndIntro();
        CarController.Instance.OnToggleEngine(true);
        Debug.Log("Ignition started");
        //CarCameraShake.Instance.StartLoopShake(0.005f);
    }
}