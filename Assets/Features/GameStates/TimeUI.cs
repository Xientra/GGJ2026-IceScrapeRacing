using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timeLabel;
    
    private void Start()
    {
        
    }

    private void LateUpdate()
    {
        timeLabel.text = GameManager.Instance.RemainingTimeSpan.ToString(@"mm\:ss");
    }
}
