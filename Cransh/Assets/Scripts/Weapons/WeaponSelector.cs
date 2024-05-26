using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class WeaponSelector : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction changeNextWeapon;
    private InputAction changePreviousWeapon;
    


    public Transform weaponHoldPoint; // Punto donde se sujetará el arma
    public GunSystem[] weapons; // Lista de armas disponibles
    public AudioSource audioSource; // Fuente de audio para los sonidos de disparo
    private int currentWeaponIndex = 0; // Índice del arma actual
    private GameObject currentWeaponInstance;
    public TextMeshProUGUI text;

    private void Awake()
    {
        playerInput = new PlayerInput();
        changeNextWeapon = playerInput.OnFoot.ChangeNextWeapon;
        changePreviousWeapon = playerInput.OnFoot.ChangePreviousWeapon;
        changeNextWeapon.performed += ctx => ChangeNextWeapon();
        changePreviousWeapon.performed += ctx => ChangePreviousWeapon();

    }

    private void OnEnable()
    {
        playerInput.OnFoot.Enable();
    }

    private void OnDisable()
    {
        playerInput.OnFoot.Disable();
    }
   

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
        //Verifica que no se este intentando cambiar de arma con la rueda del Mouse
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            //Verifica que no se este recargando mientras se quiere cambiar de arma
            if (!weapons[currentWeaponIndex].GetComponent<GunSystem>().getIsReloading())
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
    }
    void ChangeNextWeapon()
    {
        if (!weapons[currentWeaponIndex].GetComponent<GunSystem>().getIsReloading())
        {
            weapons[currentWeaponIndex].GetComponent<GunSystem>().enabled = false;
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
            EquipWeapon(weapons[currentWeaponIndex]);
        }
        else
        {
            return;
        }
    }
    void ChangePreviousWeapon()
    {
        if (!weapons[currentWeaponIndex].GetComponent<GunSystem>().getIsReloading())
        {
            weapons[currentWeaponIndex].GetComponent<GunSystem>().enabled = false;
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Length - 1;
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
        weapons[currentWeaponIndex].GetComponent<GunSystem>().UpdateAmmoUI();

    }

    public void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Length >= 1)
        {
            currentWeaponIndex = 0;
            EquipWeapon(weapons[currentWeaponIndex]);
            weapons[currentWeaponIndex].GetComponent<GunSystem>().UpdateAmmoUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length >= 2)
        {
            currentWeaponIndex = 1;
            EquipWeapon(weapons[currentWeaponIndex]);
            weapons[currentWeaponIndex].GetComponent<GunSystem>().UpdateAmmoUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Length >= 3)
        {
            currentWeaponIndex = 2;
            EquipWeapon(weapons[currentWeaponIndex]);
            weapons[currentWeaponIndex].GetComponent<GunSystem>().UpdateAmmoUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && weapons.Length >= 4)
        {
            currentWeaponIndex = 3;
            EquipWeapon(weapons[currentWeaponIndex]);
            weapons[currentWeaponIndex].GetComponent<GunSystem>().UpdateAmmoUI();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && weapons.Length >= 5)
        {
            currentWeaponIndex = 4;
            EquipWeapon(weapons[currentWeaponIndex]);
            weapons[currentWeaponIndex].GetComponent<GunSystem>().UpdateAmmoUI();
        }
    }
}



