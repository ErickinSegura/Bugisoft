using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContratosController : MonoBehaviour
{
    public string[] contratos; // Array de nombres de las escenas de los contratos
    public Sprite[] contratosImg; // Array de Sprites para los contratos
    private int contratoActivoIndex; // �ndice del contrato activo
    public Image uiImage; // Referencia a la imagen de la UI que va a cambiar

    // Start is called before the first frame update
    void Start()
    {
        // Seleccionar un contrato aleatorio al inicio
        SeleccionarContratoAleatorio();
    }

    // M�todo para seleccionar un contrato aleatorio
    void SeleccionarContratoAleatorio()
    {
        contratoActivoIndex = Random.Range(0, contratos.Length);
        CambiarImagenUI();
    }

    // M�todo para cambiar la imagen de la UI
    void CambiarImagenUI()
    {
        if (contratoActivoIndex >= 0 && contratoActivoIndex < contratosImg.Length)
        {
            uiImage.sprite = contratosImg[contratoActivoIndex];
        }
    }

    // M�todo para cambiar a la escena del contrato espec�fico
    public void CambiarEscenaContrato()
    {
        string nombreEscena = contratos[contratoActivoIndex];
        SceneManager.LoadScene(nombreEscena);
    }

    // Update is called once per frame
    void Update()
    {
        // Este m�todo puede quedar vac�o si no necesitas l�gica de actualizaci�n
    }
}
