using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : PortalTraveller
{
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public float rotationSmoothTime = 0.1f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;
    bool jumping;
    float lastGroundedTime;

    private void Start() {
        cam = Camera.main;
        if(lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        controller = GetComponent<CharacterController>();

        yaw = transform.eulerAngles.y;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.P)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        // 현재 입력값을 로컬 -> 월드 기준으로 변환
        Vector3 worldInputDir = transform.TransformDirection(inputDir);
        
        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);
        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);

        // grounded
        if(flags == CollisionFlags.Below) {
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }
        // Jump
        if(Input.GetKeyDown(KeyCode.Space)) {
            float timeSinceLastTouchGround = Time.time - lastGroundedTime;
            if(controller.isGrounded || (!jumping && timeSinceLastTouchGround < 0.15f)) {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        float mMag = Mathf.Sqrt(mx * mx + my * my);
        if(mMag > 5) {
            mx = 0;
            my = 0;
        }
        yaw += mx * mouseSensitivity;

        transform.eulerAngles = Vector3.up * yaw;
    }
}
