using UnityEngine;

public class RotarYRebotar : MonoBehaviour
{
    // Velocidad de rotaci�n en grados por segundo
    public float velocidadRotacion = 90f;
    // Amplitud y frecuencia del movimiento de rebote
    public float amplitudRebote = 1f;
    public float frecuenciaRebote = 1f;

    // Posici�n original del objeto
    private Vector3 posicionInicial;

    void Start()
    {
        // Guardar la posici�n inicial del objeto
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Rotar el objeto alrededor del eje Z
        transform.Rotate(Vector3.forward * velocidadRotacion * Time.deltaTime);

        // Calcular la nueva posici�n en Y usando una funci�n seno para el efecto de rebote
        float nuevaPosicionY = posicionInicial.y + Mathf.Sin(Time.time * frecuenciaRebote) * amplitudRebote;

        // Actualizar la posici�n del objeto
        transform.position = new Vector3(posicionInicial.x, nuevaPosicionY, posicionInicial.z);
    }
}
