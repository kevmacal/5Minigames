using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TapTouchGame()
    {
        SceneManager.LoadScene(1);
    }
    public void HoldGame()
    {
        SceneManager.LoadScene(2);
    }
    public void SwipeGame()
    {
        SceneManager.LoadScene(3);
    }
    public void ExitGame()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}
