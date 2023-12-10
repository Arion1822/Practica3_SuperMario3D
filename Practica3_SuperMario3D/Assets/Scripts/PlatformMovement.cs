using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public GameObject platform;
    public GameObject platformRotate;
    public GameObject[] waypoints;
    public float platformSpeed = 2;
    private int waypointsIndex = 0;


    // Update is called once per frame
    void Update()
    {
        MovePlatform();
        

    }

    void MovePlatform()
    {
        if(Vector3.Distance(platform.transform.position, waypoints[waypointsIndex].transform.position) < 0.1f)
        {
            waypointsIndex++;
            if(waypointsIndex >= waypoints.Length)
            {
                waypointsIndex = 0;
                
            }
            //platformRotate.transform.Rotate(new Vector3(120f, 0f, 0f) * Time.deltaTime);
        }
        

        platform.transform.position = Vector3.MoveTowards(platform.transform.position, waypoints[waypointsIndex].transform.position, platformSpeed * Time.deltaTime);
        
    }
    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            collision.gameObject.transform.SetParent(platform.transform);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            collision.gameObject.transform.SetParent(null);
        }
    }
}
