using UnityEngine;

public class MovementScript : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [SerializeField] float velocidad;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * velocidad * Time.deltaTime);
    }
}
