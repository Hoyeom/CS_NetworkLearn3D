using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] private CharacterController charController;
        [SerializeField] private Transform camera = null;
        [SerializeField] private float speed = 3;
    
        private Vector3 inputVector = Vector3.zero;
        private Controller controller;
    
    
        private void FixedUpdate()
        {
            if(!hasAuthority) { return; }

            Move(inputVector);
            transform.Rotate(Vector3.up, Mouse.current.delta.x.ReadValue());
            camera.Rotate(Vector3.left, Mouse.current.delta.y.ReadValue());
        }

        public override void OnStopAuthority()
        {
            controller.Player.Move.performed -= Performed_Move;
            controller.Player.Move.canceled -= Canceled_Move;
        
            controller.Disable();
        }

        public override void OnStartAuthority()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            camera.gameObject.SetActive(true);

            controller ??= new Controller();

            controller.Player.Move.performed += Performed_Move;
            controller.Player.Move.canceled += Canceled_Move;
        
            controller.Enable();
        }
        
        private void Move(Vector3 input)
        {
            input += Physics.gravity;
            Vector3 move = transform.right * input.x + transform.forward * input.z;

            charController.Move( move * speed * Time.fixedDeltaTime);
        }
    
        private void Canceled_Move(InputAction.CallbackContext context) => inputVector = Vector3.zero;

        private void Performed_Move(InputAction.CallbackContext context)
        {
            Vector2 temp = context.ReadValue<Vector2>();
            inputVector = new Vector3(temp.x, 0, temp.y);
        }

    }
}
