using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        
    }

    public void Hit(Vector2 knockback)
    {
        Debug.Log("Got hit with " + knockback + " force");
        rb.AddForce(knockback);
    }
}
