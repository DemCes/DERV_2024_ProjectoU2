using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para manejar escenas

public class ReiniciarEscena: MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Verifica si el objeto que choca tiene el tag "Player" y el l�ser tiene el tag "Laser"
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("Laser"))
        {
            // Reinicia la escena actual
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Debug.Log("El jugador colision� con un l�ser. La escena ha sido reiniciada.");
        }
    }
}