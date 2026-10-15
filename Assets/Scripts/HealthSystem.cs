using System;
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event Action<int> MaxHealthChanged;
    public event Action<int> CurrentHealthChanged;
    public event Action HealthDepleted;

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool invincibility = false;

    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            int clampedValue;
            if (value <= 0) clampedValue = 1; else clampedValue = value;

            if (clampedValue != maxHealth)
            {
                maxHealth = value;
                MaxHealthChanged?.Invoke(clampedValue - maxHealth);
                if (currentHealth > maxHealth) currentHealth = maxHealth;
            }
        }
    }
    public int CurrentHealth
    {
        get => currentHealth;
        set
        {
            if (value < currentHealth && invincibility) return;

            int clampedValue = Mathf.Clamp(value, 0, maxHealth);
            if (clampedValue != currentHealth)
            {
                currentHealth = value;
                CurrentHealthChanged?.Invoke(clampedValue - currentHealth);
                if (currentHealth == 0) HealthDepleted?.Invoke();
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public IEnumerator SetTempInvincibility(float invTime)
    {
        invincibility = true;
        yield return new WaitForSeconds(invTime);
        invincibility = false;
    }
}
