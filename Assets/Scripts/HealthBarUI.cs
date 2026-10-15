using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private HealthSystem health;
    [SerializeField] private RectTransform fill;
    [SerializeField] private float scale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (health != null)
        {
            health.CurrentHealthChanged += diff => 
                fill.sizeDelta = new Vector2(health.CurrentHealth * scale / health.MaxHealth, fill.sizeDelta.y);         
        }
        else Debug.Log("No health system found");
    }
}