using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goalTxt;
    [SerializeField] BallHandler ball;

    private int _goals;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _goals++;
            goalTxt.text = _goals.ToString();
            ball.LevelUp();
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _goals = 0;
        goalTxt.text=_goals.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current != null)
        {
            //Debug.Log("Hay teclado");
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                Debug.Log("Se presionó la tecla ENTER / ENVIAR");
            }

            if (Keyboard.current.backspaceKey.wasPressedThisFrame)
            {
                Debug.Log("Se presionó la tecla BORRAR (Backspace)");
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Debug.Log("Se presionó la tecla ATRÁS / ESCAPE");
                VolverAInicio();
            }
        }
    }
    public void VolverAInicio()
    {
        // Carga la escena indicada
        SceneManager.LoadScene(0);
    }
}
