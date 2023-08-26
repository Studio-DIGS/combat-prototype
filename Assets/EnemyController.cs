using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startingPosition;
    [SerializeField] private ParticleSystem particles;
    
    //access the particle module, in this case MainModule
    private ParticleSystem.MainModule pMain;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        pMain = particles.main;
    }
    
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            ResetPos();
        }

        var emission = particles.emission;
        emission.rateOverTime = rb.velocity.sqrMagnitude * 5f;
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
