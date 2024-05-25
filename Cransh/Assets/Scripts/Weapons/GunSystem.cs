using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    //Stats de las armas
    public int weaponDamage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //Verificadores
    bool isShooting, readyToShoot, isReloading;

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;


    //Funcion para registrar los inputs del mouse y disparar
    private void MyInput()
    {
        if (allowButtonHold)  isShooting = Input.GetKey(KeyCode.Mouse0);
        else isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !isReloading) Reload();
        
        if (readyToShoot && isShooting &&  !isReloading && bulletsLeft > 0)
        {
            Shoot();
        }
    }

    //Funcion para disparar
    private void Shoot()
    {
        readyToShoot = false;
        bulletsLeft--;

        //Spread
        float x = Random.Range(-spread,spread);
        float y = Random.Range(-spread, spread);
        

        //Disparar
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
            {
                rayHit.collider.GetComponent<ShootingAi>().TakenDamage(weaponDamage); 
            }
        }

        Invoke("ResetShoot", timeBetweenShooting);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    //Funcion para recargar
    private void Reload()
    {
        
    }

}
