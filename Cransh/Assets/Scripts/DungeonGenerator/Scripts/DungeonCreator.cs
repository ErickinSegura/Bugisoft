﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal;
    public GameObject[] EnemiesPrefab;  // Arreglo de prefabs de objetos aleatorios
    public int numberOfEnemies;         // Número de objetos aleatorios a generar
    public GameObject[] propsPrefabs;         // Arreglo de prefabs de objetos
    public int numberOfProps;                 // Número de objetos a generar
    public GameObject[] rareSpawns;           // Arreglo de prefabs de objetos raros
    public int numberOfRareSpawns;            // Número de objetos raros a generar
    public Transform player;                 // Referencia al jugador
    public float minDistanceFromPlayer;      // Distancia mínima del jugador
    public GameObject Portal;          // Prefab de un objeto aleatorio
    public GameObject triggerPrefab;   // Prefab del trigger
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    List<Vector3> validFloorPositions;        // Posiciones válidas para generar objetos
    List<Vector3> roomCenters;                // Centros de habitaciones

    void Start()
    {
        //CreateDungeon();
    }

    void Awake()
{
    possibleDoorVerticalPosition = new List<Vector3Int>();
    possibleDoorHorizontalPosition = new List<Vector3Int>();
    possibleWallHorizontalPosition = new List<Vector3Int>();
    possibleWallVerticalPosition = new List<Vector3Int>();
    validFloorPositions = new List<Vector3>();
    roomCenters = new List<Vector3>();
    roomDataList = new List<RoomData>();
}


    public void CreateDungeon()
    {
        DestroyAllChildren();
        DugeonGenerator generator = new DugeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerMidifier,
            roomOffset,
            corridorWidth);

        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;

        // Inicializar todas las listas antes de usarlas
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        validFloorPositions = new List<Vector3>();  // Inicializar la lista de posiciones válidas
        roomCenters = new List<Vector3>();          // Inicializar la lista de centros de habitaciones
        roomDataList = new List<RoomData>();        // Inicializar la lista de datos de habitaciones

        for (int i = 0; i < listOfRooms.Count; i++)
        {
            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner);
        }

        CreateWalls(wallParent);
        //GenerateRandomObjects();  // Generar objetos aleatorios
        GenerateRandomProps();    // Generar objetos
        GenerateRareSpawns();     // Generar objetos raros
        GenerateObjectFarFromPlayer(); // Generar un objeto lejos del jugador
        GenerateRoomCenterTriggers(); // Generar triggers en el centro de las salas
    }


    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }

    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    // Estructura para almacenar el centro de la habitación y el tamaño del mesh
    private struct RoomData
    {
        public Vector3 Center;
        public Vector3 Size;

        public RoomData(Vector3 center, Vector3 size)
        {
            Center = center;
            Size = size;
        }

    }

    private List<RoomData> roomDataList = new List<RoomData>();

    private void GenerateRoomCenterTriggers()
    {
        foreach (var roomData in roomDataList)
        {
            Vector3 triggerPosition = new Vector3(roomData.Center.x, 0, roomData.Center.z);
            CreateTrigger(triggerPosition, roomData.Size);
        }
    }

    private void CreateTrigger(Vector3 position, Vector3 meshSize)
    {
        GameObject trigger = Instantiate(triggerPrefab, position, Quaternion.identity, transform);

        // Ajusta el tamaño del trigger según el tamaño del mesh
        BoxCollider collider = trigger.GetComponent<BoxCollider>();
        if (collider != null)
        {
            collider.size = meshSize;
        }

        // Resto de tu código...
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
        topLeftV,
        topRightV,
        bottomLeftV,
        bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
        0,
        1,
        2,
        2,
        1,
        3
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.GetComponent<MeshCollider>().sharedMesh = mesh; // Add MeshCollider to the floor
        dungeonFloor.transform.parent = transform;

        // Agregar posiciones válidas para objetos en el piso de la habitación
        for (int x = Mathf.CeilToInt(bottomLeftV.x) + 1; x < Mathf.FloorToInt(topRightV.x); x++)
        {
            for (int z = Mathf.CeilToInt(bottomLeftV.z) + 1; z < Mathf.FloorToInt(topRightV.z); z++)
            {
                Vector3 floorPosition = new Vector3(x, 0, z);
                validFloorPositions.Add(floorPosition);
            }
        }

        // Calcular el tamaño del mesh
        Vector3 meshSize = new Vector3(topRightCorner.x - bottomLeftCorner.x, 1, topRightCorner.y - bottomLeftCorner.y);

        // Calcular el centro de la habitación y agregarlo a la lista de datos de habitaciones
        Vector3 roomCenter = new Vector3((bottomLeftV.x + topRightV.x) / 2, 0, (bottomLeftV.z + topRightV.z) / 2);
        roomDataList.Add(new RoomData(roomCenter, meshSize));

        for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        {
            var wallPosition = new Vector3(row, 0, bottomLeftV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }
        for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        {
            var wallPosition = new Vector3(row, 0, topRightV.z);
            AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        }

        for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        {
            var wallPosition = new Vector3(bottomLeftV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
        for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        {
            var wallPosition = new Vector3(bottomRightV.x, 0, col);
            AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        }
    }


    private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        Vector3Int point = Vector3Int.CeilToInt(wallPosition);
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }



    private void GenerateRandomProps()
    {
        for (int i = 0; i < numberOfProps; i++)
        {
            if (validFloorPositions.Count == 0 || propsPrefabs.Length == 0) break;  // Salir si no hay posiciones válidas o prefabs disponibles

            int randomIndex = UnityEngine.Random.Range(0, validFloorPositions.Count);
            Vector3 randomPosition = validFloorPositions[randomIndex];
            GameObject randomPrefab = propsPrefabs[UnityEngine.Random.Range(0, propsPrefabs.Length)];

            // Generar una rotación aleatoria en uno de los cuatro ejes cardinales
            int randomRotation = UnityEngine.Random.Range(0, 4) * 90; // Puede ser 0, 90, 180 o 270 grados
            Quaternion randomOrientation = Quaternion.Euler(0, randomRotation, 0);

            Instantiate(randomPrefab, randomPosition, randomOrientation, transform);

            validFloorPositions.RemoveAt(randomIndex);  // Eliminar la posición para evitar duplicados
        }
    }

    private void GenerateRandomObjects()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            if (validFloorPositions.Count == 0 || EnemiesPrefab.Length == 0) break;  // Salir si no hay posiciones válidas o prefabs disponibles
            int randomIndex = UnityEngine.Random.Range(0, validFloorPositions.Count);
            Vector3 randomPosition = validFloorPositions[randomIndex];
            GameObject randomPrefab = EnemiesPrefab[UnityEngine.Random.Range(0, EnemiesPrefab.Length)];
            Instantiate(randomPrefab, randomPosition, Quaternion.identity, transform);
            validFloorPositions.RemoveAt(randomIndex);  // Eliminar la posición para evitar duplicados
        }
    }

    private void GenerateRareSpawns()
    {
        for (int i = 0; i < numberOfRareSpawns; i++)
        {
            if (validFloorPositions.Count == 0 || rareSpawns.Length == 0) break;  // Salir si no hay posiciones válidas o prefabs disponibles
            int randomIndex = UnityEngine.Random.Range(0, validFloorPositions.Count);
            Vector3 randomPosition = validFloorPositions[randomIndex];
            GameObject randomPrefab = rareSpawns[UnityEngine.Random.Range(0, rareSpawns.Length)];
            Instantiate(randomPrefab, new Vector3(randomPosition.x, 1, randomPosition.z), Quaternion.Euler(90, 0, 0), transform);
            validFloorPositions.RemoveAt(randomIndex);  // Eliminar la posición para evitar duplicados
        }
    }

    private void GenerateObjectFarFromPlayer()
    {
        // Filtrar las posiciones de centros de habitaciones que están lejos del jugador
        List<Vector3> farPositions = roomCenters.FindAll(position => Vector3.Distance(position, player.position) >= minDistanceFromPlayer);

        if (farPositions.Count == 0) return;  // Salir si no hay posiciones lo suficientemente lejos

        // Elegir una posición aleatoria entre las posiciones filtradas
        Vector3 spawnPosition = farPositions[UnityEngine.Random.Range(0, farPositions.Count)];

        // Elegir un prefab aleatorio para generar

        if (Portal != null)
        {
            Instantiate(Portal, spawnPosition, Quaternion.identity, transform);
        }
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
}