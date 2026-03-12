using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    [SerializeField] Image healthBar;

    float healthbarLength;

    void Awake() {
        healthbarLength = healthBar.rectTransform.sizeDelta.x;
    }

    public void UpdateHealthBar(float health, float maxHealth) {
        float healthRation = health / maxHealth;
        healthRation = Mathf.Clamp( healthRation, 0f, 1f );
        healthBar.rectTransform.sizeDelta = new Vector2( healthbarLength * healthRation, healthBar.rectTransform.sizeDelta.y );
    }

    void LateUpdate() {
        if(transform.parent != null && XRSettings.isDeviceActive) {
            transform.LookAt(transform.parent.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
