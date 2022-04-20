using System;
using DG.Tweening;
using UnityEngine;

namespace Runtime
{
    public class HumanUnit : BaseUnit
    {
        private static readonly int HashSpeedX = Animator.StringToHash("SpeedX");
        private static readonly int HashSpeedZ = Animator.StringToHash("SpeedZ");
        
        [Header("Component")]
        
        [SerializeField] private CharacterController charController = null;
        [SerializeField] private Animator anim = null;
        [SerializeField] private Transform eyePos;
        [SerializeField] private Transform aimPosObj;

        private void Update()
        {
            aimPosObj.position = mainCam.transform.position + mainCam.transform.forward * 3;
        }

        public override void Move()
        {
            charController.SimpleMove(GetCamForwardAxisDir() * Input * speed);

            // if (Input.sqrMagnitude > 0)
            //     transform.rotation =
            //         Quaternion.Slerp(transform.rotation, GetCamAxisInputDir(), turnSpeed * Time.deltaTime);
        }
        
        public override void SetInputVector(Vector3 input)
        {
            base.SetInputVector(input);
            anim.SetFloat(HashSpeedX, Mathf.RoundToInt(Input.x));
            anim.SetFloat(HashSpeedZ, Mathf.RoundToInt(Input.z));
        }
    }
}