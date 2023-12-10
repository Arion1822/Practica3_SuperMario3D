using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject flag;
    public Texture2D checkpointTexture;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            GameManager.Instance.UpdateCheckpoint(gameObject);
            
            SetFlagMaterial();
        }
    }
    
    private void SetFlagMaterial()
    {
        if (flag != null && checkpointTexture != null)
        {
            // Create a new material
            Material newMaterial = new Material(Shader.Find("Standard"));
            
            // Assign the texture to the material's main texture
            newMaterial.mainTexture = checkpointTexture;

            // Assign the material to the flag's renderer
            Renderer flagRenderer = flag.GetComponent<Renderer>();
            if (flagRenderer != null)
            {
                flagRenderer.material = newMaterial;
            }
            else
            {
                Debug.LogWarning("Flag GameObject doesn't have a Renderer component.");
            }
        }
        else
        {
            Debug.LogWarning("Flag GameObject or checkpoint texture is not set in the inspector.");
        }
    }
}
