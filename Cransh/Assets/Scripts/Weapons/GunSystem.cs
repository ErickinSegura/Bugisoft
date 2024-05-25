using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    //Stats de las armas
    public int weaponDamage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //Sonido y modelo del arma
    public AudioClip shootingSound;     // Sonido de disparo
    public GameObject weaponModel;      // Apariencia del arma
    public AudioSource audioSource;
    //Verificadores
    bool isShooting, readyToShoot, isReloading;

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
    }
    private void Update()
    {
        MyInput();
        
    }

    //Funcion para registrar los inputs del mouse y disparar
    private void MyInput()
    {
        if (allowButtonHold)  isShooting = Input.GetKey(KeyCode.Mouse0);
        else isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading) Reload();
        
        if (readyToShoot && isShooting &&  !isReloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot(audioSource);
        }
    }

    //Funcion para disparar
    private void Shoot(AudioSource audioSource)
    {
        readyToShoot = false;
        bulletsLeft--;

        //Spread
        float x = Random.Range(-spread,spread);
        float y = Random.Range(-spread, spread);
        
        Vector3 direction = fpsCam.transform.forward + new Vector3(x,y,0);

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

    //Funcion para recargar
    private void Reload()
    {
        isReloading = true;
        Invoke("reloadFinished", reloadTime);
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

}
