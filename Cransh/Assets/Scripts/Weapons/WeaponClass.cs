using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponClass : ScriptableObject
{
    public string weaponName;           // Nombre del arma
    public GameObject weaponModel;      // Apariencia del arma
    public int damage;                  // Daño que causa el arma
    public float fireRate;              // Cadencia de disparo (disparos por segundo)
    public int chargerammo;             // Cantidad de balas balas en el cargador arma
    public int actualammo;              // Cantidad de balas del cargador actual
    public int reloadtime;              // Tiempo para recargar
    public string upgrade;              // Mejora del arma
    public AudioClip shootingSound;     // Sonido de disparo
    private bool isReloading;           // Estado de recarga
    private float nextTimeToFire = 0f;  // Tiempo para el siguiente disparo

    // Método para disparar
    public virtual void Shoot(AudioSource audioSource)
    {
        if (isReloading)
        {
            Debug.Log("Reloading...");
            return;
        }

        if (actualammo <= 0)
        {
            Debug.Log("Out of ammo!");
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            Debug.Log("Shooting weapon: " + weaponName);
            actualammo--;
            nextTimeToFire = Time.time + 1f / fireRate;

            if (shootingSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(shootingSound);
            }
        }
    }

    // Método para recargar
    public virtual IEnumerator Reload()
    {
        if (isReloading)
        {
            yield break;
        }

        isReloading = true;
        Debug.Log("Reloading weapon: " + weaponName);
        yield return new WaitForSeconds(reloadtime);
        actualammo = chargerammo;
        isReloading = false;
    }

    // Método para obtener el estado de recarga
    public bool IsReloading()
    {
        return isReloading;
    }
}

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public int upgradeValue;
}