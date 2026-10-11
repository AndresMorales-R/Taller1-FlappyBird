using UnityEngine;

/// <summary>
/// Representa un obstáculo con apariencia de coleccionable pero con efecto letal.
/// Al entrar en contacto con el jugador, desencadena la muerte y el Game Over.
/// </summary>
public class MonedaRoja : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detectar si el objeto con el que choca es el pájaro
        if (collision.CompareTag("Player"))
        {
            // Intentar obtener el componente del jugador para ejecutar su muerte
            PlayerController jugador = collision.GetComponent<PlayerController>();

            if (jugador != null)
            {
                // Si la función Morir() en PlayerController es pública, la llamamos directo:
                // jugador.Morir();

                // O notificamos directamente al GameManager si prefieres:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.MostrarGameOver();
                }
            }

            // Opcional: destruir la trampa tras activarse
            Destroy(gameObject);
        }
    }
}