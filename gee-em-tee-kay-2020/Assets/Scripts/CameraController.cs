using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Position")]
    public VectorSmoother positionSmoother;
    [SerializeField, Socks.Field(category="Smoothing")] 
    private Smoother zoomSmoother = null;
    [SerializeField, Socks.Field(category="Smoothing")] 
    private Smoother screenShakeSmoother = null;

    [SerializeField, Socks.Field(category="Position")]
    public Transform transformToFollow;

    public void Follow(Transform newTransform)
    {
        transformToFollow = newTransform;
    }

    void LateUpdate()
    {
        if (transformToFollow != null)
        {
            positionSmoother.SetDesiredValue(transformToFollow.position);
            transform.position = positionSmoother.Smooth();
        }
        else
        {
            PlayerController player = GameObject.FindObjectOfType<PlayerController>();

            if (player != null)
                transformToFollow = player.transform;
        }
        
        UpdateScreenShake();
    }

    /** Update screenshake */
    private void UpdateScreenShake()
    {
        // Add screenshake
        Vector2 screenShake = Random.insideUnitCircle;
        float multiplier = screenShakeSmoother.Smooth();

        transform.position = transform.position + (transform.up*screenShake.y*multiplier) + (transform.right*screenShake.x*multiplier);
    }
    
    public void ScreenShake(float amount)
    {
        screenShakeSmoother.SetCurrentValue(amount);
    }

}