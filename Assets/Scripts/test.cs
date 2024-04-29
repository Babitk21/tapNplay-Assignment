using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player")){
            gameObject.GetComponent<MoneyMovement>().enabled = true;
        }
    }
}
