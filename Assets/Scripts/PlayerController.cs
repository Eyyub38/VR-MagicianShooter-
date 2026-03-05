using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject look;
    [SerializeField] InputAction lookAction;
    [SerializeField] InputAction moveAction;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float lookSensitivity = 100f;
    [SerializeField] float maxHealth = 10f;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject menuUI;

    [SerializeField] Image healthBar;

    float healthbarLength;
    float playerHealth;
    float xRotation = 0f;
    float yRotation = 0f;

    public float PlayerHealth { get { return playerHealth; } set { playerHealth = value; } }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (healthBar != null)
            healthbarLength = healthBar.rectTransform.sizeDelta.x;
        playerHealth = maxHealth;
        UpdateHealthBar();
    }

    void Update()
    {
        if (GameManager.i == null)
        {
            SetUI(false);
            OnDisable();
            return;
        }

        if (GameManager.i.CurrentGameState != GameStates.Playing)
        {
            SetUI(false);
            menuUI.SetActive(true);
            OnDisable();
            return;
        }

        OnEnable();
        SetUI(true);

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        Vector2 moveInput = Vector2.zero;
        if (moveAction != null)
            moveInput = moveAction.ReadValue<Vector2>();

        Vector3 movement = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        float lookX = lookInput.x * lookSensitivity * Time.deltaTime;
        float lookY = lookInput.y * lookSensitivity * Time.deltaTime;

        xRotation -= lookY;
        yRotation += lookX;

        xRotation = Mathf.Clamp(xRotation, -30f, 30f);
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

        look.transform.localEulerAngles = new Vector3(xRotation, yRotation, 0f);
    }

    void OnEnable()
    {
        lookAction?.Enable();
        moveAction?.Enable();
    }

    void OnDisable()
    {
        lookAction?.Disable();
        moveAction?.Disable();
    }

    public void UpdateHealthBar()
    {
        float healthRation = playerHealth / maxHealth;
        healthRation = Mathf.Clamp(healthRation, 0f, 1f);
        healthBar.rectTransform.sizeDelta = new Vector2(healthbarLength * healthRation, healthBar.rectTransform.sizeDelta.y);
    }

    void SetUI(bool playing)
    {
        gameUI?.SetActive(playing);
        menuUI?.SetActive(!playing);
    }

    public void Die()
    {
        gameUI.SetActive(false);
        menuUI?.SetActive(true);
        GameManager.i.CurrentGameState = GameStates.Menu;

        var mm = FindFirstObjectByType<MenuManager>();
        if (mm != null)
            mm.ShowMenu();
    }
}
