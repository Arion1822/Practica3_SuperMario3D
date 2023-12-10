using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [Header("External Refernces")]
    public Transform T_Player;
    private CharacterController CC_PlayerCC;

    [Header("Camera Settings")]
    public float F_CamTurnSpeed;
    public float F_CamDistance;
    public float F_CamTilt;
    public float F_CamTiltLowest;
    public float F_CamTiltHighest;

    [Header("Stored Camera Information")]
    public float F_DirectionItsHeading; //Este valor al llegar a 90 se coloca justo detras de Mario. Utilizar para hacer la camara opcional

    [Header("Player Information")]
    public float F_PlayerHeight;
    
    public float F_IdleRepositionTime = 5f; // Time to wait before repositioning
    private float idleTimer;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CC_PlayerCC = T_Player.GetComponent<CharacterController>();
        UpdatePlayerHeight();
    }

    void UpdatePlayerHeight()
    {
        F_PlayerHeight = T_Player.localScale.y / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        idleTimer += Time.deltaTime;

        F_DirectionItsHeading += Input.GetAxis("Mouse X") * Time.deltaTime * F_CamTurnSpeed; // Stores our Mouse X information for rotating the camera later 
        F_CamTilt += Input.GetAxis("Mouse Y") * Time.deltaTime * F_CamTurnSpeed; // Modifying the tilt
        F_CamTilt = Mathf.Clamp(F_CamTilt, F_CamTiltLowest, F_CamTiltHighest); // Limited camera rotation on Y Axis
        transform.rotation = Quaternion.Euler(F_CamTilt, F_DirectionItsHeading, 0); // Camera tilt setup

        transform.position = T_Player.position - transform.forward * F_CamDistance + Vector3.up * F_PlayerHeight; // Set a Camera distance away from the player & sets the height focus point

        // Check if there is any movement or camera rotation
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            // Reset the idle timer
            idleTimer = 0f;
        }

        // Reposition the camera if it's idle for a certain time
        if (idleTimer >= F_IdleRepositionTime)
        {
            // Reset the camera position behind Mario
            RepositionCameraBehindPlayer();
        }
    }
    
    void RepositionCameraBehindPlayer()
    {
        // Reset the camera position behind Mario
        float newYRotation = T_Player.eulerAngles.y;
        transform.position = T_Player.position + Quaternion.Euler(0, newYRotation, 0) * Vector3.back * F_CamDistance + Vector3.up * F_PlayerHeight;

        // Recalculate F_DirectionItsHeading based on the new camera position
        F_DirectionItsHeading = newYRotation + 180;

        // Reset the camera rotation
        transform.rotation = Quaternion.Euler(F_CamTiltLowest, F_DirectionItsHeading, 0);
    }
}