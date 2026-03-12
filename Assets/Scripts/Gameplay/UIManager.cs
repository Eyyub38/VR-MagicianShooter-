using UnityEngine;
using UnityEngine.XR;

public class UIManager : MonoBehaviour {
    [SerializeField] Canvas mainCanvas;
    [SerializeField] GameObject crosshair;
    [SerializeField] ScoreBoard scoreBoard;
    [SerializeField] float vrUIDistance = 2f;
    public static UIManager i;

    void Awake() {
        i = this;
        SetupPlatformUI();
    }

    void SetupPlatformUI() {
        if(XRSettings.isDeviceActive) {
            mainCanvas.renderMode = RenderMode.WorldSpace;
            mainCanvas.transform.localScale = Vector3.one * 0.001f;

            if(crosshair) crosshair.SetActive( false );
        } else {
            mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
    }

    void Update() {
        if(XRSettings.isDeviceActive && mainCanvas.renderMode == RenderMode.WorldSpace) {
            Vector3 targeyPos = Camera.main.transform.position + Camera.main.transform.forward * vrUIDistance;
            mainCanvas.transform.position = Vector3.Lerp( mainCanvas.transform.position, targeyPos, Time.deltaTime * 5f );
            mainCanvas.transform.LookAt( Camera.main.transform );
            mainCanvas.transform.Rotate( 0, 180, 0 );
        }
    }
}