using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public GameObject player;

    public LayerMask boundaryLayer;


    public int depth = 5;
    public int width = 65;
    public int height = 65;
    public float scale = 5f;
    float offsetX = 0;
    float offsetY = 0;
    float offsetFactor;


    Vector3 spawnRadSize;
    Vector3 elementSpawnRadSize;
    Vector3 elementSpawnPoint;
    Vector3 minTerrainBounds;
    Vector3 maxTerrainBounds;


    const int maxEnemies = 15;
    const int maxObstacles = 10;
    int enemiesActive;
    int obstaclesActive;
    bool initialSpawnDone;
    public bool generationActive;
    Coroutine spawningCoroutineRunning;
    List<GameObject> enemies;
    List<GameObject> obstacles;
    int enemiesToSpawn;
    int obstaclesToSpawn;
    int generationDirection;

    public bool playerHasSpawned;

    public GameObject spawnerInfo;

	void Start () 
    {
        offsetX = Random.Range(-9999f, 9999f);
        offsetY = Random.Range(-9999f, 9999f);

        //Starting Speed of the player
        offsetFactor = 1f;

        spawnRadSize = new Vector3(50f, 5f, 50f);
        elementSpawnRadSize = new Vector3(5f, 3f, 5f);

        minTerrainBounds = gameObject.GetComponent<Collider>().bounds.min;
        maxTerrainBounds = gameObject.GetComponent<Collider>().bounds.max;

        minTerrainBounds = new Vector3 (minTerrainBounds.x + 5f, 50f, minTerrainBounds.z + 5f);
        maxTerrainBounds = new Vector3 (maxTerrainBounds.x - 5f, 50f, maxTerrainBounds.z - 5f);

        enemiesActive = 0;
        obstaclesActive = 0;
        initialSpawnDone = false;
        spawningCoroutineRunning = null;
        generationDirection = 0;

        playerHasSpawned = false;

        enemies = new List<GameObject>();
        obstacles = new List<GameObject>();
    }

	void FixedUpdate() 
    {
        Terrain terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        if (playerHasSpawned)
        {
            UpdateWorld();
        }        
    }
    
    TerrainData GenerateTerrain (TerrainData terrainData)
    {
        terrainData.heightmapResolution = width;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width-0; x++)
        {
            for (int y = 0; y < height-0; y++)
            {
                heights[x, y] = CalculaHeight(x, y);
            }
        }

        return heights;
    }

    float CalculaHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    void UpdateWorld()
    {
        Collider[] spawnVicinity;

        Vector3 elementOffset;

        if (!initialSpawnDone)
        {
            StartCoroutine(SpawnElements());
        }

        if ((enemiesActive == maxEnemies) && (obstaclesActive == maxObstacles))
        {
            initialSpawnDone = true;            
        }

        for (int i = 0; i < maxEnemies; i++)
        {
            if (spawnerInfo.transform.childCount > 0)
            {
                if (spawnerInfo.transform.GetChild(0).GetChild(i).gameObject.activeInHierarchy)
                {
                    enemies.Add(spawnerInfo.transform.GetChild(0).GetChild(i).gameObject);
                }
            }            
        }

        for (int j = 0; j < maxObstacles; j++)
        {
            if (spawnerInfo.transform.childCount > 1)
            {
                if (spawnerInfo.transform.GetChild(1).GetChild(j).gameObject.activeInHierarchy)
                {
                    obstacles.Add(spawnerInfo.transform.GetChild(1).GetChild(j).gameObject);
                }
            }
        }       

        enemiesToSpawn = maxEnemies - enemiesActive;
        obstaclesToSpawn = maxObstacles - obstaclesActive;
        
        spawnVicinity = Physics.OverlapBox(player.transform.position, spawnRadSize, Quaternion.identity, boundaryLayer);
        if(spawnVicinity.Length > 0)
        {
            foreach (Collider spawnCheck in spawnVicinity)
            {
                switch (spawnCheck.tag)
                {
                    case "TopGen":
                        offsetX += Time.deltaTime * offsetFactor; 
                        generationActive = true;
                        generationDirection = 1;

                        elementOffset = player.transform.position;
                        elementOffset.z -= offsetFactor * Time.deltaTime * (width/scale);
                        //player.transform.position = elementOffset;
                        Debug.Log(elementOffset);
                        break;
                    case "RightGen":
                        offsetY += Time.deltaTime * offsetFactor;
                        generationActive = true;
                        generationDirection = 2;

                        elementOffset = player.transform.position;
                        elementOffset.x -= offsetFactor * Time.deltaTime * (width/scale);
                        //player.transform.position = elementOffset;
                        Debug.Log(elementOffset);
                        break;
                    case "BottomGen":
                        offsetX -= Time.deltaTime * offsetFactor;
                        generationActive = true;
                        generationDirection = 3;

                        elementOffset = player.transform.position;
                        elementOffset.z += offsetFactor * Time.deltaTime * (width/scale);
                        //player.transform.position = elementOffset;
                        Debug.Log(elementOffset);
                        break;
                    case "LeftGen":
                        offsetY -= Time.deltaTime * offsetFactor;
                        generationActive = true;
                        generationDirection = 4;

                        elementOffset = player.transform.position;
                        elementOffset.x += offsetFactor * Time.deltaTime * (width/scale);
                        //player.transform.position = elementOffset;
                        Debug.Log(elementOffset);
                        break;
                }
                
                foreach (GameObject enemy in enemies)
                {
                    elementOffset = enemy.transform.position;
                    switch(generationDirection)
                    {
                        case 1:
                            elementOffset.z -= offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        case 2:
                            elementOffset.x -= offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        case 3:
                            elementOffset.z += offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        case 4:
                            elementOffset.x += offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        default:
                            break;
                    }                    
                    enemy.transform.position = elementOffset;
                    Debug.Log(elementOffset);
                }

                foreach (GameObject obstacle in obstacles)
                {
                    elementOffset = obstacle.transform.position;
                    switch(generationDirection)
                    {
                        case 1:
                            elementOffset.z -= offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        case 2:
                            elementOffset.x -= offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        case 3:
                            elementOffset.z += offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        case 4:
                            elementOffset.x += offsetFactor * Time.deltaTime * (width/scale);
                            break;
                        default:
                            break;
                    }
                    obstacle.transform.position = elementOffset;
                    Debug.Log(elementOffset);
                }
            }
        }

        else
        {
            generationActive = false;
            generationDirection = 0;                    
        }

        if (generationActive && spawningCoroutineRunning == null)
        {
            spawningCoroutineRunning = StartCoroutine(SpawnElements());
        }
    }

    IEnumerator SpawnElements()
    {
        Ray terrainElementRay;
        RaycastHit terrainElementRayInfo;
        RaycastHit terrainElementBoxInfo;
        bool spawnSuccess = false;

        while ((spawnSuccess == false) && ((enemiesToSpawn > 0) || (obstaclesToSpawn > 0)))
        {
            switch(generationDirection)
            {
                case 0:
                    elementSpawnPoint = new Vector3 (Mathf.Round(Random.Range(minTerrainBounds.x, maxTerrainBounds.x)), 50f, Mathf.Round(Random.Range(minTerrainBounds.z, maxTerrainBounds.z)));
                    break;
                case 1:
                    elementSpawnPoint = new Vector3 (Mathf.Round(Random.Range(minTerrainBounds.x, maxTerrainBounds.x)), 50f, Mathf.Round(Random.Range(maxTerrainBounds.z - 5f, maxTerrainBounds.z)));
                    break;
                case 2:
                    elementSpawnPoint = new Vector3 (Mathf.Round(Random.Range(maxTerrainBounds.x - 5f, maxTerrainBounds.x)), 50f, Mathf.Round(Random.Range(minTerrainBounds.z, maxTerrainBounds.z)));
                    break;
                case 3:
                    elementSpawnPoint = new Vector3 (Mathf.Round(Random.Range(minTerrainBounds.x, maxTerrainBounds.x)), 50f, Mathf.Round(Random.Range(minTerrainBounds.z, minTerrainBounds.z + 5f)));
                    break;
                case 4:
                    elementSpawnPoint = new Vector3 (Mathf.Round(Random.Range(minTerrainBounds.x, minTerrainBounds.x + 5f)), 50f, Mathf.Round(Random.Range(minTerrainBounds.z,maxTerrainBounds.z)));
                    break;
                default:
                    break;
            }    

            terrainElementRay = new Ray(elementSpawnPoint, Vector3.down);
            if (Physics.Raycast(terrainElementRay, out terrainElementRayInfo, 50f))
            {
                if (terrainElementRayInfo.collider.tag == "Terrain")
                {
                    Debug.DrawLine(elementSpawnPoint, terrainElementRayInfo.point, Color.red);

                    if (Physics.BoxCast(elementSpawnPoint, elementSpawnRadSize, Vector3.down, out terrainElementBoxInfo, Quaternion.identity, 50f))
                    {
                        if ((terrainElementBoxInfo.collider.tag != "Enemy") && (terrainElementBoxInfo.collider.tag != "Obstacle"))
                        {
                            if (enemiesToSpawn >= obstaclesToSpawn)
                            {
                                IPoolable tempEnemy = ObjectPooler.Instance.SpawnFromPool("Enemy", new Vector3 (terrainElementRayInfo.point.x, terrainElementRayInfo.point.y + 0.1f, terrainElementRayInfo.point.z), Quaternion.Euler (new Vector3(0, Random.Range(0, 361), 0))).GetComponent<IPoolable>();

                                tempEnemy.OnReturnToPool = DecreaseActiveEnemiesCount;
                                enemiesActive++;
                            }
                                
                            else
                            {
                                IPoolable tempObs = ObjectPooler.Instance.SpawnFromPool("Obstacle", new Vector3 (terrainElementRayInfo.point.x, terrainElementRayInfo.point.y + 2.5f, terrainElementRayInfo.point.z), Quaternion.identity).GetComponent<IPoolable>();

                                tempObs.OnReturnToPool = DecreaseActiveObstaclesCount;
                                obstaclesActive++;
                            }                                

                            spawnSuccess = true;
                        }
                    }
                }
            }
            else
            {
                Debug.DrawLine(elementSpawnPoint, Vector3.down * 50f, Color.green);
            }
        }

        spawningCoroutineRunning = null;
        yield return null;        
    }

    void DecreaseActiveEnemiesCount(GameObject enemyToRemove)
    {
        enemies.Remove(enemyToRemove);
        enemiesActive--;
    }
 
    void DecreaseActiveObstaclesCount(GameObject obstacleToRemove)
    {
        obstacles.Remove(obstacleToRemove);
        obstaclesActive--;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(player.transform.position, spawnRadSize*2);
    }
}
