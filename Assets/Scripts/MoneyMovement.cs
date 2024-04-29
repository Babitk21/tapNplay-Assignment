using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyMovement : MonoBehaviour
{
    public Transform position1;         // Transform of position1
    public Transform position2;         // Transform of position2
    public float moveSpeed = 5f;        // Speed of movement towards positions

    private bool isMovingToPosition1 = true;   // Flag to track movement direction

    void Start(){
        position1 = Player.instance.moneyPos1;
        position2 = Player.instance.moneyPos2;
    }

    void Update()
    {
        // Determine target position based on current movement direction
        Transform targetPosition = isMovingToPosition1 ? position1 : position2;

        // Calculate movement direction towards the target position
        Vector3 direction = (targetPosition.position - transform.position).normalized;

        // Calculate distance to the target position
        float distance = Vector3.Distance(transform.position, targetPosition.position);

        // Move towards the target position
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotate object to face the movement direction
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
        }

        // Check if the object has reached the target position with a small threshold
        if (distance <= 0.1f)
        {
            // Toggle movement direction to switch between position1 and position2
            isMovingToPosition1 = !isMovingToPosition1;

            // Stop movement if reached position2
            if (!isMovingToPosition1)
            {
                // Set the object's position to position2 to ensure accurate alignment
                transform.position = position2.position;

                // Disable further movement by disabling this script component
                enabled = false;
            }
        }
    }
}
