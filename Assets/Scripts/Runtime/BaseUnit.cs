using UnityEngine;

namespace Runtime
{
    public class BaseUnit : MonoBehaviour,IControlAble
    {
        [SerializeField] protected float speed = 3;
        [SerializeField] protected float turnSpeed = 8;

        protected Vector3 Input
        {
            get => input;
            private set
            {
                value.Normalize();
                input = value;
            }
        }

        private Vector3 input = Vector3.zero;
        
        protected Camera mainCam = null;

        public void Awake() => mainCam = Camera.main;
        
        public virtual void Move() { }

        public virtual void RunInput() { }
        public virtual void SetInputVector(Vector3 input) => Input = input;

        public void OnEnableTarget(PlayerController playerController) =>
            playerController.OnChangedInputVector += SetInputVector;

        public void OnDisableTarget(PlayerController playerController) =>
            playerController.OnChangedInputVector -= SetInputVector;
        

        protected Quaternion GetCamAxisInputDir() =>
            Quaternion.LookRotation(Input) * GetCamForwardAxisDir();

        protected Quaternion GetCamForwardAxisDir() =>
            Quaternion.Euler(0, mainCam.transform.eulerAngles.y, 0);

    }
}