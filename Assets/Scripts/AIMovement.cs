using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform[] steps;           // Array of Transforms representing the steps
    public float moveSpeed = 5f;        // Speed of character movement
    public float waitTime = 5f;         // Time to wait at step2 in seconds
    public GameObject money;

    private bool canDropMoney = true;
    private int currentStepIndex = 0;   // Index of the current step in the steps array
    private bool isMoving = true;       // Flag to track if the character is currently moving

    void Start()
    {
        // Start moving towards the first step
        MoveToNextStep();
    }

    void MoveToNextStep()
    {
        if (currentStepIndex < steps.Length)
        {
            Transform nextStep = steps[currentStepIndex];
            StartCoroutine(MoveToStep(nextStep));
        }
    }

    IEnumerator MoveToStep(Transform targetStep)
    {
        Vector3 direction = (targetStep.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetStep.position);

        // Move towards the target step
        while (distance > 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
            distance = Vector3.Distance(transform.position, targetStep.position);
            yield return null;
        }

        // Wait at step2 for specified waitTime
        if (currentStepIndex == 1) // Assuming step2 is at index 1 in steps array
        {
            isMoving = false; // Stop moving temporarily
            yield return new WaitForSeconds(waitTime);
            isMoving = true; // Resume movement after waitTime
            yield return new WaitForSeconds(waitTime + 1f);
            DroppingMoney();
        }

        // Move to the next step
        currentStepIndex++;

        // Check if reached the last step (step3) to stop further movement
        if (currentStepIndex >= steps.Length)
        {
            isMoving = false; // Stop moving
            yield break; // Exit coroutine
        }

        MoveToNextStep();
    }

    void DroppingMoney(){
        if (canDropMoney == true){
            Instantiate(money, steps[1].position, quaternion.identity);
            canDropMoney = false;
        }
    }
    void Update()
    {
        if (isMoving)
        {
            // Update movement towards the current step
            Transform currentStep = steps[currentStepIndex];
            Vector3 direction = (currentStep.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
        }
    }
}
