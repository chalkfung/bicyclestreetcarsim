using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public float speed = 20;
        public float rotateSpeed = 5;
        public float zoomSpeed = 5;
        public Vector2 motion = default;
        public Transform cameraTransform = default;

        void Update()
        {
            if(CanvasController.Instance.EditorState != CanvasController.EditorStates.PendingStreetCarRouteDetails)
            {
                motion = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (cameraTransform != null)
                {
                    float newDeltaTime = (Time.timeScale == 0.0f) ? Time.deltaTime : Time.deltaTime / Time.timeScale;
                    cameraTransform.Translate(motion * (speed * newDeltaTime));
                    Vector3 rotateAngles = cameraTransform.rotation.eulerAngles;
                    if (Input.GetKey(KeyCode.Q))
                    {
                        rotateAngles.y -= rotateSpeed * newDeltaTime;

                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        rotateAngles.y += rotateSpeed * newDeltaTime;
                    }

                    if (Input.GetKey(KeyCode.Minus))
                    {
                        Vector3 tmp = cameraTransform.position;
                        tmp.y += zoomSpeed * newDeltaTime;
                        cameraTransform.position = tmp;
                    }
                    if (Input.GetKey(KeyCode.Equals))
                    {
                        Vector3 tmp = cameraTransform.position;
                        tmp.y -= zoomSpeed * newDeltaTime;
                        cameraTransform.position = tmp;
                    }
                    cameraTransform.rotation = Quaternion.Euler(rotateAngles);

                }
            }
            
        }
    }

}
