using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    readonly float fpsUpdateInterval = 0.5f;  // Intervalo de atualiza��o do FPS em segundos
    float timeSinceLastUpdate = 0.0f;  // Tempo acumulado desde a �ltima atualiza��o
    int currentFPS = 0;
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

    private void Update()
    {
        LockAndHideCursor();
        FpsCounter();
    }

    private void LockAndHideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void FpsCounter()
    {
        timeSinceLastUpdate += Time.deltaTime;

        // Verifica se o intervalo de atualiza��o foi alcan�ado
        if (timeSinceLastUpdate >= fpsUpdateInterval)
        {
            // Calcula o FPS m�dio durante o intervalo e arredonda para inteiro
            currentFPS = Mathf.RoundToInt(1.0f / Time.deltaTime);
            HudManager.Instance.fps.text = "FPS: " + currentFPS;

            // Reseta o tempo acumulado
            timeSinceLastUpdate = 0.0f;
        }
    }
}
