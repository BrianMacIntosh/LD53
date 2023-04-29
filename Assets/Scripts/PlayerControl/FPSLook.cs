using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerControl
{
    public class FPSLook : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private float mouseSensitivity = 10f;

        private float xRotation = 0f;
        private float yRotation = 0f;
        private Vector2 mouseMovement;
        private float cameraY;
        private Vector3 pos;

        private void Start()
        {
            cameraY = transform.position.y - player.transform.position.y;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            UpdateMousePosition();

            xRotation -= mouseMovement.y * Time.deltaTime * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            yRotation += mouseMovement.x * Time.deltaTime * mouseSensitivity;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            player.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void UpdateMousePosition()
        {
            pos = player.transform.position;
            pos.y += cameraY;
            transform.position = pos;

            mouseMovement = Mouse.current.delta.ReadValue();
        }
    }
}