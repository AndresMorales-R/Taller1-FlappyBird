using UnityEngine;

/// <summary>
/// Controla el comportamiento individual de la moneda coleccionable.
/// Detecta el contacto con el jugador, suma al contador del GameManager y se destruye.
/// </summary>
public class Moneda : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si quien colisiona es el jugador
        if (collision.CompareTag("Player"))
        {
            // Sumar al contador de monedas del GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SumarMoneda();
            }

            // Destruir la moneda tras ser recogida
            Destroy(gameObject);
        }
    }
}