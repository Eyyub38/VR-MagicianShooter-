using UnityEngine;
using UnityEngine.InputSystem;
using System;

public enum GameStates { Menu, Playing }
public class GameManager : MonoBehaviour
{
    [SerializeField] ScoreBoard scoreBoard;
    public static GameManager i { get; private set; }
    public GameStates CurrentGameState { get; set; } = GameStates.Menu;

    public static event Action<GameStates> OnStateChanged;

    void Awake()
    {
        if (i != null && i != this)
        {
            Debug.LogWarning("Multiple GameManager instances detected, destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        i = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RestartGame()
    {
        var player = GameObject.FindFirstObjectByType<PlayerController>();
        player.PlayerHealth = player.MaxHealth;
        player.HealthBar.UpdateHealthBar(player.PlayerHealth, player.MaxHealth);

        var spawner = GameObject.FindFirstObjectByType<EnemySpawner>();
        spawner.CleanEnemies();
        spawner.EnemiesSpawned = 0;
        spawner.IsAnyEnemyAlive();

        scoreBoard.ResetScore();
        CurrentGameState = GameStates.Playing;
        OnStateChanged?.Invoke(CurrentGameState);
    }
}
