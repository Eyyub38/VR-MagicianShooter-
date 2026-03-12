using UnityEngine;

public class CameraController : MonoBehaviour {
    [SerializeField] InputReader inputReader;
    [SerializeField] float sensitivity = 0.1f;
    [Header( "Limits" )]
    [SerializeField] float xLimit = 30f;
    [SerializeField] float yLimit = 60f;

    float rotationX, rotationY;

    void HandleLook(Vector2 delta) {
        if(UnityEngine.XR.XRSettings.isDeviceActive) return;

        rotationX += delta.x * sensitivity;
        rotationY += delta.y * sensitivity;

        rotationX = Mathf.Clamp( rotationX, -xLimit, xLimit );
        rotationY = Mathf.Clamp( rotationY, -yLimit, yLimit );

        transform.localRotation = Quaternion.Euler( rotationX, rotationY, 0f );
    }
}