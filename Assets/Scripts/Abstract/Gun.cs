using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, Iinteractive
{ 
    public GameObject BulletSpawn;
    public Transform gunAnchor;


    public virtual void interact(GameObject Player)
    {
        Debug.Log("gunInteract");

        gunAnchor = Player.GetComponent<PlayerController>().playerReference.gunAnchor;
        Player.GetComponent<PlayerController>().playerReference.GunEquiped = gameObject;


    }
    public virtual void HoldWeapon()
    {
        if(gunAnchor != null)
        {
        transform.position = gunAnchor.position;
        transform.rotation = gunAnchor.rotation;

        }
    }
   
}
