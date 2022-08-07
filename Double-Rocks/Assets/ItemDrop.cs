using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject[] itemList; 
    private int itemNum; 
    private int randNum;
    private Transform Epos; //La position de l'ennemi est la position de l'item


    private void Start()
    {
        // itemList = CamFollow.itemListPass;
        Epos = GetComponent<Transform>();
        Debug.Log(itemList);
    }

    public void DropItem()
    {

        randNum = Random.Range(0, 101); // 100% de chance de loot;
        //Debug.Log("Random Number is " + randNum);


        if (randNum >= 95) 
        {
            itemNum = 2; 
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);


        }
        else if (randNum > 75 && randNum < 95) 
        {

            itemNum = 1; 
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);

        }
        else if (randNum > 40 && randNum <= 75)
        {

            itemNum = 0; 
            Instantiate(itemList[itemNum], Epos.position, Quaternion.identity);

        }


    }
} 