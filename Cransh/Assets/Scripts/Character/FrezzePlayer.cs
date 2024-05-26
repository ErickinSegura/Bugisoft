using UnityEngine;

public class FreezeCharacter : MonoBehaviour
{
    public GameObject contrato; // El objeto vac�o que representa el contrato
    public GameObject player; // El personaje que quieres congelar
    private PlayerLook playerLook; // Componente PlayerLook del personaje

    void Start()
    {
        playerLook =player.GetComponent<PlayerLook>();

        // Congela el movimiento de la c�mara al inicio
        FreezeLook();
    }

    public void FreezeLook()
    {
        // Desactiva el movimiento de la c�mara
        playerLook.canLook = false;
    }

    public void UnfreezeLook()
    {
        // Activa el movimiento de la c�mara
        playerLook.canLook = true;
    }

    // M�todo para activar el contrato
    public void ActivateContract()
    {
        contrato.SetActive(true);
        FreezeLook();
    }

    // M�todo para desactivar el contrato
    public void DeactivateContract()
    {
        contrato.SetActive(false);
        UnfreezeLook();
    }
}
