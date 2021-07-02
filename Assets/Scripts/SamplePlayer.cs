/******************************************************************************
Author: Elyas Chua-Aziz

Name of Class: DemoPlayer

Description of Class: This class will control the movement and actions of a 
                        player avatar based on user input.

Date Created: 09/06/2021
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamplePlayer : MonoBehaviour
{
    /// <summary>
    /// The distance this player will travel per second.
    /// </summary>
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotationSpeed;

    /// <summary>
    /// The camera attached to the player model.
    /// Should be dragged in from Inspector.
    /// </summary>
    [SerializeField]
    private Camera playerCamera;

    private string currentState;

    private string nextState;

    // Start is called before the first frame update
    void Start()
    {
        nextState = "Idle" +
            "";
    }

    // Update is called once per frame
    void Update()
    {
        if(nextState != currentState)
        {
            SwitchState();
        }

        CheckRotation();
    }

    /// <summary>
    /// Sets the current state of the player
    /// and starts the correct coroutine.
    /// </summary>
    private void SwitchState()
    {
        StopCoroutine(currentState);
        currentState = nextState;
        StartCoroutine(currentState);
    }


    //Coroutine for the "idle" state
    private IEnumerator Idle()
    {
        while(currentState == "Idle")
        {
            //Revealed in CA4 ans in wk 11 Logic error
            if(Input.GetAxis("Horizontal") !!= 0 || Input.GetAxis("Vertical") != 0)
            {
                nextState = "Moving";
                //Debug.Log("Move");
            }
            yield return null;
        }
    }
    //Coroutine for the "moving" state
    private IEnumerator Moving()
    {
        while (currentState == "Moving")
        {
            if (!CheckMovement())
            {
                nextState = "Idle";
                //Debug.Log("Stop");
                
            }
            yield return null;
        }
        
    }

    //This affected camera movement
    private void CheckRotation()
    {
        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(playerRotation);

        //changing from += to -= makes the camera move as intended
        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        cameraRotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    /// <summary>
    /// Checks and handles movement of the player
    /// </summary>
    /// <returns>True if user input is detected and player is moved.</returns>
    private bool CheckMovement()
    {
        //this was deemed unecessary. Commenting out affected nothing (I hope)
        //Vector3 newPos = transform.position;

        //move left and right (Problematic)
        Vector3 xMovement = transform.right * Input.GetAxis("Horizontal");
        Debug.Log(xMovement);
        //move forward and bacl
        Vector3 zMovement = transform.forward * Input.GetAxis("Vertical");
        Debug.Log(zMovement);

        Vector3 movementVector = xMovement + zMovement;

        //HAs something to do with movement
        if(movementVector.sqrMagnitude > 0)
        {
            movementVector *= (moveSpeed * Time.deltaTime);
            //newPos = movementVector;

            transform.position += movementVector;
            return true;
        }
        else
        {
            return false;
        }

    }
}
