using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemList; // liste des items
    private int itemNum; // numero a choisir dans la liste d'item
    private int randNum; // chance de loot
    private Transform Epos; // position de l'ennemie


    private void Start()
    {
        Epos = GetComponent<Transform>();
    }

    public void DropItem()
    {



        randNum = Random.Range(0, 101); // 100% loot chance;
        Debug.Log("Random Number is " + randNum);


        if (randNum >= 76) 
        {
            itemNum = 2;
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);


        }
        else if (randNum > 41 && randNum < 75) 
        {

            itemNum = 1;
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);

        }
        else if (randNum > 0 && randNum <= 40)
        {

            itemNum = 0;
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);

        }


    }
} 