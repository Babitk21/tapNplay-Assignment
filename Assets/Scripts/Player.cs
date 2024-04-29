using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour{
    public static Player instance{
        get;
        set;
    }
    
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public Animator animator;
    public GameObject secondRoom;
    public CinemachineFreeLook roomCamera;
    public float moneyCollected;
    public Transform moneyPos1;
    public Transform moneyPos2;
    
    private bool isRoomCameraActive;
    private float turnSmoothVelocity;

    private void Start(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(instance);
        }
    }

    private void Update(){
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
        if (direction.magnitude >= 0.1f){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothTime, turnSmoothVelocity);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            animator.SetBool("isMoving", true);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * (speed * Time.deltaTime));
        }
        else{
            animator.SetBool("isMoving", false);
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Money")){
            other.GetComponent<MoneyMovement>().enabled = true;
        }
        else if (other.CompareTag("SecondRoomUnlocker") && moneyCollected >= 4){
            secondRoom.SetActive(true);
            secondRoom.SetActive(true);
        }
        else if (other.CompareTag("RoomCameraActivater")){
            if (!isRoomCameraActive){
                roomCamera.Priority = 11;
                isRoomCameraActive = true;
            }
            else{
                isRoomCameraActive = false;
                roomCamera.Priority = 9;
            }
        }
    }
}
