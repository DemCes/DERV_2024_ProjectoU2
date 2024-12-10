using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DesbloqueaFinal : MonoBehaviour
{
    public GameObject door;
    public GameObject additionalDoor;
    public TextMeshPro textMeshPro;
    public Material glowWinDoorMaterial;  // Asigna este material en el inspector de Unity
    public Color newTextColor = Color.green;
    public Color defaultTextColor = Color.white;

    private Renderer doorRenderer;
    private Renderer additionalDoorRenderer;
    private bool doorUnlocked = false;
    private Door doorScript;

    void Start()
    {
        // Asignar los renderers y desactivar el script de la puerta inicialmente
        if (door != null)
        {
            doorRenderer = door.GetComponent<Renderer>();
            doorScript = door.GetComponent<Door>();
            doorScript.enabled = false;  // Desactivar la puerta al inicio
        }

        if (additionalDoor != null)
        {
            additionalDoorRenderer = additionalDoor.GetComponent<Renderer>();
        }

        // Colores iniciales del texto
        if (textMeshPro != null)
        {
            textMeshPro.color = defaultTextColor;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Si el trigger es tocado por el jugador y la puerta aún no ha sido desbloqueada
        if (other.CompareTag("Player") && !doorUnlocked)
        {
            // Asignar el material "Glow Win Door" a la puerta
            if (doorRenderer != null && glowWinDoorMaterial != null)
            {
                doorRenderer.material = glowWinDoorMaterial;
            }

            // Asignar el material "Glow Win Door" a la additionalDoor
            if (additionalDoorRenderer != null && glowWinDoorMaterial != null)
            {
                additionalDoorRenderer.material = glowWinDoorMaterial;
            }

            // Cambiar el color del TextMeshPro
            if (textMeshPro != null)
            {
                textMeshPro.color = newTextColor;
            }

            // Desbloquear la puerta
            if (doorScript != null)
            {
                doorScript.isUnlocked = true;
                doorScript.enabled = true;  // Habilitar el script de la puerta
            }

            doorUnlocked = true;
            gameObject.SetActive(false);  // Desactivar el trigger una vez que ha sido activado
        }
    }
}
