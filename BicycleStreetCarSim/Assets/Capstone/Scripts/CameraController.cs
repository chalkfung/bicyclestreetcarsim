using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Capstone
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
                    cameraTransform.Translate(motion * speed * Time.deltaTime);
                    Vector3 rotateAngles = cameraTransform.rotation.eulerAngles;
                    if (Input.GetKey(KeyCode.Q))
                    {
                        rotateAngles.y -= rotateSpeed * Time.deltaTime;

                    }
                    if (Input.GetKey(KeyCode.E))
                    {
                        rotateAngles.y += rotateSpeed * Time.deltaTime;
                    }

                    if (Input.GetKey(KeyCode.Minus))
                    {
                        Vector3 tmp = cameraTransform.position;
                        tmp.y += zoomSpeed * Time.deltaTime;
                        cameraTransform.position = tmp;
                    }
                    if (Input.GetKey(KeyCode.Equals))
                    {
                        Vector3 tmp = cameraTransform.position;
                        tmp.y -= zoomSpeed * Time.deltaTime;
                        cameraTransform.position = tmp;
                    }
                    cameraTransform.rotation = Quaternion.Euler(rotateAngles);

                }
            }
            
        }
    }

}
