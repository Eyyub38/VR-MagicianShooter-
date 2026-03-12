using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {
    [SerializeField] GameObject menu;
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;

    [SerializeField] InputReader inputReader;

    void Start() {
        if(GameManager.i != null) {
            GameManager.i.CurrentGameState = GameStates.Menu;
            GameManager.OnStateChanged += OnGameStatesChanged;
        }

        startButton.onClick.AddListener( HandleStartGame );
        quitButton.onClick.AddListener( HandleQuitGame );

        ShowMenu();
    }

    void HandleStartGame() {
        if(GameManager.i != null) {
            GameManager.i.CurrentGameState = GameStates.Playing;
            GameManager.i.RestartGame();
        }
    }

    void HandleQuitGame() {
        Application.Quit();
    }

    void OnGameStatesChanged(GameStates newState) {
        if(newState == GameStates.Menu) ShowMenu();
        else HideMenu();
    }

    public void ShowMenu() => menu.SetActive( true );
    public void HideMenu() => menu.SetActive( false );

    void Oestroy() {
        GameManager.OnStateChanged -= OnGameStatesChanged;
    }
}
