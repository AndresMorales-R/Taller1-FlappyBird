using UnityEngine;

/// <summary>
/// Controla el movimiento continuo (scroll) del escenario hacia la izquierda y su reinicio cíclico.
/// Permite configurar distintas velocidades en distintas capas para generar el efecto Parallax.
/// </summary>
public class ScrollFondo : MonoBehaviour
{
    [Tooltip("Velocidad a la que se mueve el fondo. Para el suelo, debe coincidir con la de las tuberías.")]
    public float velocidad = 2f;

    [Tooltip("El ancho exacto del escenario pintado para saber en qué punto debe reiniciar el bucle.")]
    public float anchoFondo;

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Solo se mueve si el juego no está en la pantalla de Game Over o Inicio
        if (Time.timeScale == 0) return;

        // Desplaza el fondo hacia la izquierda constantemente
        transform.position += Vector3.left * velocidad * Time.deltaTime;

        // Si el fondo avanzó una distancia igual a su ancho, salta a su posición original
        if (transform.position.x <= posicionInicial.x - anchoFondo)
        {
            transform.position = posicionInicial;
        }
    }
}