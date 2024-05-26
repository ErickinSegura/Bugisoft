using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction shootAction;
    private InputAction reloadAction;
    private InputAction cancelingReload;

    //Stats de las armas
    public int weaponDamage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //Sonido y modelo del arma
    public AudioClip shootingSound;     // Sonido de disparo
    public GameObject weaponModel;      // Apariencia del arma
    public AudioSource audioSource;     // Lugar de donde se disparan las balas

    //Verificadores
    bool isShooting, readyToShoot, isReloading, cancelReload;

    //Referencia
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        cancelReload = false;

        playerInput = new PlayerInput();
        shootAction = playerInput.OnFoot.Shoot;
        reloadAction = playerInput.OnFoot.Reload;
        cancelingReload = playerInput.OnFoot.CancelReloading;
        
        shootAction.performed += ctx => StartShooting();
        shootAction.canceled += ctx => StopShooting();
        reloadAction.performed += ctx => Reload();
        cancelingReload.performed += ctx => CancelReload();
    }

    private void OnEnable()
    {
        playerInput.OnFoot.Enable();
    }

    private void OnDisable()
    {
        playerInput.OnFoot.Disable();
    }

    private void OnDestroy()
    {
        shootAction.performed -= ctx => StartShooting();
        shootAction.canceled -= ctx => StopShooting();
        reloadAction.performed -= ctx => Reload();
        cancelingReload.performed -= ctx => CancelReload();
    }

    private void Update()
    {
        MyInput();
    }

    //Función para registrar los inputs del player y disparar
    private void MyInput()
    {
        if (readyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot(audioSource);
        }
    }

    private void StartShooting()
    {
        isShooting = true;
    }

    private void StopShooting()
    {
        isShooting = false;
    }

    //Función para disparar
    private void Shoot(AudioSource audioSource)
    {
        readyToShoot = false;
        bulletsLeft--;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //Disparar
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<ShootingAi>().TakenDamage(weaponDamage);
            }
            else if (rayHit.collider.CompareTag("Player"))
            {
                rayHit.collider.GetComponent<PlayerBehaviour>().TakenDamage(weaponDamage);
            }
        }

        audioSource.PlayOneShot(shootingSound);

        //CamShaker
        camShake.Shake(camShakeDuration, camShakeMagnitude);

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShoot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    //Función para recargar
    private void Reload()
    {
        if (bulletsLeft < magazineSize && !isReloading && !cancelReload)
        {
            isReloading = true;
            cancelReload = false;
            Invoke("reloadFinished", reloadTime);
            Debug.Log("Recargado");
        }
    }

    private void CancelReload()
    {
        if (isReloading && !cancelReload) 
        {
            isReloading = false;
            cancelReload = true;
            CancelInvoke("reloadFinished");
            cancelReload = false;
            Debug.Log("Recarga cancelada");
        }
    }

    private void reloadFinished()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    public int getBulletsLeft()
    {
        return bulletsLeft;
    }

    public bool getIsReloading()
    {
        return isReloading;
    }
}
