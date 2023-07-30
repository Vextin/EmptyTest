using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    CharacterController _controller;
    bool inVehicle;
    [SerializeField] float _walkSpeed = 6;
    [SerializeField] GameObject _camera;
    Transform attachedVehicle;

    //a public, readonly copy of the private inVehicle for other scripts to check, but never set.
    public bool InVehicle
    {
        get { return inVehicle; }
    }

    //Same as above, but for the attached vehicle.
    public Transform AttachedVehicle
    {
        get { return attachedVehicle; }
    }

    private void Start()
    {
        //I'm using the builtin CharacterController class for this demo. 
        //There should be one attached to this GameObject.
        _controller = GetComponent<CharacterController>();
        inVehicle = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _movement = GatherInputs();

        if (!inVehicle) Move(_movement);
        else VehicleMove(_movement);
    }

    Vector3 GatherInputs()
    {
        Vector3 move = Vector3.zero;
        //These are called "ternary operators" or "ternary conditionals". They're lazy. The format is "some_variable = some_boolean ? value_if_true : value_if_false"
        move.x += Input.GetKey(KeyCode.D) ? 1f : 0f;
        move.x -= Input.GetKey(KeyCode.A) ? 1f : 0f;
        move.z += Input.GetKey(KeyCode.W) ? 1f : 0f;
        move.z -= Input.GetKey(KeyCode.S) ? 1f : 0f;

        //we can't fly
        move.y = 0;
        return move; 
    }

    void Move(Vector3 move)
    {
        //Make sure movement is tied to framerate so we don't move faster on a better PC. Also apply walk speed.
        move *= Time.deltaTime * _walkSpeed;

        //same as saying "move.z units along the local Z axis and move.x units along the local X axis"
        Vector3 finalMove =
            transform.forward * move.z +
            transform.right * move.x;

        //Move according to player input
        _controller.Move(finalMove);
    }
    
    //Yours will probably be different from Move() in some way, but this is not
    void VehicleMove(Vector3 move)
    {
        //Make sure movement is tied to framerate so we don't move faster on a better PC. Also apply walk speed.
        move *= Time.deltaTime * _walkSpeed;

        //same as saying "move.z units along the local Z axis and move.x units along the local X axis"
        Vector3 finalMove =
            transform.forward * move.z +
            transform.right * move.x;

        //Move according to player input
        _controller.Move(finalMove);
    }

    private void OnTriggerEnter(Collider other)
    {
        //The only triggers we care about are EnterTriggers, which is what the trigger attached to the car is tagged as.
        if (!other.CompareTag("EnterTrigger")) return;
        
        inVehicle= true;
        attachedVehicle = other.transform.parent;
    }

    private void OnTriggerExit(Collider other)
    {
        //The only triggers we care about are EnterTriggers, which is what the trigger attached to the car is tagged as.
        if (!other.CompareTag("EnterTrigger")) return;
        
        inVehicle= false;
        attachedVehicle = null;
    }

}
