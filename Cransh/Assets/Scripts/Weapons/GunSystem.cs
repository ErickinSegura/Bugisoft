using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GunSystem : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction shootAction;
    private InputAction reloadAction;
    private InputAction cancelingReload;

    // Stats de las armas
    public int weaponDamage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // Sonido y modelo del arma
    public AudioClip shootingSound;     // Sonido de disparo
    public AudioClip reloadSound;
    public GameObject weaponModel;      // Apariencia del arma
    public AudioSource audioSource;     // Lugar de donde se disparan las balas

    // Verificadores
    bool isShooting, readyToShoot, isReloading, cancelReload;

    // Referencia
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    // Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;
    public CameraShake camShake;
    public float camShakeMagnitude, camShakeDuration;

    // Referencia al slider de la barra de munici�n
    public Slider ammoSlider;
    // Referencia al Image del Fill del slider para cambiar su color
    public Image ammoFillImage;

    // Colores para la recarga
    public Color reloadColor = new Color(0xAD / 255f, 0x21 / 255f, 0x00 / 255f); // #AD2100
    Color white = Color.white;
    public Color normalColor = new Color(0f, 250f / 255f, 255f / 255f); // #00FAFF

    private TimeStopAbility timeStopAbility; // Referencia a la habilidad de detener el tiempo
    public GameObject bulletPrefab; // Prefab de la bala para mostrarla detenida

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        cancelReload = false;
        UpdateAmmoUI();
        timeStopAbility = FindObjectOfType<TimeStopAbility>(); // Obtener la referencia a TimeStopAbility

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

    // Funci�n para registrar los inputs del player y disparar
    private void MyInput()
    {
        if (readyToShoot && isShooting && !isReloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot(audioSource);
        }
    }

    private void Start()
    {
        UpdateAmmoUI();
    }

    private void StartShooting()
    {
        isShooting = true;
    }

    private void StopShooting()
    {
        isShooting = false;
    }

    // Funci�n para disparar
    private void Shoot(AudioSource audioSource)
    {
        readyToShoot = false;
        bulletsLeft--;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        // Disparar
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);
            if (rayHit.collider.CompareTag("Enemy"))
            {
                //rayHit.collider.GetComponent<ShootingAi>().TakenDamage(weaponDamage);
            }
            else if (rayHit.collider.CompareTag("Player"))
            {
                rayHit.collider.GetComponent<PlayerBehaviour>().TakenDamage(weaponDamage);
            }

            // Crear bala parada si la habilidad est� activa
            if (timeStopAbility != null && timeStopAbility.IsAbilityActive())
            {
                GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
                var stoppedBullet = bullet.GetComponent<StoppedBullet>();
                stoppedBullet.Initialize(direction, rayHit.point);
                timeStopAbility.AddStoppedBullet(stoppedBullet);
            }
        }

        audioSource.PlayOneShot(shootingSound);

        // CamShaker
        camShake.Shake(camShakeDuration, camShakeMagnitude);

        // Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));

        GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Destroy(flash, 0.5f); // Destruye el muzzle flash despu�s de 0.5 segundos (ajusta el tiempo seg�n sea necesario)

        bulletsLeft-=1;
        bulletsShot-=1;

        UpdateAmmoUI();

        Invoke("ResetShoot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShoot()
    {
        readyToShoot = true;
    }

    // Funci�n para recargar
    private void Reload()
    {
        if (bulletsLeft < magazineSize && !isReloading && !cancelReload)
        {
            isReloading = true;
            cancelReload = false;
            audioSource.PlayOneShot(reloadSound);
            Invoke("reloadFinished", reloadTime);
            Debug.Log("Recargado");

            // Actualizar UI durante la recarga
            StartCoroutine(ReloadUIUpdate());

        }
    }

    private void CancelReload()
    {
        if (isReloading && !cancelReload)
        {
            audioSource.Stop();
            isReloading = false;
            cancelReload = true;
            CancelInvoke("reloadFinished");
            cancelReload = false;
            Debug.Log("Recarga cancelada");
            StopAllCoroutines();
            UpdateAmmoUI();
            ammoFillImage.color = normalColor;
        }
    }

    private void reloadFinished()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
        ;
        UpdateAmmoUI();
    }

    private IEnumerator ReloadUIUpdate()
    {
        float elapsedTime = 0;
        ammoFillImage.color = white;
        // ammoFillImage.color = reloadColor;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            ammoSlider.value = Mathf.Lerp(0, 1, elapsedTime / reloadTime);
            yield return null; // Esperar un frame
        }

        // ammoFillImage.color = white;
        FinishReload();
        UpdateAmmoUI();
    }

    private void FinishReload()
    {
        ammoFillImage.color = white;
        ammoSlider.fillRect.GetComponent<Image>().color = normalColor;
    }

    public void UpdateAmmoUI()
    {
        if (ammoSlider != null)
        {
            ammoSlider.value = (float)bulletsLeft / magazineSize;
        }
    }

    public int getBulletsLeft()
    {
        return bulletsLeft;
    }

    public bool getIsReloading()
    {
        return isReloading;
    }

    public void changeWeaponPositionOnReload()
    {

    }
}
