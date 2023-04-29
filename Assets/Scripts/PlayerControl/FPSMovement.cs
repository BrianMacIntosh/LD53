using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class FPSMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;

        private CharacterController controller;
        private Vector2 moveInput;
        private bool jumpInput;

        public static event Action<Vector2> NewMousePos; 

        private void Start()
        {
            controller = GetComponent<CharacterController>();
            FPSLook look = GetComponent<FPSLook>();
        }

        private void Update()
        {
            // MOVE
            float x = moveInput.x;
            float z = moveInput.y;
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * moveSpeed * Time.deltaTime);

            // JUMP (BROKEN)
            if (jumpInput && controller.isGrounded)
            {
                controller.Move(Vector3.up * jumpForce * Time.deltaTime);
            }
        }

        // Player Input Message Callbacks 
        public void OnMovement(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            jumpInput = value.Get<float>() > 0.5f;
        }

        public void OnMousePos(InputValue value)
        {
            var pos = value.Get<Vector2>();
            NewMousePos?.Invoke(pos);
        }
    }
}