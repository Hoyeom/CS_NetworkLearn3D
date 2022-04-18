using Mirror;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Player
{
    public class PlayerController : NetworkBehaviour,IAttackAble
    {
        [SyncVar] private float curHealth = 100;
        
        [SerializeField] private CharacterController charController;
        [SerializeField] private Transform camera = null;
        [SerializeField] private float speed = 3;
    
        private Vector3 inputVector = Vector3.zero;
        private Controller controller;
        private float xRotation;
        
        private void FixedUpdate()
        {
            if(!hasAuthority) { return; }

            Move(inputVector);
            transform.Rotate(Vector3.up, Mouse.current.delta.x.ReadValue());

            xRotation -= Mouse.current.delta.y.ReadValue();
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }

        public override void OnStopAuthority()
        {
            controller.Player.Move.performed -= Performed_Move;
            controller.Player.Move.canceled -= Canceled_Move;
            controller.Player.Fire.started -= Started_Fire;
            
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
            controller.Player.Fire.started += Started_Fire;
        
            controller.Enable();
        }

        private void Started_Fire(InputAction.CallbackContext context)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            
            Debug.Log("Fire");
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent<IAttackAble>(out IAttackAble attackAble))
                {
                    CmdFire(attackAble);
                }
            }
        }

        [Command]
        private void CmdFire(IAttackAble attackAble)
        {
            attackAble?.TakeDamage(5);
        }
        
        private void Move(Vector3 input)
        {
            Vector3 move = transform.right * input.x + transform.forward * input.z;

            charController.Move((move * speed + Physics.gravity) * Time.fixedDeltaTime);
        }

        [Server]
        public void TakeDamage(float damage)
        {
            curHealth -= damage;
            Debug.Log($"{gameObject.name}은 {damage}피해를 입었따! 현재체력이당!: {curHealth}");
        }

        public NetworkIdentity GetIdentity()
        {
            return GetComponent<NetworkIdentity>();
        }


        private void Canceled_Move(InputAction.CallbackContext context) => inputVector = Vector3.zero;

        private void Performed_Move(InputAction.CallbackContext context)
        {
            Vector2 temp = context.ReadValue<Vector2>();
            inputVector = new Vector3(temp.x, 0, temp.y);
        }

    }
}
