

using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider healthBarSlider;  // Referencia al componente Slider de la barra de vida
    public float maxHealth = 100f;  // Valor máximo de salud
    private float currentHealth;  // Valor actual de salud

    void Start()
    {
        // Inicializar la salud actual y el valor del Slider
        currentHealth = maxHealth;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = currentHealth;
        Debug.Log("Initial Health: " + currentHealth);
    }

    void Update()
    {
        // Ejemplo de lógica para disminuir salud (puedes personalizar esta lógica según tus necesidades)
        if (Input.GetKeyDown(KeyCode.Space))  // Presionar espacio para simular daño
        {
            AdjustHealth(-10f);  // Disminuir 10 unidades de salud
        }
    }

    public void AdjustHealth(float amount)
    {
        // Ajustar la salud actual y actualizar el valor del Slider
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBarSlider.value = currentHealth;

        // Para depuración, imprime el valor actual de salud en la consola
        Debug.Log("Adjusted Health: " + currentHealth);
        Debug.Log("Health Bar Slider Value: " + healthBarSlider.value);
    }
}
