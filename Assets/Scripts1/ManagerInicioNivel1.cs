using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerInicioNivel1 : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogo
    {
        public string name;
        public string texto;
    }

    public List<Dialogo> charla;

    [SerializeField] TextMeshProUGUI txt_mensaje;
    [SerializeField] TextMeshProUGUI txt_nombre;
    [SerializeField] Canvas canvasIntroduccion;
    [SerializeField] Camera camaraPrincipal;

    // Panel references
    [SerializeField] GameObject panel1; // Panel for introduction text
    [SerializeField] GameObject panel2; // Panel for dialogue (contains txt_mensaje and txt_nombre)

    int contador;
    bool enIntroduccion = true;

    void mostrarDialogo()
    {
        txt_mensaje.text = charla[contador].texto;
        txt_nombre.text = charla[contador].name;
    }

    void finalizarIntroduccion()
    {
        panel1.SetActive(false);    // Hide introduction panel
        panel2.SetActive(true);     // Show dialogue panel
        mostrarDialogo();           // Start the conversation
    }

    void finalizarConversacion()
    {
        canvasIntroduccion.gameObject.SetActive(false); // Hide the entire canvas
        camaraPrincipal.gameObject.SetActive(false);    // Disable main camera
    }

    void Start()
    {
        contador = 0;

        // Initial setup
        camaraPrincipal.gameObject.SetActive(true);
        canvasIntroduccion.gameObject.SetActive(true);

        // Panel setup
        panel1.SetActive(true);     // Show introduction panel
        panel2.SetActive(false);    // Hide dialogue panel initially
    }

    void Update()
    {
        if (enIntroduccion && Input.GetKeyDown(KeyCode.E))
        {
            enIntroduccion = false;
            finalizarIntroduccion();
        }
        else if (!enIntroduccion && Input.GetKeyDown(KeyCode.C))
        {
            contador++;
            if (contador >= charla.Count)
            {
                finalizarConversacion();
            }
            else
            {
                mostrarDialogo();
            }
        }
    }
}