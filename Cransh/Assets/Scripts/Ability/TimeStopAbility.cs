using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeStopAbility : MonoBehaviour
{
    public Slider abilitySlider; // Slider que muestra el progreso de la habilidad
    public float abilityDuration = 5f; // Duración de la habilidad en segundos
    private bool abilityActive = false; // Indica si la habilidad está activa
    private float abilityCooldown = 10f; // Tiempo de recarga de la habilidad
    private float cooldownTimer = 0f; // Temporizador de recarga

    public Image filtro; // Imagen del filtro para indicar la habilidad activa
    private List<StoppedBullet> stoppedBullets = new List<StoppedBullet>(); // Lista de balas detenidas

    // Colores para los diferentes estados
    private Color abilityActiveColor = new Color(0x76 / 255f, 0x00 / 255f, 0x0E / 255f); // #76000E
    private Color cooldownColor = new Color(0xFF / 255f, 0xED / 255f, 0x00 / 255f); // #FFED00
    private Color readyColor = new Color(0x00 / 255f, 0xFF / 255f, 0x94 / 255f); // #00FF94

    private void Start()
    {
        filtro.enabled = false;
        abilitySlider.fillRect.GetComponent<Image>().color = readyColor; // Inicialmente el color de la barra es verde
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && abilitySlider.value == 1 && !abilityActive)
        {
            StartCoroutine(ActivateAbility());
        }

        if (!abilityActive && cooldownTimer > 0)
        {
            cooldownTimer -= Time.unscaledDeltaTime; // Usar Time.unscaledDeltaTime en lugar de Time.deltaTime
            abilitySlider.value = 1 - (cooldownTimer / abilityCooldown);
            abilitySlider.fillRect.GetComponent<Image>().color = cooldownColor; // Cambiar el color a amarillo durante la recarga
        }

        if (!abilityActive && cooldownTimer <= 0 && abilitySlider.value != 1)
        {
            abilitySlider.fillRect.GetComponent<Image>().color = readyColor; // Cambiar el color a verde cuando esté listo
        }

        if (!abilityActive && cooldownTimer <= 0 && abilitySlider.value == 1)
        {
            abilitySlider.fillRect.GetComponent<Image>().color = readyColor; // Cambiar el color a verde cuando esté completamente cargado
        }
    }

    private IEnumerator ActivateAbility()
    {
        filtro.enabled = true;
        abilityActive = true;
        abilitySlider.value = 1;
        abilitySlider.fillRect.GetComponent<Image>().color = abilityActiveColor; // Cambiar el color a rojo durante la habilidad

        float elapsedTime = 0f;
        while (elapsedTime < abilityDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            abilitySlider.value = 1 - (elapsedTime / abilityDuration);
            yield return null;
        }

        // Reanudar el tiempo
        filtro.enabled = false;
        abilityActive = false;
        cooldownTimer = abilityCooldown;

        // Mover las balas detenidas
        foreach (var bullet in stoppedBullets)
        {
            bullet.MoveBulletAlongLine(1f); // Ajusta la duración del movimiento para que sea visible
        }

        stoppedBullets.Clear(); // Limpiar la lista después de mover las balas
    }

    public bool IsAbilityActive()
    {
        return abilityActive;
    }

    public void AddStoppedBullet(StoppedBullet bullet)
    {
        stoppedBullets.Add(bullet);
    }
}
