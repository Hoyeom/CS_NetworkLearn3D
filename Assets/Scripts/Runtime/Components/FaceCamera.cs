using System;
using UnityEngine;

namespace Runtime.Components
{
    public class FaceCamera : MonoBehaviour
    {
        private Transform mainCamTransform;

        private void Awake()
        {
            mainCamTransform = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(
                transform.position + mainCamTransform.rotation * Vector3.forward,
                mainCamTransform.rotation * Vector3.up);
        }
    }
}