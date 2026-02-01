using DG.Tweening;
using UnityEngine;

public class Scraping : MonoBehaviour
{
    [SerializeField] private Transform handPivo;
    [SerializeField] private LayerMask windowLayer;
    [SerializeField] private float noramlOffset;
    [SerializeField] private float mouseSensivityMuliplayer = 1;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform noramleCameraPos;
    [SerializeField] private Transform scrapingCameraPos;
    [SerializeField] private float cameraAnimatonDuration = 1;
    [SerializeField] private Ease cameraAnimatonEase;
    private InputSystem_Actions _input;
    private Camera _camera;

    public Transform cameraParent;


    [SerializeField]
    private ParticleSystem scrapeParticles;
    
    void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();

        _camera = Camera.main;
        cameraParent.position = noramleCameraPos.position;

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = false;
    }

    void Update()
    {
        bool scraping = _input.Player.Scrape.IsPressed();

        if (scraping)
        {
            Vector2 mousePos = _input.Player.MousePosition.ReadValue<Vector2>();
            mousePos.x *= mouseSensivityMuliplayer;
            mousePos.y *= mouseSensivityMuliplayer;

            var hitPosNorm = GetWindowWorldPos(mousePos);

            handPivo.position = hitPosNorm.Item1 + hitPosNorm.Item2 * noramlOffset;
            leftHand.gameObject.SetActive(false);
        }
        else
        {
            handPivo.position = Vector3.zero;
            leftHand.gameObject.SetActive(true);
        }


        bool wasPressedThisFrame = _input.Player.Scrape.WasPressedThisFrame();
        if (wasPressedThisFrame)
        {
            
            cameraParent.DOKill();
            cameraParent.DOLocalMove(scrapingCameraPos.localPosition, cameraAnimatonDuration).SetAutoKill(true)
                .SetEase(cameraAnimatonEase);
            cameraParent.DOLocalRotate(scrapingCameraPos.localRotation.eulerAngles, cameraAnimatonDuration).SetAutoKill(true)
                .SetEase(cameraAnimatonEase);
            
            //scrapeParticles.Play();
        }

        bool wasRealeasedThisFrame = _input.Player.Scrape.WasReleasedThisFrame();
        if (wasRealeasedThisFrame)
        {
            cameraParent.DOKill();
            cameraParent.DOLocalMove(noramleCameraPos.localPosition, cameraAnimatonDuration).SetAutoKill(true)
                .SetEase(cameraAnimatonEase);
            cameraParent.DOLocalRotate(noramleCameraPos.localRotation.eulerAngles, cameraAnimatonDuration).SetAutoKill(true)
                .SetEase(cameraAnimatonEase);
            
            //scrapeParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private (Vector3, Vector3) GetWindowWorldPos(Vector2 mousePos)
    {
        var ray = _camera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out RaycastHit hit, 50, windowLayer))
        {
            Debug.DrawRay(ray.origin, ray.direction * 12,
                Color.blue);
            return (hit.point, ray.direction.normalized);
        }

        return (Vector3.zero, Vector2.zero);
    }
}