using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MacheteSwipeController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Identificador")]
    [Tooltip("Número o identificador de este cuadro (ej. 1 al 6)")]
    [SerializeField] int numeroMachete;

    [Header("Configuración de Swipe")]
    [Tooltip("Distancia mínima en píxeles para considerar que fue un swipe y no un toque simple Recomendado 50f")]
    [SerializeField] float umbralSwipe;
    
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de desplazamiento hacia el punto final (en unidades por segundo o píxeles) 5f lento")]
    [SerializeField] float velocidadMovimiento;
    [SerializeField] float waitTime;

    private Vector2 _posIni;
    private Vector3 _posOrigen;
    private Coroutine _rutinaMovimiento;
    private Camera _mainCamera;
    private bool _isOnAttack;
    public void OnPointerDown(PointerEventData eventData)
    {
        //Si esta en ataque no debe poder usarse
        if (_isOnAttack) return;
        _posIni = eventData.position;
        Debug.Log($"[TOUCH DETECTADO] Cuadro {numeroMachete} presionado en: {eventData.position}");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isOnAttack) return;
        Vector2 posicionFinal = eventData.position;
        Vector2 desplazamiento = posicionFinal - _posIni;

        // Comprobamos si el movimiento superó la distancia mínima
        if (desplazamiento.magnitude >= umbralSwipe)
        {
            string direccion = CalcularDireccionSwipe(desplazamiento);

            // Imprime el Debug que necesitas
            Debug.Log($"Swipe {direccion} en cuadro {numeroMachete}");
            Vector3 destino = ObtenerPosicionDestino(eventData.position);
            // Detenemos un movimiento previo si la cuchilla aún se estaba moviendo
            if (_rutinaMovimiento != null)
            {
                StopCoroutine(_rutinaMovimiento);
            }
            // Iniciamos el movimiento hacia el final del swipe
            _rutinaMovimiento = StartCoroutine(MoverHaciaDestino(destino));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!_isOnAttack) return;
        if (collision.CompareTag("Player")||collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
    }

    private string CalcularDireccionSwipe(Vector2 desplazamiento)
    {
        // Comparamos el eje X vs el eje Y para saber la dirección dominante
        if (Mathf.Abs(desplazamiento.x) > Mathf.Abs(desplazamiento.y))
        {
            return desplazamiento.x > 0 ? "Derecha" : "Izquierda";
        }
        else
        {
            return desplazamiento.y > 0 ? "Arriba" : "Abajo";
        }
    }
    private IEnumerator MoverHaciaDestino(Vector3 puntoDestino)
    {
        _isOnAttack = true;
        puntoDestino.z = _posOrigen.z; // Mantener profundidad Z

        // --- 1. RECORRIDO DE IDA ---
        while (Vector3.Distance(transform.position, puntoDestino) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                puntoDestino,
                velocidadMovimiento * Time.deltaTime
            );
            yield return null;
        }
        transform.position = puntoDestino;

        // --- 2. PAUSA EN EL PUNTOS DE IMPACTO ---
        // Aquí es donde en el futuro aplicas el daño a los enemigos
        yield return new WaitForSeconds(waitTime);

        // --- 3. RECORRIDO DE VUELTA AL ORIGEN ---
        while (Vector3.Distance(transform.position, _posOrigen) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                _posOrigen,
                velocidadMovimiento * Time.deltaTime
            );
            yield return null;
        }

        // Aseguramos que vuelva exactamente a su origen y habilitamos nuevos toques
        transform.position = _posOrigen;
        _isOnAttack = false;
    }

    private Vector3 ObtenerPosicionDestino(Vector2 pantallaPos)
    {
        // Si el objeto es un Sprite 2D en la escena
        if (GetComponent<SpriteRenderer>() != null)
        {
            Vector3 posMundo = _mainCamera.ScreenToWorldPoint(new Vector3(pantallaPos.x, pantallaPos.y, _mainCamera.nearClipPlane));
            return posMundo;
        }

        // Si el objeto está dentro de un Canvas UI (RectTransform)
        return pantallaPos;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainCamera=Camera.main;
        _posOrigen = transform.position;
        _isOnAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
