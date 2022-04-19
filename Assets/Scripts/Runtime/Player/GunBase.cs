﻿using System;
using Mirror;
using UnityEngine;

namespace Runtime.Player
{
    public class GunBase : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;

        public IAttackAble FireRayBullet(Vector3 hitPoint)
        {
            IAttackAble attackAble = null;
            
            Ray ray = new Ray(firePoint.position, hitPoint - firePoint.position);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if(hit.collider.TryGetComponent<IAttackAble>(out attackAble))
                {
                    
                }
            }

            return attackAble;
        }
    }
}