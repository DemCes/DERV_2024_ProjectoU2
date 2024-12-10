using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine;

public class ManagerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTiempo;
    [SerializeField] TextMeshProUGUI textOrbesRestantes;
    [SerializeField] TextMeshProUGUI textOrbesFake;
    [SerializeField] TextMeshProUGUI textScore;

    [SerializeField] GameObject panelFinal;
    [SerializeField] TextMeshProUGUI textFinalPuntuacion;


    [SerializeField] GameObject segundoPersonaje;
    [SerializeField] Camera camaraSegundoPersonaje;
    [SerializeField] Camera camaraPrimerPersonaje;
    [SerializeField] GameObject puntoDeTeletransporte;

    private int contadorSegundos;
    private int orbesRestantes;
    private int fallos;
    private int score;
    private bool juegoTerminado;

    void Start()
    {
        contadorSegundos = 30;
        orbesRestantes = 6;
        fallos = 0;
        score = 0;

        textTiempo.text = contadorSegundos.ToString();
        textOrbesRestantes.text = orbesRestantes.ToString();
        textOrbesFake.text = fallos.ToString();
        textScore.text = score.ToString();

        juegoTerminado = false;
        panelFinal.SetActive(false);

        if (segundoPersonaje != null)
        {
            segundoPersonaje.SetActive(false); // Desactiva el segundo personaje al inicio
        }

        if (camaraSegundoPersonaje != null)
        {
            camaraSegundoPersonaje.enabled = false; // Desactiva la cámara del segundo personaje al inicio
        }

        if (camaraPrimerPersonaje != null)
        {
            camaraPrimerPersonaje.enabled = true; // Asegúrate de que la cámara del primer personaje esté activa
        }


        StartCoroutine(CorutinaTiempo());
    }

    IEnumerator CorutinaTiempo()
    {
        while (contadorSegundos > 0 && orbesRestantes > 0)
        {
            textTiempo.text = contadorSegundos.ToString();
            contadorSegundos--;
            yield return new WaitForSeconds(1.0f);
        }
        if (orbesRestantes == 0)
        {
            TerminarJuego();
        }
    }

    private void CalcularScore()
    {
        score = (contadorSegundos * 100) + ((6 - orbesRestantes) * 500) - (fallos * 200);
        textScore.text = score.ToString();

    }

    public void TocarOrbe()
    {
        if (orbesRestantes > 0)
        {
            orbesRestantes--;
            textOrbesRestantes.text = orbesRestantes.ToString();
            Debug.Log("Orbe real tocado. Orbes restantes: " + orbesRestantes);
            CalcularScore();
        }
    }

    public void TocarFake()
    {
        fallos++;
        textOrbesFake.text = fallos.ToString();
        CalcularScore();
    }

    private void TerminarJuego()
    {
        juegoTerminado = true;
        panelFinal.SetActive(true); // Muestra el panel final
        textFinalPuntuacion.text = "Juego Terminado" + score;


    }

    void Update()
    {
        // Detectar la tecla R para volver a controlar al segundo personaje
        if (juegoTerminado && Input.GetKeyDown(KeyCode.R))
        {
            VolverControlOtroPersonaje();
        }
    }

    private void VolverControlOtroPersonaje()
    {
        if (segundoPersonaje != null)
        {
            segundoPersonaje.SetActive(true);

            segundoPersonaje.transform.position = puntoDeTeletransporte.transform.position;

            // Activa el segundo personaje
        }

        if (camaraSegundoPersonaje != null)
        {
            camaraSegundoPersonaje.enabled = true; // Activa la cámara del segundo personaje
        }

        if (camaraPrimerPersonaje != null)
        {
            camaraPrimerPersonaje.enabled = false; // Desactiva la cámara del primer personaje
        }

        panelFinal.SetActive(false); // Oculta el panel final
        Debug.Log("Cambiado al segundo personaje y su cámara.");
    }
}