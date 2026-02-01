using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class MainMenueButton : MonoBehaviour, IInteracttable
{
    [SerializeField] private Transform hightLightTra;
    [SerializeField] private UnityEvent OnInteractEvent;


    private void Start()
    {
        hightLightTra.gameObject.SetActive(false);
    }

    public void OnInteract()
    {
        OnInteractEvent.Invoke();
    }

    public void OnEndInteract()
    {
    }

    public void OnHover()
    {
        hightLightTra.gameObject.SetActive(true);
    }

    public void EndOnHover()
    {
        hightLightTra.gameObject.SetActive(false);
    }
}