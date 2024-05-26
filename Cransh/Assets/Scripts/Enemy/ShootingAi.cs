using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAi : MonoBehaviour
{
    public int enemyLife = 100;
    public float moveSpeed = 2.0f; // Velocidad de movimiento
    public float rotationSpeed = 5.0f; // Velocidad de rotación
    public float changeDirectionTime = 2.0f; // Tiempo para cambiar de dirección
    public float chaseRange = 10.0f; // Rango de persecución
    public float stopRange = 2.0f; // Rango máximo de acercamiento
    public float shootRange = 5.0f; // Rango de disparo
    public GameObject bulletPrefab; // Prefab de la bala
    public Transform bulletSpawnPoint; // Punto de aparición de la bala
    public float shootCooldown = 1.0f; // Tiempo entre disparos
    public float bulletSpeed = 20.0f; // Velocidad de la bala
    public float avoidDistance = 1.0f; // Distancia mínima para evitar a otros enemigos
    private Vector3 randomDirection; // Dirección aleatoria actual
    private float timer; // Temporizador para cambiar de dirección
    private Transform player; // Referencia al jugador
    private float shootTimer; // Temporizador para disparar
    private Vector3 offset; // Desplazamiento aleatorio

    private float bulletLifetime = 1.0f;

    void Start()
    {
        // Inicializar el temporizador y la dirección aleatoria
        timer = changeDirectionTime;
        shootTimer = shootCooldown;
        SetRandomDirection();

        // Encontrar al jugador en la escena (asegúrate de que el jugador tenga la etiqueta "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Generar un pequeño desplazamiento aleatorio
        SetRandomOffset();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango de persecución y fuera del rango máximo de acercamiento
        if (distanceToPlayer <= chaseRange && distanceToPlayer > stopRange)
        {
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= shootRange)
        {
            // Rotar hacia el jugador y disparar
            RotateTowardsPlayer();
            TryToShootPlayer();
        }
        else
        {
            // Actualizar el temporizador
            timer -= Time.deltaTime;

            // Si el temporizador llega a cero, cambiar de dirección
            if (timer <= 0)
            {
                SetRandomDirection();
                timer = changeDirectionTime;
            }

            // Mover al enemigo en la dirección aleatoria
            transform.Translate(randomDirection * moveSpeed * Time.deltaTime);
        }
    }

    public void TakenDamage(int damage)
    {
        enemyLife -= damage;
        if (enemyLife <= 0)
        {
            Debug.Log("Destruyelo");
            StartCoroutine(DestroyEnemy());
        }
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForEndOfFrame();

        // Destruir todos los hijos
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Destruir el objeto principal
        Destroy(gameObject);
    }

    private void SetRandomDirection()
    {
        // Generar una dirección aleatoria
        float randomX = Random.Range(-1.0f, 1.0f);
        float randomZ = Random.Range(-1.0f, 1.0f);
        randomDirection = new Vector3(randomX, 0, randomZ).normalized;
    }

    private void SetRandomOffset()
    {
        // Generar un desplazamiento aleatorio para evitar que los enemigos se empalmen
        float offsetX = Random.Range(-1.0f, 1.0f);
        float offsetZ = Random.Range(-1.0f, 1.0f);
        offset = new Vector3(offsetX, 0, offsetZ).normalized * avoidDistance;
    }

    private void MoveTowardsPlayer()
    {
        // Dirección hacia el jugador con un pequeño desplazamiento
        Vector3 directionToPlayer = (player.position + offset - transform.position).normalized;
        directionToPlayer.y = 0; // Asegurarse de que la dirección no afecte el eje Y

        // Rotar hacia el jugador
        RotateTowardsPlayer();

        // Mover hacia el jugador
        transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position + offset - transform.position).normalized;
        direction.y = 0; // Mantener el eje Y constante
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void TryToShootPlayer()
    {
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0)
        {
            // Disparar al jugador
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    private void Shoot()
    {
        // Instanciar una bala en el punto de aparición
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        // Asignar velocidad a la bala
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
        }

        // Destruir la bala después de un tiempo
        Destroy(bullet, bulletLifetime);
    }
}
