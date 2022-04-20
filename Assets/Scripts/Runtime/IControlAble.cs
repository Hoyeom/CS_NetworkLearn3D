using UnityEngine;

namespace Runtime
{
    public interface IControlAble
    {
        public void Move();
        public void SetInputVector(Vector3 input);
        public void RunInput();
        public void OnEnableTarget(PlayerController playerController);
        public void OnDisableTarget(PlayerController playerController);
    }
}