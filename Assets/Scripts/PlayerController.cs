using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PortalTraveller
{
    public float walkSpeed = 3;
    public float smoothMoveTime = 0.1f;
    public float gravity = 18;
    float verticalVelocity;

    CharacterController controller;
    Vector3 velocity;
    Vector3 smoothV;
    private void Start() {
        controller = GetComponent<CharacterController>();
    }
    private void Update() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection (inputDir);
        float currentSpeed = walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp (velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move (velocity * Time.deltaTime);
        if(flags == CollisionFlags.Below) {

        }

        if(Input.GetKeyDown(KeyCode.Space)) {
            
        }
    }
}
