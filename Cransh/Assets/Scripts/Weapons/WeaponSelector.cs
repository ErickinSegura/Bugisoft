using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public Transform weaponHoldPoint; // Punto donde se sujetará el arma
    public GunSystem[] weapons; // Lista de armas disponibles
    public AudioSource audioSource; // Fuente de audio para los sonidos de disparo
    private int currentWeaponIndex = 0; // Índice del arma actual
    private GameObject currentWeaponInstance;

    void Start()
    {
        EquipWeapon(weapons[currentWeaponIndex]);
    }

    void HandleWeaponSwitch()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            if (scroll > 0)
            {
                currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
            }
            else if (scroll < 0)
            {
                currentWeaponIndex--;
                if (currentWeaponIndex < 0)
                {
                    currentWeaponIndex = weapons.Length - 1;
                }
            }
            EquipWeapon(weapons[currentWeaponIndex]);
        }
    }

    public void EquipWeapon(GunSystem selectedWeapon)
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
        }

        currentWeaponInstance = Instantiate(selectedWeapon.weaponModel, weaponHoldPoint.position, weaponHoldPoint.rotation);
        currentWeaponInstance.transform.parent = weaponHoldPoint;
    }

    public void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Length >= 1)
        {
            currentWeaponIndex = 0;
            EquipWeapon(weapons[currentWeaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length >= 2)
        {
            currentWeaponIndex = 1;
            EquipWeapon(weapons[currentWeaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Length >= 3)
        {
            currentWeaponIndex = 2;
            EquipWeapon(weapons[currentWeaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && weapons.Length >= 4)
        {
            currentWeaponIndex = 3;
            EquipWeapon(weapons[currentWeaponIndex]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && weapons.Length >= 5)
        {
            currentWeaponIndex = 4;
            EquipWeapon(weapons[currentWeaponIndex]);
        }
    }
}



