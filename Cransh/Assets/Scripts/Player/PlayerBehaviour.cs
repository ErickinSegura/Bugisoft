using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public Slider[] healthBars;
    private int health = 500;
    public Image damage;

    void Start()
    {
        // Inicializar los sliders de salud
        UpdateHealthBars();
        damage.enabled = false;
    }

    public void TakenDamage(int damage)
    {
        if (health <= 0)
        {
            // AQUÍ HAZ ALGO DON ERICK
        }
        if (health > 0)
        {
            int previousHealth = health;
            health = Mathf.Max(health - damage, 0); // Asegurarse de que la salud no sea negativa
            StartCoroutine(UpdateHealthBarsGradually(previousHealth, health));
            StartCoroutine(ShowDamage());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            TakenDamage(100);
        }
    }

    private void UpdateHealthBars()
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            healthBars[i].value = (health - (i * 100)) / 100f;
        }
    }

    private IEnumerator ShowDamage()
    {
        damage.enabled = true;
        yield return new WaitForSeconds(3); // Esperar 3 segundos
        damage.enabled = false;
    }

    private IEnumerator UpdateHealthBarsGradually(int fromHealth, int toHealth)
    {
        float elapsedTime = 0f;
        float duration = 2f;
        int startBarIndex = fromHealth / 100;
        int endBarIndex = toHealth / 100;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            for (int i = startBarIndex - 1; i >= endBarIndex; i--)
            {
                if (i == startBarIndex)
                {
                    healthBars[i].value = Mathf.Lerp(fromHealth % 100 / 100f, 0, percentageComplete);
                }
                else if (i == endBarIndex)
                {
                    healthBars[i].value = Mathf.Lerp(1, toHealth % 100 / 100f, percentageComplete);
                }
                else
                {
                    healthBars[i].value = Mathf.Lerp(1, 0, percentageComplete);
                }
            }
            yield return null;
        }

        // Asegurarse de que los valores finales sean correctos
        for (int i = startBarIndex - 1; i >= endBarIndex; i--)
        {
            if (i == endBarIndex)
            {
                healthBars[i].value = toHealth % 100 / 100f;
            }
            else
            {
                healthBars[i].value = 0;
            }
        }
    }
}
