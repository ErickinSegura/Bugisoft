using UnityEngine;

public class FrezzeCharacter : MonoBehaviour
{
    public GameObject contrato;
    public GameObject player; // El personaje que quieres congelar
    private CharacterController characterController; // Controlador del personaje
    private bool isFrozen = false;

    void Start()
    {
        // Obtiene el componente CharacterController del personaje
        characterController = player.GetComponent<CharacterController>();

        // Asegúrate de que el canvas esté desactivado al inicio
        contrato.SetActive(false);
    }

    void Update()
    {
        // Aquí puedes definir la condición bajo la cual se activará el canvas y se congelará el personaje
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleFreeze();
        }
    }

    void ToggleFreeze()
    {
        isFrozen = !isFrozen;

        if (isFrozen)
        {
            // Activa el canvas y desactiva el movimiento del personaje
            contrato.SetActive(true);
            characterController.enabled = false;
        }
        else
        {
            // Desactiva el canvas y activa el movimiento del personaje
            contrato.SetActive(false);
            characterController.enabled = true;
        }
    }
}
