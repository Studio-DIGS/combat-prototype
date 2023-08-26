using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startingPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }
    
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            ResetPos();
        }
    }

    public void Hit(Vector2 knockback)
    {
        Debug.Log("Got hit with " + knockback + " force");
        rb.AddForce(knockback);
    }

    public void ResetPos()
    {
        transform.position = startingPosition;
    }
}
