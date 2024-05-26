using UnityEngine;

public class FreezeCharacter : MonoBehaviour
{
    public GameObject contrato; // El objeto vacío que representa el contrato
    public GameObject player; // El personaje que quieres congelar
    private PlayerLook playerLook; // Componente PlayerLook del personaje

    void Start()
    {
        playerLook =player.GetComponent<PlayerLook>();

        // Congela el movimiento de la cámara al inicio
        FreezeLook();
    }

    public void FreezeLook()
    {
        // Desactiva el movimiento de la cámara
        playerLook.canLook = false;
    }

    public void UnfreezeLook()
    {
        // Activa el movimiento de la cámara
        playerLook.canLook = true;
    }

    // Método para activar el contrato
    public void ActivateContract()
    {
        contrato.SetActive(true);
        FreezeLook();
    }

    // Método para desactivar el contrato
    public void DeactivateContract()
    {
        contrato.SetActive(false);
        UnfreezeLook();
    }
}
