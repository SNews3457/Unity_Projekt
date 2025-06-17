using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ModeSwitcher : MonoBehaviour
{
    [Header("References")]
    private CharacterController2D Player;
    private SpriteRenderer sp;
    private PlayerMovement Movement;
    private Attack attack;

    [Header("Light Parameters")]
    [SerializeField] private float LigthSpeed = 50;
    [SerializeField] private float ligthDamage = 3;
    private float AttackCooldownLigth = 0.12f;

    [Header("Dark Parameters")]
    [SerializeField] private float ShadowSpeed = 40;
    [SerializeField] private float DarkDamage = 6;
    private float AttackCooldownDark = 0.365f;

    public enum PlayerMode { Light, Dark }
    public PlayerMode currentMode;

    [Header("Dark Meter Settings")]
    [SerializeField] private float darkMeter = 0f;
    [SerializeField] private float maxDarkMeter = 100f;
    [SerializeField] private float darkMeterFillRate = 0.3f;    // base rate per second
    [SerializeField] private float darkMeterDrainRate = 5f;     // per second in Light mode

    [Header("Corruption Penalty")]
    private int corruptionStage = 0;
    [SerializeField] private float corruptionPenaltyFactor = 0.15f; // each stage reduces fill rate by 15%
    private bool wasAbove80 = false;

    [Header("UI")]
    [SerializeField] private Slider darkMeterSlider;

    [Header("Vignette Settings")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private float vignetteStartThreshold = 80f;
    [SerializeField] private float maxVignetteIntensity = 0.8f;

    private Vignette vignette;
    private bool meterFullTriggered = false;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            vignette.intensity.value = 0f;
        }

        Player = GetComponent<CharacterController2D>();
        sp = GetComponent<SpriteRenderer>();
        Movement = GetComponent<PlayerMovement>();
        attack = GetComponent<Attack>();

        currentMode = PlayerMode.Light;
        UpdateMode();

        if (darkMeterSlider != null)
        {
            darkMeterSlider.minValue = 0;
            darkMeterSlider.maxValue = maxDarkMeter;
            darkMeterSlider.value = darkMeter;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Switch();
        }

        HandleDarkMeter();
    }

    public void Switch()
    {
        currentMode = currentMode == PlayerMode.Light ? PlayerMode.Dark : PlayerMode.Light;
        Debug.ClearDeveloperConsole();
        Debug.Log("Switched to " + currentMode + " mode");
        UpdateMode();
    }

    public void UpdateMode()
    {
        switch (currentMode)
        {
            case PlayerMode.Light:
                Movement.runSpeed = LigthSpeed;
                sp.color = Color.white;
                attack.AttackCooldownM = AttackCooldownLigth;
                attack.dmgValue = ligthDamage;
                break;

            case PlayerMode.Dark:
                Movement.runSpeed = ShadowSpeed;
                sp.color = Color.black;
                attack.AttackCooldownM = AttackCooldownDark;
                attack.dmgValue = DarkDamage;
                break;
        }
    }

    private void HandleDarkMeter()
    {
        if (currentMode == PlayerMode.Dark)
        {
            float penalty = 1f - (corruptionStage * corruptionPenaltyFactor);
            penalty = Mathf.Clamp(penalty, 0.1f, 1f); // fill rate never below 10%

            darkMeter += darkMeterFillRate * penalty * Time.deltaTime;

            if (darkMeter >= maxDarkMeter)
            {
                darkMeter = maxDarkMeter;
                corruptionStage = 0;     // Reset penalty
                wasAbove80 = false;

                if (!meterFullTriggered)
                {
                    OnDarkMeterFull();
                    meterFullTriggered = true;
                }
            }
            else
            {
                meterFullTriggered = false;
            }
        }
        else // Light mode
        {
            if ((darkMeter / maxDarkMeter >= 0.8f) && !wasAbove80)
            {
                corruptionStage++;
                wasAbove80 = true;
            }

            darkMeter -= darkMeterDrainRate * Time.deltaTime;
            if (darkMeter < 0)
                darkMeter = 0;

            meterFullTriggered = false;
        }

        // UI Update
        if (darkMeterSlider != null)
        {
            darkMeterSlider.value = darkMeter;
        }

        UpdateVignetteEffect();
    }

    private void UpdateVignetteEffect()
    {
        if (vignette == null) return;

        float percent = (darkMeter / maxDarkMeter) * 100f;

        if (percent >= vignetteStartThreshold)
        {
            float t = (percent - vignetteStartThreshold) / (100f - vignetteStartThreshold);
            vignette.intensity.value = Mathf.Lerp(0f, maxVignetteIntensity, t);
        }
        else
        {
            vignette.intensity.value = 0f;
        }
    }

    private void OnDarkMeterFull()
    {
        Debug.Log("Spieler wird ebenfalls zum Monster und stirbt vollständig (z.B. mit einer Animation).");
        // TODO: Animation / Game Over / etc.
    }
}
