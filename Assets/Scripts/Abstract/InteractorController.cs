using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//deve ponerse en el player

public class InteractorController : MonoBehaviour
{
    
    GameObject Other;


    private void OnTriggerExit(Collider other)
    {
        Other = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        Other = other.gameObject;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) & Other != null)
        {
            if (Other.TryGetComponent<Iinteractive>(out Iinteractive interact))
            {
                interact.interact(gameObject);
                Other = null;
            }
        }
    }
}
