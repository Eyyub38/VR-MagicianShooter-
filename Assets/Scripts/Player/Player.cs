using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    [SerializeField] float maxHealth = 10f;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject menuUI;


    [SerializeField] HealthBar healthBar;

    float playerHealth;

    public float PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }
    public HealthBar HealthBar => healthBar;
    public float MaxHealth => maxHealth;


    void Start() {
        playerHealth = maxHealth;
        healthBar.UpdateHealthBar( playerHealth, maxHealth );
    }

    void Update() {
        if(GameManager.i.CurrentGameState != GameStates.Playing) {
            SetUI( false );
            menuUI.SetActive( true );
            
            KeyboardController.i.OnDisable();
            return;
        }

        KeyboardController.i.OnEnable();
        SetUI( true );
    }

    void SetUI(bool playing) {
        gameUI?.SetActive( playing );
        menuUI?.SetActive( !playing );
    }

    public void Die() {
        gameUI.SetActive( false );
        menuUI?.SetActive( true );
        GameManager.i.CurrentGameState = GameStates.Menu;

        var mm = FindFirstObjectByType<MenuManager>();
        if(mm != null)
            mm.ShowMenu();
    }
}
