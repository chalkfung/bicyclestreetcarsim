using UnityEngine;
using System;
namespace Capstone
{
    public class CanvasController : MonoBehaviour
    {
        public SelectionPanel selectionPanel = default;
        public UnityEngine.UI.Button addJunctionButton = default;
        [SerializeField] private Transform planeTransform = default;
        [SerializeField] private GameObject junctionPrefab = default;
        private void Start()
        {
            if(selectionPanel != null)
            {
                selectionPanel.gameObject.SetActive(false);
            }
        }
        private void OnEnable()
        {
            if (addJunctionButton != null)
            {
                addJunctionButton.onClick.AddListener(OnAddJunctionButton);
            }
            if(selectionPanel != null)
            {
                selectionPanel.SelectionEndDrag += OnDragSelectionEnd;
            }
        }
        private void OnDisable()
        {
            if (addJunctionButton != null)
            {
                addJunctionButton.onClick.RemoveListener(OnAddJunctionButton);
                selectionPanel.SelectionEndDrag -= OnDragSelectionEnd;
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 screenPoint = Input.mousePosition;
                //screenPoint.z = Camera.main.transform.position.y - planeTransform.position.y;
                //Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
                Debug.Log("ScreenPoint: " + screenPoint);
            }
            //if (Input.GetMouseButtonUp(0))
            //{
            //    if (selectionPanel != null)
            //    {
            //        selectionPanel.SetActive(false);
            //    }
            //}
        }

        void OnDragSelectionEnd(object source, EventArgs args)
        {
            SelectionPanel.SelectionPanelEventArgs selectionArgs = args as SelectionPanel.SelectionPanelEventArgs;
            if(selectionArgs != null)
            {
                SpawnTrafficJunctionCube(selectionArgs);
            }
            if(selectionPanel != null)
            {
                selectionPanel.gameObject.SetActive(false);
            }
        }

        void SpawnTrafficJunctionCube(SelectionPanel.SelectionPanelEventArgs selectionArgs)
        {
            float screenPointZ = Camera.main.transform.position.y - planeTransform.position.y;
            Vector3 startPos = selectionArgs.startPos;
            Vector3 endPos = selectionArgs.endPos;
            startPos.z = endPos.z = screenPointZ;
            Vector3 startWorldPos = Camera.main.ScreenToWorldPoint(startPos);
            Vector3 endWorldPos = Camera.main.ScreenToWorldPoint(endPos);
            float junctionWidth = Math.Abs(endWorldPos.x - startWorldPos.x);
            float junctionHeight = Math.Abs(endWorldPos.z - startWorldPos.z);
            Vector3 centrePoint = new Vector3((startWorldPos.x + endWorldPos.x) / 2, 1.0f, (startWorldPos.z + endWorldPos.z) / 2);
            GameObject junctionGO = Instantiate(junctionPrefab);
            junctionGO.transform.position = centrePoint;
            junctionGO.transform.eulerAngles = new Vector3(0.0f, selectionArgs.cameraYAngle, 0.0f);
            junctionGO.transform.localScale = new Vector3(junctionWidth, 1.0f, junctionHeight);
        }

        void OnAddJunctionButton()
        {
            if(selectionPanel != null)
            {
                selectionPanel.gameObject.SetActive(true);
            }
        }
    }
}

