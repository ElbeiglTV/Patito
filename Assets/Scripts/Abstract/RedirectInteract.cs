using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedirectInteract : MonoBehaviour, Iinteractive
{
    public GameObject interactiveToRedirect;
    

    public void interact(GameObject Player)
    {
        Debug.Log("redirectInteract");
        GetComponent<Collider>().enabled = false;
        interactiveToRedirect.GetComponent<Iinteractive>()?.interact(Player);
    }
}
