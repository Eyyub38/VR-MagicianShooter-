using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject startText;
    [SerializeField] GameObject quitText;

    [SerializeField] InputAction enterAction;
    [SerializeField] InputAction changeAction;

    [SerializeField] List<GameObject> menuTexts;
    [SerializeField] Color selectedColor;
    Color originalColor = Color.white;

    TMPro.TMP_Text startTMP;
    TMPro.TMP_Text quitTMP;

    int selectedIndex = 0;
    float changeCooldown = 0.2f;
    float lastChangeTime;

    void Start()
    {
        if (menu == null || startText == null || quitText == null || menuTexts == null || menuTexts.Count == 0)
        {
            Debug.LogError("MenuManager: missing serialized references, disabling component");
            enabled = false;
            return;
        }

        startTMP = startText.GetComponent<TMPro.TMP_Text>();
        quitTMP = quitText.GetComponent<TMPro.TMP_Text>();
        if (startTMP == null || quitTMP == null)
        {
            Debug.LogError("MenuManager: startText/quitText lacks a TMP_Text component, disabling");
            enabled = false;
            return;
        }

        menu.SetActive(true);
        if (GameManager.i != null)
        {
            GameManager.i.CurrentGameState = GameStates.Menu;
            GameManager.OnStateChanged += OnGameStateChanged;
        }

        SetSelections();
    }

    void Update()
    {
        if (enterAction.triggered)
        {
            switch (selectedIndex)
            {
                case 0:
                    startTMP.color = selectedColor;
                    if (GameManager.i != null)
                        GameManager.i.CurrentGameState = GameStates.Playing;
                    GameManager.i.RestartGame();
                    break;
                case 1:
                    quitTMP.color = selectedColor;
                    Application.Quit();
                    break;
            }
        }
    }

    void SetSelections()
    {
        for (int i = 0; i < menuTexts.Count; i++)
        {
            var tmp = menuTexts[i]?.GetComponent<TMPro.TMP_Text>();
            if (tmp == null)
            {
                Debug.LogWarning($"MenuManager: menuTexts[{i}] has no TMP_Text component");
                continue;
            }

            if (i == selectedIndex)
                tmp.color = selectedColor;
            else
                tmp.color = originalColor;
        }
    }

    void OnEnable()
    {
        enterAction.Enable();
        changeAction.Enable();
        changeAction.performed += OnChange;
    }

    void OnDisable()
    {
        enterAction.Disable();
        changeAction.Disable();
        changeAction.performed -= OnChange;

        GameManager.OnStateChanged -= OnGameStateChanged;
    }

    void OnChange(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        if (Time.time - lastChangeTime < changeCooldown) return;

        float value = ctx.ReadValue<float>();
        if (value > 0.5f)
            selectedIndex++;
        else if (value < -0.5f)
            selectedIndex--;

        if (selectedIndex >= menuTexts.Count)
            selectedIndex = 0;
        if (selectedIndex < 0)
            selectedIndex = menuTexts.Count - 1;

        SetSelections();
        lastChangeTime = Time.time;
    }

    void OnGameStateChanged(GameStates newState)
    {
        if (newState == GameStates.Menu)
            ShowMenu();
        else
            HideMenu();
    }

    public void ShowMenu()
    {
        if (menu != null)
            menu.SetActive(true);
    }

    public void HideMenu()
    {
        if (menu != null)
            menu.SetActive(false);
    }
}
