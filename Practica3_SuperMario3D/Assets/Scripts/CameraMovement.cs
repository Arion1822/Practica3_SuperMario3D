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
        F_DirectionItsHeading += Input.GetAxis("Mouse X") * Time.deltaTime * F_CamTurnSpeed; //Stores our Mouse X information for rotating the camera later 
        F_CamTilt += Input.GetAxis("Mouse Y") * Time.deltaTime * F_CamTurnSpeed; //Modifying the tilt
        F_CamTilt = Mathf.Clamp(F_CamTilt, F_CamTiltLowest, F_CamTiltHighest); //limited camera rotation on Y Axis
        transform.rotation = Quaternion.Euler(F_CamTilt, F_DirectionItsHeading, 0); //Camera tilt setup

        transform.position = T_Player.position - transform.forward * F_CamDistance + Vector3.up * F_PlayerHeight; //Set a Camera distance away from the player & sets the height focus point
    }
}
