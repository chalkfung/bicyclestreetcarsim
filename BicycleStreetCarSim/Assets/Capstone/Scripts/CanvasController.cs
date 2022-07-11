using UnityEngine;
using System;
namespace Capstone
{
    public class CanvasController : MonoBehaviour
    {
        public SelectionPanel selectionPanel = default;
        public UnityEngine.UI.Button addJunctionButton = default;
        public UnityEngine.UI.Button addStreetCarStopButton = default;
        public UnityEngine.UI.Button addStreetCarStartButton = default;
        public UnityEngine.UI.Button addStreetCarWaypointButton = default;
        public UnityEngine.UI.Button addStreetCarWaypointEnd = default;
        public UnityEngine.UI.Button cancelButton = default;
        [SerializeField] private Transform planeTransform = default;
        [SerializeField] private GameObject junctionPrefab = default;
        [SerializeField] private GameObject streetCarStopPrefab = default;
        private EditorStates editorState = default;
        private EditorStates EditorState
        {
            get
            {
                return editorState;
            }
            set
            {
                if(selectionPanel != null)
                {
                    switch (value)
                    {
                        case EditorStates.Default:
                            selectionPanel.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddJunction:
                            selectionPanel.gameObject.SetActive(true);
                            break;
                        case EditorStates.AddStreetCarStop:
                            selectionPanel.gameObject.SetActive(true);
                            break;
                        case EditorStates.AddStreetCarStart:
                            break;
                        case EditorStates.AddStreetCarWaypoint:
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(true);
                            break;
                        case EditorStates.AddStreetCarEnd:
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(true);
                            break;
                        default:
                            break;
                    }
                }
                editorState = value;
            }
        }
        enum EditorStates
        {
            Default,
            AddJunction,
            AddStreetCarStop,
            AddStreetCarStart,
            AddStreetCarWaypoint,
            AddStreetCarEnd,
            PendingStreetCarRouteDetails
        }
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
            if (addStreetCarStopButton != null)
            {
                addStreetCarStopButton.onClick.AddListener(OnAddStreetCarStopButton);
            }
            if (addStreetCarStartButton != null)
            {
                addStreetCarStartButton.onClick.AddListener(OnAddStreetCarStart);
            }
            if (addStreetCarWaypointButton != null)
            {
                addStreetCarWaypointButton.onClick.AddListener(OnAddStreetCarStopButton);
            }
            if (addStreetCarWaypointEnd != null)
            {
                addStreetCarWaypointEnd.onClick.AddListener(OnAddStreetCarStopButton);
            }
            if(cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelButton);
            }
            if (selectionPanel != null)
            {
                selectionPanel.SelectionEndDrag += OnDragSelectionEnd;
            }
            EditorState = EditorStates.Default;
        }
        private void OnDisable()
        {
            if(selectionPanel)
            {
                selectionPanel.SelectionEndDrag -= OnDragSelectionEnd;
            }
            if (addJunctionButton != null)
            {
                addJunctionButton.onClick.RemoveListener(OnAddJunctionButton);
            }
            if (addStreetCarStopButton != null)
            {
                addStreetCarStopButton.onClick.AddListener(OnAddStreetCarStopButton);
            }
        }

        private void Update()
        {
            if(EditorState == EditorStates.AddStreetCarStart)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 screenPoint = Input.mousePosition;
                    screenPoint.z = Camera.main.transform.position.y - planeTransform.position.y;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
                    Debug.Log("ScreenPoint: " + screenPoint);
                    EditorState = EditorStates.AddStreetCarWaypoint;
                }
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
            if (args is SelectionPanel.SelectionPanelEventArgs selectionArgs)
            {
                switch (EditorState)
                {
                    case EditorStates.AddJunction:
                        SpawnTrafficJunctionCube(selectionArgs);
                        break;
                    case EditorStates.AddStreetCarStop:
                        SpawnStreetCarStop(selectionArgs);
                        break;
                    default:
                        break;
                }
                
            }
            EditorState = EditorStates.Default;
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

        void SpawnStreetCarStop(SelectionPanel.SelectionPanelEventArgs selectionArgs)
        {
            float screenPointZ = Camera.main.transform.position.y - planeTransform.position.y;
            Vector3 startPos = selectionArgs.startPos;
            Vector3 endPos = selectionArgs.endPos;
            startPos.z = endPos.z = screenPointZ;
            Vector3 startWorldPos = Camera.main.ScreenToWorldPoint(startPos);
            Vector3 endWorldPos = Camera.main.ScreenToWorldPoint(endPos);
            float width = Math.Abs(endWorldPos.x - startWorldPos.x);
            float height = Math.Abs(endWorldPos.z - startWorldPos.z);
            Vector3 centrePoint = new Vector3((startWorldPos.x + endWorldPos.x) / 2, 1.0f, (startWorldPos.z + endWorldPos.z) / 2);
            GameObject stop = Instantiate(streetCarStopPrefab);
            stop.transform.position = centrePoint;
            stop.transform.eulerAngles = new Vector3(0.0f, selectionArgs.cameraYAngle, 0.0f);
            stop.transform.localScale = new Vector3(width, 1.0f, height);
        }

        void OnAddJunctionButton()
        {
            EditorState = EditorStates.AddJunction;
        }

        void OnAddStreetCarStopButton()
        {
            EditorState = EditorStates.AddStreetCarStop;
        }
        void OnCancelButton()
        {
            EditorState = EditorStates.Default;
        }
        void OnAddStreetCarStart()
        {
            EditorState = EditorStates.PendingStreetCarRouteDetails;
        }
    }
}

