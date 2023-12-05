using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.RemoveLife(1);
            Destroy(gameObject);
        }
    }
}
