using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public TextMeshProUGUI HealthPercent;
    public TextMeshProUGUI StaminaPercent;
    public Slider healthBar = default;
    public Slider staminaBar = default;

    private void OnEnable()
    {
        FirstPersonController.OnDamage += UpdateHealth;
        FirstPersonController.OnHeal += UpdateHealth;
        FirstPersonController.OnStaminaChange += UpdateStamina;
    }
    private void OnDisable()
    {
        FirstPersonController.OnDamage -= UpdateHealth;
        FirstPersonController.OnHeal -= UpdateHealth;
        FirstPersonController.OnStaminaChange -= UpdateStamina;
    }
    private void UpdateHealth(float currentHealth)
    {
        healthBar.value = currentHealth;
        HealthPercent.text = (int)currentHealth+"%";
    }

    private void UpdateStamina(float currentStamina)
    {
        staminaBar.value = currentStamina;
        StaminaPercent.text = (int)currentStamina+"%";
    }
}
