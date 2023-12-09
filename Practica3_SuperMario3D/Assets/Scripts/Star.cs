using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    private void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, 60f) * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.GetStar();
            //Destroy(gameObject);
        }
    }
}
