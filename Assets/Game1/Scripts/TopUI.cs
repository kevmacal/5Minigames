using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TopUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI txtTime;
    [SerializeField] TextMeshProUGUI txtLifes;
    [SerializeField] UnityEngine.UI.Image statusIcon;
    [SerializeField] Sprite[] statusIconList;

    [Header("Configuración")]
    [SerializeField] float initialTime; // 5 minutos (5 * 60) en float seria 300f
    [SerializeField] int initialLifes;

    private float _time;
    private bool _isActive = false;
    private int _lifesRemaining;
    
    public enum EstadoJuego { Escavando, GameOver, Win }
    //0 Escavando, 1 GameOver, 2 Win

    public static event Action OnTimeOut;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _time = initialTime;
        _lifesRemaining = initialLifes;
        IniciarTemporizador();
        txtLifes.text = _lifesRemaining.ToString();
        if (statusIcon!=null)
        {
            statusIcon.sprite=statusIconList[0];
        }
    }
    public void IniciarTemporizador()
    {
        _isActive = true;
        StartCoroutine(TimerRoutine());
    }
    private IEnumerator TimerRoutine()
    {
        while (_time > 0 && _isActive)
        {
            //Debug.Log(_isActive);
            UpdateTimerTxt();
            yield return new WaitForSeconds(1f); // Espera 1 segundo en cada iteración
            _time -= 1f;
        }
        
        // Evitar que el tiempo siga bajando luego de llegar a 0
        if (_time <= 0)
        {
            _time = 0;
            UpdateTimerTxt();
            TimeOver();
        }
    }
    private void UpdateTimerTxt()
    {
        int minutos = Mathf.FloorToInt(_time / 60);
        int segundos = Mathf.FloorToInt(_time % 60);

        // Formatea para que siempre muestre 2 dígitos por sección (ej. 05:09)
        txtTime.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    private void UpdateLifesTxt()
    {
        txtLifes.text=_lifesRemaining.ToString();
    }
    public void MineTouched()
    {
        if (_isActive)
        {
            if (_lifesRemaining > 0)
            {
                _lifesRemaining--;
            }
            if (_lifesRemaining == 0)
            {
                _isActive = false;
                statusIcon.sprite = statusIconList[1];
            }
            UpdateLifesTxt();
        }        
    }
    public bool IsAlife()
    {
        if (_lifesRemaining != 0) return true;
        return false;
    }
    private void TimeOver()
    {
        Debug.Log("¡El tiempo se ha agotado! Fin del juego.");
        statusIcon.sprite = statusIconList[1];
        OnTimeOut?.Invoke();
        // Aquí puedes disparar la derrota del jugador o quitarle una vida
    }
    public void WinGame()
    {
        _isActive = false;
        statusIcon.sprite = statusIconList[2];
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
