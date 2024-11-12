using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; private set; }

    [Header("Debug")]
    public Text stateText;
    public Text fps;

    [Header("Attributes")]
    public Slider hpSlider;
    public Text hpText;
    public Slider staminaSlider;
    public Text staminaText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
