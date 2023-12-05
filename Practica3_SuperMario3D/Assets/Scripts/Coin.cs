using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.AddCoins(1);
            Destroy(gameObject);
        }
    }
}
