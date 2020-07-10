using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Socks.Field(category="Lifespan")]
    public int startingAge;
    
    [SerializeField, Socks.Field(category="Lifespan")]
    public Vector2 lifeSpanRange;

    private int age;

    void Awake()
    {
        // stub
    }

    void Update()
    {
        // stub
    }
}