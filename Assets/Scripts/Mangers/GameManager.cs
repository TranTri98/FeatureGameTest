using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnKeyCollected;
    public static event Action OnGameWon;
    public static event Action OnGameLost;

    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip dieSound;

    public enum GameState { Playing, Won, Lost }
    public GameState State { get; set; } = GameState.Playing;

    private void Awake()
    {
        // Singleton pattern for quick access
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void NotifyKeyCollected()
    {
        if (State != GameState.Playing) return;
        OnKeyCollected?.Invoke();
    }

    public void NotifyPlayerDied()
    {
        if (State != GameState.Playing) return;
        State = GameState.Lost;
        OnGameLost?.Invoke();
        SoundManager.Instance.PlaySFX(dieSound, 0.5f);
        Time.timeScale = 0f; // pause game
    }

    public void NotifyPlayerWon()
    {
        if (State != GameState.Playing) return;
        State = GameState.Won;
        OnGameWon?.Invoke();
        SoundManager.Instance.PlaySFX(winSound);
        Time.timeScale = 0f; // pause game
    }
}
