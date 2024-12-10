using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerDialogos : MonoBehaviour
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
    [SerializeField] GameObject panelDialogo;
    [SerializeField] MonoBehaviour scriptObjeto;

    int contador;

    void mostrarDialogo()
    {
        txt_mensaje.text = charla[contador].texto;
        txt_nombre.text = charla[contador].name;
    }

    void finalizarConversacion()
    {
        canvasIntroduccion.gameObject.SetActive(false);
        scriptObjeto.enabled = true;
    }

    void Start()
    {
        contador = 0;
        scriptObjeto.enabled = false;
        mostrarDialogo();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
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