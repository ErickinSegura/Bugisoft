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

        // Generar paredes alrededor del collider



    }

    public void Update()
    {
        if (isBattleActive)
        {
            CheckEnemies();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Battle Start");
            GenerateEnemiesInArea();

            GenerateWalls();
        }
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
        Vector3 colliderCenter = collider.center;

        // Determinar la orientación del pasillo
        bool isHorizontal = colliderSize.x > colliderSize.z;

        // Calcular la mitad del tamaño de la pared
        Vector3 wallSize = isHorizontal ? new Vector3(colliderSize.x, colliderSize.y, 0.1f) : new Vector3(0.1f, colliderSize.y, colliderSize.z);

        // Instanciar las paredes dentro del objeto padre
        for (int i = 0; i < 2; i++)
        {
            Vector3 wallPosition;
            Quaternion wallRotation;

            if (isHorizontal)
            {
                float xPos = colliderCenter.x + (i == 0 ? -colliderSize.x / 2f : colliderSize.x / 2f);
                wallPosition = new Vector3(xPos, colliderCenter.y, colliderCenter.z);
                wallRotation = Quaternion.identity; // Sin rotación
            }
            else
            {
                float zPos = colliderCenter.z + (i == 0 ? -colliderSize.z / 2f : colliderSize.z / 2f);
                wallPosition = new Vector3(colliderCenter.x, colliderCenter.y, zPos);
                wallRotation = Quaternion.Euler(0, 90, 0); // Girar verticalmente
            }

            GameObject wall = Instantiate(wallPrefab, wallPosition, wallRotation, transform);
            wall.transform.localScale = wallSize;
        }
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
            BatlleEnd();
        }
    }

    public void BatlleEnd()
    {
        Debug.Log("Battle End");
        Destroy(gameObject);
    }
}