using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class ReiniciarEscena: MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que choca tiene el tag "Player" y el láser tiene el tag "Laser"
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Laser"))
        {
            // Reinicia la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("El jugador colisionó con un láser. La escena ha sido reiniciada.");
        }
    }
}