using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour, Iinteractive
{ 
    public GameObject BulletSpawn;
    public Transform gunAnchor;


    public virtual void interact(GameObject Player)
    {
        gunAnchor = Player.GetComponent<PlayerController>().gunAnchor; 
        
    }
    public virtual void HoldWeapon()
    {
        transform.position = gunAnchor.position;
        transform.rotation = gunAnchor.rotation;
    }
   
}
