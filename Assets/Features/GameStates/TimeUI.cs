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
        bool even = (int)(Time.time % 2) == 0;
        timeLabel.text = GameManager.Instance.CurrentTime.ToString(even ? @"hh\:mm" : @"hh\ mm");
    }
}
