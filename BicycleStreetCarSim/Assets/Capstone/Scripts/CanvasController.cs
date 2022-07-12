using UnityEngine;
using System;
namespace Capstone
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance { get; private set; }
        public SelectionPanel selectionPanel = default;
        public UnityEngine.UI.Button addJunctionButton = default;
        public UnityEngine.UI.Button addStreetCarStopButton = default;
        public UnityEngine.UI.Button addStreetCarRouteButton = default;
        public UnityEngine.UI.Button addStreetCarWaypointButton = default;
        public UnityEngine.UI.Button addStreetCarWaypointEnd = default;
        public UnityEngine.UI.Button cancelButton = default;
        public TMPro.TMP_InputField streetCarRouteInputField = default;
        [SerializeField] private Transform planeTransform = default;
        [SerializeField] private GameObject junctionPrefab = default;
        [SerializeField] private GameObject streetCarStopPrefab = default;
        bool mouseReleased = true;

        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        private EditorStates editorState = default;
        public EditorStates EditorState
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
                            addJunctionButton.gameObject.SetActive(true);
                            addStreetCarStopButton.gameObject.SetActive(true);
                            addStreetCarRouteButton.gameObject.SetActive(true);
                            selectionPanel.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddJunction:
                            selectionPanel.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarStop:
                            selectionPanel.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.PendingStreetCarRouteDetails:
                            streetCarRouteInputField.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarRoute:
                            streetCarRouteInputField.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(true);
                            addStreetCarWaypointEnd.gameObject.SetActive(true);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarWaypoint:
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarEnd:
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            break;
                        default:
                            break;
                    }
                }
                editorState = value;
            }
        }
        public enum EditorStates
        {
            Default,
            AddJunction,
            AddStreetCarStop,
            AddStreetCarRoute,
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
            if (addStreetCarRouteButton != null)
            {
                addStreetCarRouteButton.onClick.AddListener(OnAddStreetCarRoute);
            }
            if (addStreetCarWaypointButton != null)
            {
                addStreetCarWaypointButton.onClick.AddListener(OnAddStreetCarWaypointButton);
            }
            if (addStreetCarWaypointEnd != null)
            {
                addStreetCarWaypointEnd.onClick.AddListener(OnAddStreetCarEndButton);
            }
            if(streetCarRouteInputField != null)
            {
                streetCarRouteInputField.onSubmit.AddListener(OnStreetCarInputFieldEnter);
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
                addStreetCarStopButton.onClick.RemoveListener(OnAddStreetCarStopButton);
            }
        }

        private void Update()
        {
            if(EditorState == EditorStates.AddStreetCarEnd || EditorState == EditorStates.AddStreetCarWaypoint)
            {
                if (Input.GetMouseButtonDown(0) && mouseReleased)
                {
                    Vector3 screenPoint = Input.mousePosition;
                    screenPoint.z = Camera.main.transform.position.y - planeTransform.position.y;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPoint);
                    Debug.Log("ScreenPoint: " + screenPoint);
                    mouseReleased = !mouseReleased;
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
        void OnAddStreetCarWaypointButton()
        {
            EditorState = EditorStates.AddStreetCarWaypoint;
        }
        void OnAddStreetCarEndButton()
        {
            EditorState = EditorStates.AddStreetCarEnd;
        }
        void OnCancelButton()
        {
            switch(EditorState)
            {
                case EditorStates.AddStreetCarWaypoint:
                case EditorStates.AddStreetCarEnd:
                    mouseReleased = true;
                    EditorState = EditorStates.AddStreetCarRoute;
                    break;
                default:
                    EditorState = EditorStates.Default;
                    break;
            }
        }
        void OnAddStreetCarRoute()
        {
            EditorState = EditorStates.PendingStreetCarRouteDetails;
        }
        void OnStreetCarInputFieldEnter(string input)
        {
            EditorState = EditorStates.AddStreetCarRoute;
        }
    }
}

