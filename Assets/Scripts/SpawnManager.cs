using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    Transform playerSpawn;
    [SerializeField]
    GameObject player;

    [Header("Spawn Prefabs")]
    [Space(10)]
    [SerializeField]
    GameObject enemyPrefabA;
    [SerializeField]
    GameObject enemyPrefabB;
    [SerializeField]
    GameObject enemyPrefabC;
    [SerializeField]
    GameObject enemyPrefabD;

    [Header("Spawn Rates")]
    [Space(10)]
    [SerializeField]
    float secondsPerEnemySpawn = 3.0f;
    [SerializeField]
    [Range(.1f, 1f)]
    float reductionMultiplier = 0.95f;
    float elapsedSpawnRate = 0.0f;

    List<Transform> spawns;
    int curSpawnPoint = 0;

    float spawnRate;

    List<GameObject> enemiesSpawned;

    private void Start()
    {
        spawns = new List<Transform>();
        enemiesSpawned = new List<GameObject>();

        spawnRate = secondsPerEnemySpawn;

        foreach(Transform child in transform)
        {
            spawns.Add(child);
        }
    }


    private void Update()
    {
        elapsedSpawnRate += Time.deltaTime;

        //Spawn enemy
        if(elapsedSpawnRate >= spawnRate)
        {
            elapsedSpawnRate = 0.0f;

            if (curSpawnPoint >= spawns.Count)
            {
                curSpawnPoint = 0;
            }

            //Get spawn point
            Transform spawn = spawns[curSpawnPoint];      
            GameObject prefabToUse = enemyPrefabA;
            if(curSpawnPoint == 1)
            {
                prefabToUse = enemyPrefabB;
            }
            else if(curSpawnPoint == 2)
            {
                prefabToUse = enemyPrefabC;
            }
            else if (curSpawnPoint == 3)
            {
                prefabToUse = enemyPrefabD;
            }

            curSpawnPoint++;

            //Created and enemy and sets the enemies parent.
            GameObject newEnemy = Instantiate(prefabToUse, spawn.position, spawn.rotation);
            newEnemy.GetComponent<Enemy>().SetParent(this);

            enemiesSpawned.Add(newEnemy);

            //everytime an enemy spawns, it gets faster.
            if(spawnRate > 0.03f)
                spawnRate *= reductionMultiplier;

            Debug.Log(spawnRate);
        }
    }
    public void DeleteEnemy(GameObject enemy)
    {
        enemiesSpawned.Remove(enemy);
        Destroy(enemy);
    }

    public void HitPlayer()
    {
        //Question: What is this weird for loop?

        //Answer: the Delete function will remove it from the "enemiesSpawned" list.
        //When removing an element from a list, another will take its place.
        //So if you keep it at the 0th element, it will go one by one and remove everything.
        for(;enemiesSpawned.Count > 0;)
        {
            //The reason why we can't just call Destroy here is because EnemyC might have bullets we need to Destroy as well.
            //EnemyC is the owner of those bullets so we need to destroy them in that class.
            enemiesSpawned[0].GetComponent<Enemy>().Delete();
        }

        //Move player back to spawn
        player.transform.position = playerSpawn.position;
        player.GetComponent<PlayerMovement>().ResetScore();

        //reset spawn rate
        spawnRate = secondsPerEnemySpawn;
    }

}
