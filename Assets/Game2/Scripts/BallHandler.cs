using UnityEngine;
using UnityEngine.EventSystems;

public class BallHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject pointerArrow;
    [SerializeField] GameObject ball;

    private Rigidbody2D _rb;
    private Vector3 _initialPos;
    private Quaternion _initialRotation;
    private bool _isPointerDown;
    private float _tiempoInicio = 0f;
    private float _velRot = 1f;
    private float _maxAngle = 75f;
    private float _finalAngle = 0;
    private float _ballforce = 8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isPointerDown = false;
        _rb = GetComponent<Rigidbody2D>();
        _initialPos = transform.position;
        _initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPointerDown)
        {
            RotateArrow();
        }
    }

    public void ResetBall()
    {
        transform.position = _initialPos;
        transform.rotation= _initialRotation;
        _rb.linearVelocity = Vector2.zero;
    }
    public void LevelUp()
    {
        _velRot = _velRot + 0.25f;
    }

    private void RotateArrow()
    {
        _finalAngle = Mathf.Sin((Time.time - _tiempoInicio) * _velRot) * _maxAngle;
        pointerArrow.transform.localRotation = Quaternion.Euler(0, 0, _finalAngle);
    }
    private void ThrowBall(Vector2 direccion, float fuerza)
    {
        // Sin velocidades previas
        _rb.linearVelocity = Vector2.zero;

        // Fuerza de impulso
        _rb.AddForce(direccion.normalized * fuerza, ForceMode2D.Impulse);
    }
    // Se ejecuta al colocar el dedo en el Sprite
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Pelota sostenida");
        _tiempoInicio = Time.time;
        _isPointerDown = true;
    }

    // Se ejecuta al sacar el dedo del Sprite
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log(_finalAngle);

        Vector2 direccionDisparo = pointerArrow.transform.up;
        _isPointerDown = false;
        ThrowBall(direccionDisparo, _ballforce);
    }
    /*
    // Se ejecuta al tocar el Collider 2D
    private void OnMouseDown()
    {
        Debug.Log("Pelota ClicDown");
    }

    // Se ejecuta al levantar el dedo del Collider 2D
    private void OnMouseUp()
    {
        Debug.Log("Pelota ClickUp");
    }
    */
}
