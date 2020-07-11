using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Position")]
    public VectorSmoother positionSmoother;

    [SerializeField, Socks.Field(category="Position")]
    public Transform transformToFollow;

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
    }

}