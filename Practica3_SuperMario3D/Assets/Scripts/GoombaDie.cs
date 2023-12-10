using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaDie : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GetComponentInParent<Goomba>().Die();
        }
    }
    
}
