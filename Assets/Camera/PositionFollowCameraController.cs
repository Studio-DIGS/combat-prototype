using System.Collections;
using System.Collections.Generic;
using System.Data;
using TarodevController;
using UnityEditorInternal;
using UnityEngine;

namespace Obscura
{
    public class PositionFollowCameraController : AbstractCameraController
    {
        [SerializeField] public float crosshairLength;
        [Range(0,1)]
        [SerializeField] private float followSpeedFactor; // The fraction of the target's movement speed that is set to the camera's chasing speed.
        [SerializeField] private float leashDistance; // The camera should move at the same speed as the target when they are leashDistance apart
        [SerializeField] private float catchUpSpeed; // The camera should move catchUpSpeed toward the target when the target is not moving
        [SerializeField] private float playerVelocityThreshold = 150f; // Speed where player is considered "stopped"
        
        private Camera managedCamera;
        private Vector3 targetPosition;
        private Vector3 cameraPosition;
        private LineRenderer cameraLineRenderer;
        private PlayerController playerController;

        private void Awake()
        {
            managedCamera = gameObject.GetComponent<Camera>();
            cameraLineRenderer = gameObject.GetComponent<LineRenderer>();
            playerController = this.Target.GetComponent<PlayerController>();
        }
        
        private void Start()
        {
            targetPosition = this.Target.transform.position;
            cameraPosition = this.managedCamera.transform.position;
            Vector3 spawnLocation = new Vector3(targetPosition.x,targetPosition.y, managedCamera.transform.position.z);
            managedCamera.transform.position = spawnLocation; // initialize to camera's starting position
        }

        //Use the LateUpdate message to avoid setting the camera's position before
        //GameObject locations are finalized.
        void LateUpdate()
        {
            targetPosition = this.Target.transform.position;
            cameraPosition = this.managedCamera.transform.position;
            var targetPosition2D = new Vector2(targetPosition.x, targetPosition.y);
            var cameraPosition2D = new Vector2(cameraPosition.x, cameraPosition.y);
            
            Vector2 smoothPosition;
            var distanceBetween = (targetPosition2D - cameraPosition2D).magnitude;
            
            var playerVelocity = playerController.GetCurrentVelocity();
            if (playerVelocity <= playerVelocityThreshold)
            {
                // Debug.Log("PLAYER STOPPED");
                
                smoothPosition = Vector2.Lerp(cameraPosition2D, targetPosition2D, catchUpSpeed * Time.deltaTime);

            }
            else
            {
                // Debug.Log("PLAYER MOVING");
                
                var distCovered = playerVelocity * FollowSpeedFactor(distanceBetween) * Time.deltaTime;
                
                smoothPosition = Vector2.Lerp(cameraPosition2D, targetPosition2D,
                    distCovered / distanceBetween);
            }
            
            cameraPosition = new Vector3(smoothPosition.x, smoothPosition.y, cameraPosition.z);
            this.managedCamera.transform.position = cameraPosition;

            if (this.DrawLogic)
            {
                cameraLineRenderer.enabled = true;
                DrawCameraLogic();
            }
            else
            {
                cameraLineRenderer.enabled = false;
            }
        }

        private float FollowSpeedFactor(float distanceBetween)
        {
            if (distanceBetween <= leashDistance)
            {
                return followSpeedFactor;
            }

            return 1f;
        }

        public override void DrawCameraLogic()
        {
            var z = this.Target.transform.position.z - this.managedCamera.transform.position.z;

            cameraLineRenderer.positionCount = 7;
            cameraLineRenderer.useWorldSpace = false;
            cameraLineRenderer.SetPosition(0, new Vector3(crosshairLength, 0, z));
            cameraLineRenderer.SetPosition(1, new Vector3(0, 0, z));
            cameraLineRenderer.SetPosition(2, new Vector3(0, crosshairLength, z));
            cameraLineRenderer.SetPosition(3, new Vector3(0, 0, z));
            cameraLineRenderer.SetPosition(4, new Vector3(-crosshairLength, 0, z));
            cameraLineRenderer.SetPosition(5, new Vector3(0, 0, z));
            cameraLineRenderer.SetPosition(6, new Vector3(0, -crosshairLength, z));
        }
    }
}
