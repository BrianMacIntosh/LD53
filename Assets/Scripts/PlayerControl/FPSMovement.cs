using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class FPSMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask groundLayer;

        private CharacterController controller;
        private Vector2 moveInput;
        private bool jumpInput;
        private Vector3 velocity;

        //public static event Action<Vector2> NewMousePos;

        private void Start()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            // MOVE
            float x = moveInput.x;
            float z = moveInput.y;
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * moveSpeed * Time.deltaTime);

            // JUMP
            if (jumpInput && IsGrounded())
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
            }
            jumpInput = false;

            // GRAVITY
            velocity.y += Physics.gravity.y * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }

        private bool IsGrounded()
        {
            Vector3 center = controller.bounds.center;
            center.y -= (controller.height / 2f) - controller.radius - 0.1f;
            return Physics.OverlapSphere(center, controller.radius, groundLayer).Length > 0;
        }

        // Player Input Message Callbacks 
        public void OnMovement(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public void OnJump(InputValue value)
        {
            jumpInput = value.Get<float>() > 0.5f;
            Debug.Log("Jump!");
        }

        public void OnMousePos(InputValue value)
        {
            var pos = value.Get<Vector2>();
            //NewMousePos?.Invoke(pos);
        }
    }
}