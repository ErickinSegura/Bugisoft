using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStartBattle : MonoBehaviour
{
    public GameObject[] EnemiesPrefab;  // Prefabs de los enemigos a generar
    public int numberOfEnemies;         // Número de enemigos a generar
    public float minColliderSize = 20f;  // Tamaño mínimo del collider para activar la batalla

    public bool isBattleActive;         // Indica si la batalla está activa
    public GameObject wallPrefab;       // Prefab de la pared

    public static DungeonStartBattle instance;     // Instancia de la clase

    private bool isWallsDestroyed = false;

    public Vector3 manualRotationHorizontal = Vector3.zero; // Rotación manual para paredes horizontales
    public Vector3 manualRotationVertical = Vector3.zero; // Rotación manual para paredes verticales

    private bool isPlayerInside = false; // Para verificar si el jugador está dentro del collider
    public float activationDistance = 5f;

    private Transform playerTransform; // Referencia al transform del jugador


    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (GetComponent<BoxCollider>().size.magnitude < minColliderSize)
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if (isBattleActive)
        {
            CheckEnemies();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isPlayerInside)
        {
            Debug.Log("Player is inside and space key pressed");
            BattleEnd();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player detected, starting battle");
            isPlayerInside = true;  // Mover esta línea antes de generar enemigos y paredes
            playerTransform = other.transform;

            // Empezar una nueva corrutina para verificar la distancia
            StartCoroutine(CheckPlayerDistanceAndStartBattle());
        }
    }

    private IEnumerator CheckPlayerDistanceAndStartBattle()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 colliderCenter = collider.bounds.center;
        Vector3 colliderSize = collider.size;

        // Esperar hasta que el jugador esté dentro de la distancia de activación
        while (true)
        {
            Vector3 playerPosition = playerTransform.position;
            float distanceToLeft = Mathf.Abs(playerPosition.x - (colliderCenter.x - colliderSize.x / 2));
            float distanceToRight = Mathf.Abs(playerPosition.x - (colliderCenter.x + colliderSize.x / 2));
            float distanceToTop = Mathf.Abs(playerPosition.z - (colliderCenter.z + colliderSize.z / 2));
            float distanceToBottom = Mathf.Abs(playerPosition.z - (colliderCenter.z - colliderSize.z / 2));

            if (distanceToLeft > activationDistance && distanceToRight > activationDistance &&
                distanceToTop > activationDistance && distanceToBottom > activationDistance)
            {
                break;
            }

            yield return null; // Esperar un frame antes de volver a comprobar
        }

        GenerateEnemiesInArea();
        GenerateWalls();
    }

    private void GenerateEnemiesInArea()
    {
        isBattleActive = true;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = GenerateRandomPositionInsideCollider();
            GameObject randomPrefab = EnemiesPrefab[Random.Range(0, EnemiesPrefab.Length)];
            GameObject enemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity);
            enemy.transform.parent = transform;
        }
    }

    private void GenerateWalls()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Vector3 colliderSize = collider.size;
        Vector3 colliderCenter = transform.TransformPoint(collider.center);

        // Calcular las posiciones de las esquinas del BoxCollider en el espacio global
        Vector3 topLeft = colliderCenter + transform.TransformVector(new Vector3(-colliderSize.x / 2, 0, colliderSize.z / 2));
        Vector3 topRight = colliderCenter + transform.TransformVector(new Vector3(colliderSize.x / 2, 0, colliderSize.z / 2));
        Vector3 bottomLeft = colliderCenter + transform.TransformVector(new Vector3(-colliderSize.x / 2, 0, -colliderSize.z / 2));
        Vector3 bottomRight = colliderCenter + transform.TransformVector(new Vector3(colliderSize.x / 2, 0, -colliderSize.z / 2));

        // Crear las paredes
        InstantiateWall(topLeft, topRight, colliderSize.y, true); // Pared superior
        InstantiateWall(topRight, bottomRight, colliderSize.y, false); // Pared derecha
        InstantiateWall(bottomRight, bottomLeft, colliderSize.y, true); // Pared inferior
        InstantiateWall(bottomLeft, topLeft, colliderSize.y, false); // Pared izquierda
    }

    private void InstantiateWall(Vector3 start, Vector3 end, float height, bool isHorizontal)
    {
        Vector3 position = (start + end) / 2;

        Debug.Log("Pos: " + position);

        Vector3 direction = end - start;
        Quaternion rotation;
        Vector3 scale;

        if (isHorizontal)
        {
            // Aplicar rotación manual para paredes horizontales
            rotation = Quaternion.Euler(manualRotationHorizontal);
            scale = new Vector3(direction.magnitude * 5, height, 0.1f);
            Debug.Log("hor " + scale);
        }
        else
        {
            // Aplicar rotación manual para paredes verticales
            rotation = Quaternion.Euler(manualRotationVertical);
            scale = new Vector3(0.1f, height, direction.magnitude);
            Debug.Log("ver " + scale);

        }

        if(isHorizontal)
        {
            if(((start.x + end.x) / 2) > start.x) position = start;
            else position = end;
        }

        GameObject wall = Instantiate(wallPrefab, position, rotation, transform);
        wall.transform.localScale = scale;
        wall.tag = "Wall";
    }

    private Vector3 GenerateRandomPositionInsideCollider()
    {
        Vector3 colliderSize = GetComponent<BoxCollider>().size;

        float randomX = Random.Range(-colliderSize.x / 2f, colliderSize.x / 2f);
        float randomZ = Random.Range(-colliderSize.z / 2f, colliderSize.z / 2f);

        Vector3 colliderPosition = transform.position;
        Vector3 randomPosition = colliderPosition + new Vector3(randomX, 0f, randomZ);

        return randomPosition;
    }

    public void CheckEnemies()
    {
        if (transform.childCount <= 4)
        {
            BattleEnd();
        }
    }

    public void BattleEnd()
    {
        Debug.Log("Battle End");
        if (!isWallsDestroyed)
        {
            DestroyWalls();
            isWallsDestroyed = true;
        }
        isBattleActive = false;
    }

    private void DestroyWalls()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Wall"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}
