using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EggsController : MonoBehaviour
{
    [SerializeField] List<GameObject> Eggs;
    [SerializeField] SpawnerController spawnGR;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI _finalStatus;
    [SerializeField] TextMeshProUGUI _huevosGallinas;

    private int _eggsGallina=0;
    private int _eggsRatas = 0;
    private int _totalEggs;
    private int _initialEggs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _totalEggs=Eggs.Count;
        _initialEggs = _totalEggs;
        canvas.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_totalEggs>0)
        {
            if (collision.CompareTag("Player")|| collision.CompareTag("Enemy"))
            {
                _totalEggs--;
                if (collision.CompareTag("Player"))
                {
                    _eggsGallina++;
                }
                else
                {
                    _eggsRatas++;
                }
                Destroy(Eggs[_totalEggs]);
                Destroy(collision.gameObject);
            }            
        }
        
        if (_totalEggs==0)
        {
            spawnGR.DetenerSpawneo();
            canvas.enabled = true;
            if (_eggsGallina==_initialEggs)
            {
                _finalStatus.text = "Has Ganado!!";
                _huevosGallinas.text="Salvaste todos los huevos";
            }
            else
            {
                if (_eggsRatas==_initialEggs)
                {
                    _finalStatus.text = "Has Perdido";
                    _huevosGallinas.text = "No pudiste salvar un solo huevo";
                }
                else
                {
                    _finalStatus.text = "Buen intento";
                    _huevosGallinas.text = "Salvaste "+_eggsGallina+" huevos";
                }
            }
        }
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
