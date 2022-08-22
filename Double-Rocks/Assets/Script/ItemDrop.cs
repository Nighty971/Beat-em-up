using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] itemList; // liste des items
    [SerializeField] private int dropNumber; // nombre de drop
    private int randNum; // chance de loot
    private int itemNum; // numero a choisir dans la liste d'item
    private Transform Epos; // position de l'ennemie


    private void Start()
    {
        Epos = GetComponent<Transform>();
        dropNumber = 2;
    }

    public void DropItem()
    {

        

        randNum = Random.Range(0, 101); // chance de loot;
        
        

        if (randNum >= 76 && dropNumber > 0 ) 
        {
            itemNum = 2;
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);
            dropNumber--;

        }

        else if (randNum > 41 && randNum < 75 && dropNumber > 0) 

        {

            itemNum = 1;
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);
            dropNumber--;
        }

        else if (randNum > 0 && randNum <= 40 && dropNumber > 0)

        {

            itemNum = 0;
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);
            dropNumber--;

        }
        
    }

} 