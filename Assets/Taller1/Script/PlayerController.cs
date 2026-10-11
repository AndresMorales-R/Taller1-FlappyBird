using UnityEngine;

/// <summary>
/// Clase encargada de controlar el comportamiento principal del Jugador (Flappy).
/// Maneja el impulso de vuelo (aleteo), las animaciones físicas de rotación según la velocidad vertical,
/// la reproducción de sonidos y la detección de colisiones con los obstáculos y coleccionables.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Configuración de Físicas")]
    [Tooltip("Fuerza del impulso vertical aplicada al aletear hacia arriba.")]
    public float fuerzaSalto = 6.5f;

    [Header("Configuración de Rotación")]
    [Tooltip("Multiplicador para inclinar al pájaro al caer o subir, replicando el juego original.")]
    public float multiplicadorRotacion = 4f;

    [Header("Efectos de Sonido")]
    [Tooltip("Clip de audio que se reproduce al aletear.")]
    public AudioClip sonidoAleteo;
    [Tooltip("Clip de audio que se reproduce al chocar y perder la partida.")]
    public AudioClip sonidoChoque;

    // Referencias a los componentes internos del GameObject
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool estaMuerto = false;

    /// <summary>
    /// Método de inicialización llamado al inicio de la escena.
    /// Obtiene y almacena los componentes necesarios para evitar búsquedas repetitivas.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Método de actualización llamado una vez por frame.
    /// Se encarga de leer el input del usuario (Espacio o Clic) y ajustar la rotación visual del personaje.
    /// </summary>
    void Update()
    {
        // Bloquear controles si el jugador chocó O si el juego está en pausa (Game Over)
        if (estaMuerto || Time.timeScale == 0f) return;

        // Detectar la tecla Espacio o el Clic izquierdo del mouse para aletear
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Aletear();
        }

        // Rotación del pájaro basada en su velocidad vertical...
        float anguloRotacion = rb.linearVelocity.y * multiplicadorRotacion;
        anguloRotacion = Mathf.Clamp(anguloRotacion, -90f, 45f);
        transform.rotation = Quaternion.Euler(0, 0, anguloRotacion);
    }

    /// <summary>
    /// Aplica una fuerza vertical hacia arriba para simular el aleteo.
    /// Reinicia la velocidad vertical previa para que la respuesta del salto sea consistente,
    /// y reproduce el sonido de impulso.
    /// </summary>
    private void Aletear()
    {
        // Se anula la velocidad en 'y' antes del salto para no acumular fuerzas gravitatorias
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

        // Reproducir el sonido de aleteo si está asignado
        if (sonidoAleteo != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoAleteo);
        }
    }

    /// <summary>
    /// Detecta colisiones físicas letales (Tuberías, Suelo, Techo u otros obstáculos sólidos).
    /// </summary>
    /// <param name="collision">Datos físicos de la colisión.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Cualquier impacto físico con un collider sólido resulta en Game Over
        if (!estaMuerto)
        {
            Morir();
        }
    }

    /// <summary>
    /// Detecta la entrada a zonas trigger sin físicas duras (espacio para sumar puntos o recolección de monedas).
    /// </summary>
    /// <param name="collision">Datos del collider tipo trigger.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (estaMuerto) return;

        // Identifica si el objeto recolectado es una moneda
        if (collision.CompareTag("Moneda"))
        {
            // Aquí deberás conectar con tu GameManager (ej. GameManager.Instance.SumarMoneda())
            Destroy(collision.gameObject); // Destruye la moneda al recogerla
        }
        // Identifica si el jugador atravesó una tubería exitosamente
        else if (collision.CompareTag("Puntaje"))
        {
            // Aquí deberás conectar con tu GameManager (ej. GameManager.Instance.SumarPunto())
        }
    }

    /// <summary>
    /// Maneja la lógica interna de derrota: cambia el estado del jugador a muerto, 
    /// reproduce el sonido de choque y prepara el terreno para la pantalla de fin de juego.
    /// </summary>
    private void Morir()
    {
        estaMuerto = true;

        if (sonidoChoque != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoChoque);
        }

        // Aquí le avisamos al GameManager que active la pantalla final
        GameManager.Instance.MostrarGameOver();
    }
}