using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Clase administradora del estado general del juego (Singleton).
/// Controla la puntuación (tuberías y monedas), las pantallas de Inicio y Game Over,
/// el guardado del récord (High Score) y la gestión general del flujo de la partida.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Patrón Singleton para acceder al GameManager desde cualquier otro script fácilmente
    public static GameManager Instance { get; private set; }

    [Header("Interfaz de Usuario (UI)")]
    [Tooltip("Texto en pantalla para el puntaje de tuberías.")]
    public TextMeshProUGUI textoPuntaje;
    [Tooltip("Texto en pantalla para el contador de monedas.")]
    public TextMeshProUGUI textoMonedas;
    [Tooltip("Texto en pantalla de Game Over para mostrar el puntaje final.")]
    public TextMeshProUGUI textoPuntajeFinal;
    [Tooltip("Texto en pantalla de Game Over para mostrar el récord histórico.")]
    public TextMeshProUGUI textoRecord;

    [Header("Pantallas")]
    [Tooltip("Panel o Canvas que contiene la UI de 'Tap/Click para comenzar'.")]
    public GameObject pantallaInicio;
    [Tooltip("Panel o Canvas que contiene la UI de Game Over.")]
    public GameObject pantallaGameOver;

    [Header("Audio")]
    [Tooltip("Sonido a reproducir cuando se recoge una moneda.")]
    public AudioClip sonidoMoneda;
    [Tooltip("Jingle a reproducir cuando se supera el récord histórico.")]
    public AudioClip sonidoNuevoRecord;
    private AudioSource audioSource;

    // Variables internas de estado
    private int puntaje = 0;
    private int monedas = 0;
    private bool juegoIniciado = false;

    /// <summary>
    /// Configura el Singleton y pausa el juego al inicio para mostrar la pantalla de bienvenida.
    /// </summary>
    private void Awake()
    {
        // Configuración del Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Estado inicial de la UI
        pantallaInicio.SetActive(true);
        pantallaGameOver.SetActive(false);
        ActualizarUI();

        // Pausa el juego (tiempo a 0) hasta que el jugador inicie
        Time.timeScale = 0f;
    }

    private void Update()
    {
        // Detecta el input para iniciar la partida si aún no ha comenzado
        if (!juegoIniciado && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            IniciarJuego();
        }
    }

    /// <summary>
    /// Oculta la pantalla de inicio y reanuda el tiempo para comenzar el movimiento.
    /// </summary>
    private void IniciarJuego()
    {
        juegoIniciado = true;
        pantallaInicio.SetActive(false);
        Time.timeScale = 1f; // El tiempo vuelve a la normalidad
    }

    /// <summary>
    /// Incrementa el puntaje principal (tuberías esquivadas) y actualiza la UI.
    /// </summary>
    public void AumentarPuntaje()
    {
        puntaje++;
        ActualizarUI();
    }

    /// <summary>
    /// Incrementa el contador independiente de monedas, reproduce el sonido correspondiente y actualiza la UI.
    /// </summary>
    public void SumarMoneda()
    {
        monedas++;
        ActualizarUI();

        if (sonidoMoneda != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoMoneda);
        }
    }

    /// <summary>
    /// Detiene el juego, verifica si hay un nuevo récord, guarda los datos y muestra la pantalla de Game Over.
    /// </summary>
    public void MostrarGameOver()
    {
        Time.timeScale = 0f; // Pausa todo el movimiento
        pantallaGameOver.SetActive(true);

        // Lógica de guardado y carga del récord usando PlayerPrefs
        int recordGuardado = PlayerPrefs.GetInt("MejorPuntaje", 0);

        if (puntaje > recordGuardado)
        {
            recordGuardado = puntaje;
            PlayerPrefs.SetInt("MejorPuntaje", recordGuardado);
            PlayerPrefs.Save();

            // Reproduce jingle de nuevo récord si existe
            if (sonidoNuevoRecord != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoNuevoRecord);
            }
        }

        // Actualizar textos de la pantalla final
        if (textoPuntajeFinal != null) textoPuntajeFinal.text = "Puntaje: " + puntaje.ToString();
        if (textoRecord != null) textoRecord.text = "Mejor Récord: " + recordGuardado.ToString();
    }

    /// <summary>
    /// Reinicia la escena actual, devolviendo todo a su estado inicial.
    /// Se debe vincular al botón de reinicio en la pantalla de Game Over.
    /// </summary>
    public void ReiniciarJuego()
    {
        // Se carga nuevamente la escena activa actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Centraliza la actualización de los textos en pantalla para mantener el código limpio.
    /// </summary>
    private void ActualizarUI()
    {
        if (textoPuntaje != null) textoPuntaje.text = puntaje.ToString();
        if (textoMonedas != null) textoMonedas.text = monedas.ToString();
    }
}