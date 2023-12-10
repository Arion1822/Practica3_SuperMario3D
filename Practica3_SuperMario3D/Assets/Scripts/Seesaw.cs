using UnityEngine;

public class Seesaw : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPlayerOnBalancin = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isPlayerOnBalancin)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float rotationSpeed = 5.0f;

            // Calcula la rotaci�n basada en la entrada del jugador
            float targetRotation = transform.rotation.eulerAngles.y + horizontalInput * 30.0f;  // Ajusta el factor de rotaci�n seg�n sea necesario

            // Interpola suavemente hacia la nueva rotaci�n
            Quaternion targetQuaternion = Quaternion.Euler(0, targetRotation, 0);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetQuaternion, Time.deltaTime * rotationSpeed));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBalancin = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerOnBalancin = false;
        }
    }
}