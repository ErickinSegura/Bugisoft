using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Cargar la escena del juego
        SceneManager.LoadScene("Casa");
    }

    public void QuitGame()
    {
        // Salir de la aplicación
        Application.Quit();
    }
}
