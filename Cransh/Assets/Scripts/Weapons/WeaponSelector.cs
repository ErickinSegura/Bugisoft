using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelector : MonoBehaviour
{
    public Transform weaponHoldPoint; // Punto donde se sujetará el arma
    public GunSystem[] weapons; // Lista de armas disponibles
    public AudioSource audioSource; // Fuente de audio para los sonidos de disparo
    private int currentWeaponIndex = 0; // Índice del arma actual
    private GameObject currentWeaponInstance;
    public TextMeshProUGUI text;

    // Referencia al slider de la barra de munición
    public Slider ammoSlider;

    void Start()
    {
        EquipWeapon(weapons[currentWeaponIndex]);
    }

    private void Update()
    {
        HandleWeaponSwitch();
        weapons[currentWeaponIndex].GetComponent<GunSystem>().enabled = true;
        text.SetText(weapons[currentWeaponIndex].GetComponent<GunSystem>().getBulletsLeft() + " / " + 
            weapons[currentWeaponIndex].GetComponent<GunSystem>().magazineSize);
    }

    void HandleWeaponSwitch()
    {
        
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        { 
            weapons[currentWeaponIndex].GetComponent<GunSystem>().enabled = false;
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
        weapons[currentWeaponIndex].GetComponent<GunSystem>().enabled = true;

        // Pasar la referencia del slider al sistema de armas
        weapons[currentWeaponIndex].ammoSlider = ammoSlider;

        // Actualizar la barra de munición al cambiar de arma
        weapons[currentWeaponIndex].UpdateAmmoUI();
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



