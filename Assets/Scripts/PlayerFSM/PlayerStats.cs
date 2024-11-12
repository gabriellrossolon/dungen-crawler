using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Attributes")]
    public float maxHp;
    public float currentHp;
    public float hpRegenRate;
    private float hpRegenTimer = 0f;
    public bool canHpRegen;

    public float maxStamina;
    public float currentStamina;
    public float staminaRegenRate;
    private float staminaRegenTimer = 0f;
    public bool canStaminaRegen;
    [HideInInspector] public float staminaDrainCooldown = 0f;

    private const float regenInterval = 1f;

    private void Start()
    {
        currentHp = maxHp;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        ShowValueInHud();
        AttributesControll();
        AttributesRegen();
    }

    private void AttributesControll()
    {
        if (currentHp <= 0) { currentHp = 0; }
        if (currentStamina <= 0) { currentStamina = 0; }
    }

    private void AttributesRegen()
    {
        if (currentHp < 100f && canHpRegen)
        {
            hpRegenTimer += Time.deltaTime;
            if (hpRegenTimer >= regenInterval / hpRegenRate)
            {
                currentHp = Mathf.Min(currentHp + 1f, 100f);
                hpRegenTimer = 0f;
            }
        }

        if (currentStamina < 100f && canStaminaRegen)
        {
            staminaRegenTimer += Time.deltaTime;
            if (staminaRegenTimer >= regenInterval / staminaRegenRate)
            {
                currentStamina = Mathf.Min(currentStamina + 1f, 100f);
                staminaRegenTimer = 0f;
            }
        }
    }

    private void ShowValueInHud()
    {
        HudManager.Instance.hpSlider.maxValue = maxHp;
        HudManager.Instance.hpSlider.value = currentHp;
        HudManager.Instance.hpText.text = $"{currentHp}/{maxHp}";
        HudManager.Instance.staminaSlider.maxValue = maxStamina;
        HudManager.Instance.staminaSlider.value = currentStamina;
        HudManager.Instance.staminaText.text = $"{currentStamina}/{maxStamina}";
    }

    public void DoDamage(float damageValue)
    {
        currentHp -= damageValue;
    }
}
