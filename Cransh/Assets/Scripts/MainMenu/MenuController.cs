using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    // Instanciar el controlador de SFX
    public SFXController sfxController;

    // Start is called before the first frame update   
    void Start()
    {
        // Obtener el controlador de SFX
        sfxController = SFXController.instance;
        sfxController.PlaySFX(4);
    }

    public void StartGame()
    {
        // Cargar la escena del juego
        SceneManager.LoadScene("Casa");
        sfxController.PlaySFX(5);

    }

    public void QuitGame()
    {
        // Salir de la aplicación
        Application.Quit();
    }
}
