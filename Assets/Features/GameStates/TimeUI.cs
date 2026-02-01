using DG.Tweening;
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

    public void SwitchOnOff(bool on)
    {
        if (on == false)
            timeLabel.alpha = 0.0f;

        if (on)
            DOVirtual.Float(0.0f, 1.0f, 0.7f, f => timeLabel.alpha = f);
    }
}
