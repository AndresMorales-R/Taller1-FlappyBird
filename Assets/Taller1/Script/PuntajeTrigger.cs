using UnityEngine;

public class PuntajeTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el pájaro cruzó la tubería
        if (collision.gameObject.CompareTag("Player"))
        {
            // Llamamos a TU método exacto usando tu Singleton
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AumentarPuntaje();
            }
            else
            {
                Debug.LogWarning("No se encontró el GameManager en la escena.");
            }
        }
    }
}