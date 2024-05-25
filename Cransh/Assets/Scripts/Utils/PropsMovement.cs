using UnityEngine;

public class RotarYRebotar : MonoBehaviour
{
    // Velocidad de rotación en grados por segundo
    public float velocidadRotacion = 90f;
    // Amplitud y frecuencia del movimiento de rebote
    public float amplitudRebote = 1f;
    public float frecuenciaRebote = 1f;

    // Posición original del objeto
    private Vector3 posicionInicial;

    void Start()
    {
        // Guardar la posición inicial del objeto
        posicionInicial = transform.position;
    }

    void Update()
    {
        // Rotar el objeto alrededor del eje Z
        transform.Rotate(Vector3.forward * velocidadRotacion * Time.deltaTime);

        // Calcular la nueva posición en Y usando una función seno para el efecto de rebote
        float nuevaPosicionY = posicionInicial.y + Mathf.Sin(Time.time * frecuenciaRebote) * amplitudRebote;

        // Actualizar la posición del objeto
        transform.position = new Vector3(posicionInicial.x, nuevaPosicionY, posicionInicial.z);
    }
}
