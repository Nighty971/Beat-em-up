using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

    [SerializeField] GameObject player;

    [SerializeField] float xClampLeft;
    [SerializeField] float xClampRight;

    Vector3 dirPlayerToCam;
    Vector3 posPlayer;


    private void Start()
    {
        dirPlayerToCam = transform.position - player.transform.position;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        //Follow();
        //MoveToPlayer();
        FollowClamp();
        
    }

    private void Follow()
    {
        Vector3 posCam = player.transform.position;
        posCam += dirPlayerToCam;

        transform.position = posCam;
    }

    private void MoveToPlayer()
    {
        Vector3 posCam = player.transform.position;
        posCam += dirPlayerToCam;
        posCam.y = 0;
        Vector3 clampPosition = posCam;
        //Vector3 clampPosition = Vector3.Lerp(transform.position, posCam, Time.deltaTime);
        clampPosition.x = Mathf.Clamp(clampPosition.x, xClampLeft, xClampRight);

        transform.position = clampPosition;


    }

    private void FollowClamp()
    {
        //if (isOutOfBoundaries())
        {
            MoveToPlayer();
        }

        //posPlayer = player.transform.position;
        //posPlayer.y = 0;
        //posPlayer.z = transform.position.z;
        //transform.position = posPlayer;
    }

    bool isOutOfBoundaries()
    {
        // RECUPERER LA POSITION X DU PLAYER A L'ECRAN
        float screenPosPlayer = Camera.main.WorldToScreenPoint(player.transform.position).x;


        // SI MON PLAYER EST DANS LES LIMITES DE MON X CLAMP
        if (screenPosPlayer > xClampLeft && screenPosPlayer < Screen.width - xClampRight)
        {
            return false;

        }

        return true;

    }




}
