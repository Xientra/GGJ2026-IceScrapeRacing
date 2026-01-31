using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField]
    public bool GameIsRunning { get; private set; } = false;

    [SerializeField]
    private float timeInSeconds = 60 * 3;

    private float _remainingTime;

    public float RemainingTime => _remainingTime;
    public TimeSpan RemainingTimeSpan => TimeSpan.FromSeconds(_remainingTime);
    public DateTime CurrentTime
    {
        get
        {
            float inUniversePassedSeconds = (1 - (_remainingTime / timeInSeconds)) * inUniverseTimeInSeconds;
            return new DateTime(2026, 2, 2, startHour, startMinutes, 0) + TimeSpan.FromSeconds(inUniversePassedSeconds);
        }
    }

    public event EventHandler<bool> GameOver;

    [Header("9 to 5")]
    [SerializeField]
    private float inUniverseTimeInSeconds = 60 * 30f;

    [SerializeField]
    private int startHour = 8;

    [SerializeField]
    private int startMinutes = 30;
    
    private void Awake()
    {
        Instance = this;
    }
    

    private void Update()
    {
        if (!GameIsRunning) return;
        
        _remainingTime -= Time.deltaTime;

        if (_remainingTime <= 0)
            EndGame(false);
    }

    public void StartGame()
    {
        GameIsRunning = true;
        _remainingTime = timeInSeconds;
    }

    private void EndGame(bool playerWon)
    {
        _remainingTime = 0;
        GameIsRunning = false;
        GameOver?.Invoke(this, playerWon);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EndGame(true);
        }
    }
}