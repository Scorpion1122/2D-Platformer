using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thijs.Platformer.Characters
{
    public class CameraController2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float offsetChangeSpeed = 1f;
        [SerializeField] private float maxOffset = 5f;

        private Vector2 targetOffset;
        private Vector2 previousPosition;

        private void OnEnable()
        {
            if (target == null)
                enabled = false;
        }

        private void Update()
        {
            Vector2 currentPosition = target.position;
            Vector2 velocity = currentPosition - previousPosition;
        }

        private void LateUpdate()
        {
            previousPosition = target.position;
        }
    }
}
