using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class KeyboardController : MonoBehaviour {
    [SerializeField] GameObject look;
    [SerializeField] InputAction lookAction;
    [SerializeField] float lookSensitivity = 100f;

    float xRotation = 0f;
    float yRotation = 0f;

    public static KeyboardController i { get; set; }

    bool IsVRActive() {
        var xrDisplay = ArrayUtility.FindAll( new UnityEngine.XR.XRDisplaySubsystem[0], s => s.running );
        return XRSettings.isDeviceActive || (xrDisplay != null && xrDisplay.Count > 0);
    }

    void Awake() {
        i = this;
    }

    void Start() {
        if(IsVRActive()) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update() {
        if(IsVRActive()) {
            return;
        }

        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        float lookX = lookInput.x * lookSensitivity * Time.deltaTime;
        float lookY = lookInput.y * lookSensitivity * Time.deltaTime;

        xRotation -= lookY;
        yRotation += lookX;

        xRotation = Mathf.Clamp( xRotation, -30f, 30f );
        yRotation = Mathf.Clamp( yRotation, -60f, 60f );

        look.transform.localEulerAngles = new Vector3( xRotation, yRotation, 0f );
    }

    public void OnEnable() {
        lookAction?.Enable();
    }

    public void OnDisable() {
        lookAction?.Disable();
    }
}