using UnityEngine;

public class Oscilador : MonoBehaviour
{
    public Vector3 startPosition = new Vector3(40.0999985f, 19.9428005f, 15.3000002f);
    public Vector3 endPosition = new Vector3(40.0999985f, 19.9428005f, -19.3999996f); 
    public float speed = 2.0f; // Velocidad de movimiento



    void Start()
    {
        // Asignamos la posición inicial al transform para evitar problemas
        transform.position = startPosition;

    }

    void Update()
    {
        // Movimiento de ida y vuelta entre startPosition y endPosition usando Mathf.PingPong
        float t = Mathf.PingPong(Time.time * speed, 1.0f); // Interpolación entre 0 y 1
        transform.position = Vector3.Lerp(startPosition, endPosition, t);
    }
}
