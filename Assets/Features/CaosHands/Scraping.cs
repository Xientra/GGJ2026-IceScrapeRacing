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


    void Start()
    {
        _input = new InputSystem_Actions();
        _input.Enable();

        _camera = Camera.main;
        _camera.transform.position = noramleCameraPos.position;
        
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
        if(wasPressedThisFrame)
        {
            _camera.transform.DOKill();
            _camera.transform.DOMove(scrapingCameraPos.position, cameraAnimatonDuration).SetAutoKill(true).SetEase(cameraAnimatonEase);
            _camera.transform.DORotate(scrapingCameraPos.rotation.eulerAngles, cameraAnimatonDuration).SetAutoKill(true).SetEase(cameraAnimatonEase);
        }

        bool wasRealeasedThisFrame = _input.Player.Scrape.WasReleasedThisFrame();
        if (wasRealeasedThisFrame)
        {
            _camera.transform.DOKill();
            _camera.transform.DOMove(noramleCameraPos.position, cameraAnimatonDuration).SetAutoKill(true).SetEase(cameraAnimatonEase);
            _camera.transform.DORotate(noramleCameraPos.rotation.eulerAngles, cameraAnimatonDuration).SetAutoKill(true).SetEase(cameraAnimatonEase);
        }
    }

    private (Vector3, Vector3) GetWindowWorldPos(Vector2 mousePos)
    {
        if (Physics.Raycast(_camera.ScreenPointToRay(mousePos), out RaycastHit hit, 50, windowLayer))
        {
            Debug.DrawRay(_camera.ScreenPointToRay(mousePos).origin, _camera.ScreenPointToRay(mousePos).direction * 12,
                Color.blue);
            return (hit.point, -hit.normal);
        }

        return (Vector3.zero, Vector2.zero);
    }
}