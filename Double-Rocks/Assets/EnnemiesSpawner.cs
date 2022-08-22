using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemiesSpawner : MonoBehaviour
{
    [SerializeField] int spawnCount = 5;
    [SerializeField] Transform enemiesParent;
    [SerializeField] List<GameObject> enemiesInScene;

    [SerializeField] GameObject ennemies;
    [SerializeField] GameObject player;

    public static int enemyEnVie;

    //pour plus tard, a une certaine distance du player les ennemies spawn 
    //private float triggerRadius = 10f;




    // Start is called before the first frame update
    void Start()
    {



        // Rajouter tous les enfants de enemiesParent dans la liste enemiesInScene
        foreach (Transform child in enemiesParent)
        {
            enemiesInScene.Add(child.gameObject);
        }


        enemyEnVie = spawnCount  + enemiesInScene.Count;


        for (int i = 0; i < spawnCount; i++)
        {

            Vector2 randomPos = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));

            GameObject enemy = Instantiate(ennemies, randomPos, transform.rotation, transform);
            
        }

    }

    // Update is called once per frame
    void Update()
    {

        // a modifié plus tard
        if (spawnCount <= 0 || player == null)
        {
            return;
        }
    }
}
