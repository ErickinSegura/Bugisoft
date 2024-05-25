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
    
    // Método para disparar
    public virtual void Shoot()
    {
        Debug.Log("Shooting weapon: " + weaponName);
    }
}

[System.Serializable]
public class Upgrade
{
    public string upgradeName;
    public int upgradeValue;
}