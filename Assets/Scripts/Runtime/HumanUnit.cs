using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime
{
    public class HumanUnit : BaseUnit
    {
        private static readonly int HashSpeedX = Animator.StringToHash("SpeedX");
        private static readonly int HashSpeedZ = Animator.StringToHash("SpeedZ");
        
        [Header("Component")]
        
        [SerializeField] private CharacterController charController = null;
        [SerializeField] private Animator anim = null;
        
        [Header("Animation")]
        [SerializeField] private Transform aimPosObj;
        [Header("Aim")]
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private Transform vCamRoot;
        private float xRotation;

        private void Update()
        {
            SetAimPos();
            MouseRotate();
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

        private void SetAimPos()
        {
            aimPosObj.position = mainCam.transform.position + mainCam.transform.forward * 3;
        }
        
        private void MouseRotate()
        {
            transform.Rotate(Vector3.up, Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime);

            xRotation -= Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);
            vCamRoot.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
    }
}