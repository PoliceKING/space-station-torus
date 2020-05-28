using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelScript : MonoBehaviour
{
    PlayerController player;
    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<PlayerController>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player.inRefuelRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            player.inRefuelRange = false;
        }
    }
}
