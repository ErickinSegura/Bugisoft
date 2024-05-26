using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucarachaBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab de la bala
    public float bulletSpeed = 5f; // Velocidad de las balas
    public int bulletCount = 10; // Número de balas a generar
    public float fireRate = 2f; // Tiempo entre cada generación de balas

    private float nextFireTime;

    void Start()
    {
        nextFireTime = Time.time + fireRate;
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            GenerateBullets();
            nextFireTime = Time.time + fireRate;
        }
    }

    void GenerateBullets()
    {
        float angleStep = 360f / bulletCount;
        float angle = 0f;

        for (int i = 0; i < bulletCount; i++)
        {
            float bulletDirX = transform.position.x + Mathf.Sin(angle * Mathf.Deg2Rad);
            float bulletDirZ = transform.position.z + Mathf.Cos(angle * Mathf.Deg2Rad);

            Vector3 bulletMoveVector = new Vector3(bulletDirX, transform.position.y, bulletDirZ);
            Vector3 bulletDir = (bulletMoveVector - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = bulletDir * bulletSpeed;

            angle += angleStep;
        }
    }
}
