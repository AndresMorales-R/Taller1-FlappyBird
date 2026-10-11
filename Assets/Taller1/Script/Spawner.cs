using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject obstaculoPrefab;
    public GameObject cartelPrefab;
    public GameObject monedaRojaPrefab;

    [Header("Tiempos de Generación")]
    public float tiempoGeneracion = 1.8f;
    private float temporizador = 0f;

    [Header("Rango de Altura Tuberías y Monedas")]
    public float alturaMinima = -1.2f;
    public float alturaMaxima = 2.0f;

    [Header("Configuración del Cartel")]
    [Tooltip("Altura fija constante (Eje Y) en la que siempre aparecerá el cartel.")]
    public float alturaFijaCartel = 0.5f;

    [Tooltip("Velocidad de movimiento de los obstáculos (debe coincidir con la de ObstaculoMovil).")]
    public float velocidadEscenario = 2.5f;

    // Control de periodicidad
    private int contadorCartel = 0;
    private int objetivoCartel;
    private int contadorMonedaRoja = 0;
    private int objetivoMonedaRoja;

    private void Start()
    {
        objetivoCartel = Random.Range(3, 5);
        objetivoMonedaRoja = Random.Range(3, 6);
    }

    private void Update()
    {
        temporizador += Time.deltaTime;

        if (temporizador >= tiempoGeneracion)
        {
            GenerarCiclo();
            temporizador = 0f;
        }
    }

    private void GenerarCiclo()
    {
        // 1. Tubería estándar en la posición actual del Spawner
        float alturaTuberias = Random.Range(alturaMinima, alturaMaxima);
        Vector3 posicionTuberias = new Vector3(transform.position.x, alturaTuberias, 0f);
        Instantiate(obstaculoPrefab, posicionTuberias, Quaternion.identity);

        contadorCartel++;
        contadorMonedaRoja++;

        // 2. Generación del cartel centrado entre tuberías
        if (contadorCartel >= objetivoCartel)
        {
            GenerarCartel();
            contadorCartel = 0;
            objetivoCartel = Random.Range(3, 5);
        }

        // 3. Generación de la moneda roja
        if (contadorMonedaRoja >= objetivoMonedaRoja)
        {
            GenerarMonedaRoja();
            contadorMonedaRoja = 0;
            objetivoMonedaRoja = Random.Range(3, 6);
        }
    }

    [Header("Ajustes Finos de Posición en X")]
    [Tooltip("Ajuste manual del cartel en X. Valores positivos lo mueven hacia la derecha; negativos hacia la izquierda.")]
    [Range(-5f, 5f)]
    public float ajusteFinoX = 0f;

    [Tooltip("Ajuste manual de la moneda en X. Valores positivos la adelantan; negativos la retrasan respecto al tubo.")]
    [Range(-5f, 5f)]
    public float ajusteFinoMonedaX = 0f;

    private void GenerarCartel()
    {
        if (cartelPrefab == null) return;

        // Distancia total entre dos columnas de tuberías
        float distanciaEntreTuberias = velocidadEscenario * tiempoGeneracion;

        // Posición intermedia base + calibración fina
        // Se toma la cuarta parte como referencia intermedia y se suma el ajuste del Inspector
        float xCartel = transform.position.x - (distanciaEntreTuberias * 0.25f) + ajusteFinoX;

        Vector3 posicionCartel = new Vector3(xCartel, alturaFijaCartel, 0f);
        Instantiate(cartelPrefab, posicionCartel, Quaternion.identity);
    }

    private void GenerarMonedaRoja()
    {
        if (monedaRojaPrefab == null) return;

        float alturaMoneda = Random.Range(alturaMinima, alturaMaxima);

        // Se le suma el ajuste fino a la posición original del Spawner en X
        float xMoneda = transform.position.x + ajusteFinoMonedaX;

        Vector3 posicionMoneda = new Vector3(xMoneda, alturaMoneda, 0f);
        Instantiate(monedaRojaPrefab, posicionMoneda, Quaternion.identity);
    }
}