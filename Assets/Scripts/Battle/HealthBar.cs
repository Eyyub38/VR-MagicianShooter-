using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image healthBar;

    float healthbarLength;

    void Awake()
    {
        healthbarLength = healthBar.rectTransform.sizeDelta.x;
    }

    public void UpdateHealthBar(float health, float maxHealth)
    {
        float healthRation = health / maxHealth;
        healthRation = Mathf.Clamp(healthRation, 0f, 1f);
        healthBar.rectTransform.sizeDelta = new Vector2(healthbarLength * healthRation, healthBar.rectTransform.sizeDelta.y);
    }
}
