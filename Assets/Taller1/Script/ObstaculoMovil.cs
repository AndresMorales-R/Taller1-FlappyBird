using UnityEngine;

/// <summary>
/// Controla el desplazamiento constante hacia la izquierda de los obstáculos y su destrucción
/// una vez que salen del límite de la pantalla.
/// </summary>
public class ObstaculoMovil : MonoBehaviour
{
    [Tooltip("Velocidad de desplazamiento horizontal hacia la izquierda junto con el scroll.")]
    public float velocidad = 3.5f;

    [Tooltip("Posición en el eje X donde el obstáculo se destruirá para liberar memoria.")]
    public float limiteDestruccionX = -15f;

    void Update()
    {
        // Movimiento constante hacia la izquierda en cada frame
        transform.position += Vector3.left * velocidad * Time.deltaTime;

        // Destrucción del GameObject cuando sale del encuadre de la cámara
        if (transform.position.x < limiteDestruccionX)
        {
            Destroy(gameObject);
        }
    }
}