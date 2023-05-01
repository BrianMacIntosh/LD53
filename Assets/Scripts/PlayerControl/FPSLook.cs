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
        private Vector2 mousePos;
        private float cameraY;

        private void Start()
        {
            cameraY = transform.position.y - player.transform.position.y;
            Cursor.lockState = CursorLockMode.Locked;

            //FPSMovement.NewMousePos += NewMousePos;
        }


        private void LateUpdate()
        {
            mousePos = Mouse.current.delta.ReadValue();
            UpdateCameraPosition();
            UpgradeCameraRotation();
        }

        private void UpdateCameraPosition()
        {
            var pos = player.transform.position;
            pos.y += cameraY;
            transform.position = pos;
        }

        private void UpgradeCameraRotation()
        {
            xRotation -= mousePos.y * Time.deltaTime * mouseSensitivity;
            xRotation = Mathf.Clamp(xRotation, -90, 90);
            yRotation += mousePos.x * Time.deltaTime * mouseSensitivity;
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            player.transform.localRotation = Quaternion.Euler(0, yRotation, 0);
        }

        //private void NewMousePos(Vector2 pos)
        //{
        //    mousePos = pos;
        //}
    }
}