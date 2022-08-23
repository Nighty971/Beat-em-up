using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    [SerializeField] float spawnDuration;
    [SerializeField] int numberEnnemies;

    //public List<Enemy> enemies = new List<Enemy>();
    //private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    private float spawnTimer;
    private float spawnDelay;
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnDelay = spawnDuration / numberEnnemies;
        spawnTime = spawnDelay;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            //spawn an enemy
            if (Time.time >= spawnTime && enemiesToSpawn.Count > 0)
            {
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
                enemiesToSpawn.RemoveAt(0); // and remove it
                numberEnnemies--;
                spawnTime = Time.time + spawnDelay;
                spawnedEnemies.Add(enemy);
                
            }
        }
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;

}

