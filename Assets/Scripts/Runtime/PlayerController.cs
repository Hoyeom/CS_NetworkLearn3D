using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime
{
    public class PlayerController : MonoBehaviour
    {
        private Controller controller;

        public event Action<Vector3> OnChangedInputVector;
        private Vector3 inputVector = Vector2.zero;
        private Vector3 InputVector
        {
            get => inputVector;
            set
            {
                inputVector = value;
                OnChangedInputVector?.Invoke(inputVector);
            }
        }

        [SerializeField] private GameObject target = null;
        private IControlAble iControlAble;

        private void Awake() => ReTargeting();
        private void FixedUpdate() => iControlAble.Move();

        private void ReTargeting(GameObject newTarget = null)
        {
            if(newTarget != null) target = newTarget;
        
            iControlAble?.OnDisableTarget(this);
        
            iControlAble = target.GetComponent<IControlAble>();
        
            iControlAble.OnEnableTarget(this);
        }

    

        private void MoveInput_Performed(InputAction.CallbackContext context)
        {
            Vector2 temp = context.ReadValue<Vector2>();
            InputVector = new Vector3(temp.x, 0, temp.y);
        }
    
        private void MoveInput_Canceled(InputAction.CallbackContext context)
        {
            InputVector = Vector3.zero;
        }

        private void RunInput_Started(InputAction.CallbackContext context) =>
            iControlAble.RunInput();

        private void OnEnable()
        {
            controller ??= new Controller();
        
            controller.Enable();
            controller.Player.Move.performed += MoveInput_Performed;
            controller.Player.Move.canceled += MoveInput_Canceled;
        }
    

        private void OnDisable()
        {
            controller.Disable();
            controller.Player.Move.performed -= MoveInput_Performed;
            controller.Player.Move.canceled -= MoveInput_Canceled;
        }
    }
}