using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    public TextMeshProUGUI counterText;
    public float timeLimit = 120.0f; 
    private float currentTime;

    public int orbesObjetivo = 20; 
    private int orbesRecolectados = 0;
    private bool timerActive = false;
    public DesbloqueaFinal2 desbloqueaFinal;

    void Start()
    {
        currentTime = timeLimit;
        counterText.text = "0/" + orbesObjetivo;
        UpdateTimerText();
    }

    void Update()
    {
        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();

            if (currentTime <= 0)
            {
                currentTime = 0;
                timerActive = false;
                Debug.Log("Tiempo agotado");
            }
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        timerActive = true;
    }

    public void IncrementOrbCounter()
    {
        orbesRecolectados++;
        Debug.Log("Orbes recolectados: " + orbesRecolectados);

        counterText.text = $"{orbesRecolectados}/{orbesObjetivo}";      


        if (orbesRecolectados >= orbesObjetivo)
        {
            DesbloquearPuerta();
        }
    }

    private void DesbloquearPuerta()
    {
        if (desbloqueaFinal != null)
        {
            desbloqueaFinal.UnlockDoor();
            
        }
    }


void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartTimer();
        }
    }
}
