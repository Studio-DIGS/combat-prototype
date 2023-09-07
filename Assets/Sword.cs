using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float hoverHeight = 0.1f;  // Adjust the height of the hover animation.
    public float hoverSpeed = 1.0f;  // Adjust the speed of the hover animation.

    private Vector3 initialPosition;
    private float timeElapsed;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the new Y position based on the hover animation.
        float newY = initialPosition.y + Mathf.Sin(timeElapsed * hoverSpeed) * hoverHeight;

        // Update the item's position with the new Y value.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Increment timeElapsed for the sin wave.
        timeElapsed += Time.deltaTime;
    }
}
