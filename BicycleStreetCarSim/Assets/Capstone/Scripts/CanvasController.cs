using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Capstone.Scripts
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
        public TMPro.TMP_InputField streetCarRouteScheduleInputField = default;
        [SerializeField] private Transform planeTransform = default;
        [SerializeField] private GameObject junctionPrefab = default;
        [SerializeField] private GameObject streetCarStopPrefab = default;
        [SerializeField] private GameObject streetCarWaypointStartPrefab = default;
        [SerializeField] private GameObject streetCarWaypointPrefab = default;
        [SerializeField] private GameObject streetCarWaypointEndPrefab = default;
        private StreetCarWaypoint lastWaypointReference = default;
        private String routeName = default;
        private List<int> currentRouteSchedule = default;
        private bool mouseReleased = true;
        

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
                            if (streetCarRouteInputField)
                                streetCarRouteInputField.text = "";
                            if (streetCarRouteScheduleInputField)
                                streetCarRouteScheduleInputField.text = "";
                            routeName = "";
                            currentRouteSchedule = null;
                            lastWaypointReference = null;
                            addJunctionButton.gameObject.SetActive(true);
                            addStreetCarStopButton.gameObject.SetActive(true);
                            addStreetCarRouteButton.gameObject.SetActive(true);
                            selectionPanel.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddJunction:
                            selectionPanel.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarStop:
                            selectionPanel.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
                            break;
                        case EditorStates.PendingStreetCarRouteDetails:
                            streetCarRouteInputField.gameObject.SetActive(true);
                            streetCarRouteScheduleInputField.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarStart:
                            streetCarRouteInputField.gameObject.SetActive(false);
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            mouseReleased = true;
                            break;
                        case EditorStates.AddStreetCarRoute:
                            streetCarRouteInputField.gameObject.SetActive(false);
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
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
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
                            mouseReleased = true;
                            break;
                        case EditorStates.AddStreetCarEnd:
                            cancelButton.gameObject.SetActive(true);
                            addStreetCarWaypointEnd.gameObject.SetActive(false);
                            addStreetCarWaypointButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            streetCarRouteScheduleInputField.gameObject.SetActive(false);
                            mouseReleased = true;
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
            if(streetCarRouteScheduleInputField != null)
            {
                streetCarRouteScheduleInputField.onSubmit.AddListener(OnStreetCarInputFieldEnter);
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
            if( EditorState == EditorStates.AddStreetCarStart || EditorState == EditorStates.AddStreetCarEnd || EditorState == EditorStates.AddStreetCarWaypoint)
            {
                if (Input.GetMouseButtonDown(0) && mouseReleased)
                {
                    Vector3 worldPos = ScreenToWorldPos(Input.mousePosition);
                    mouseReleased = !mouseReleased;
                    switch(EditorState)
                    {
                        case EditorStates.AddStreetCarStart:
                            //Spawn StartPoint and pass in parameters
                            GameObject startWaypoint = Instantiate(streetCarWaypointStartPrefab);
                            startWaypoint.transform.position = worldPos;
                            StreetCarWaypointStart startComponent = startWaypoint.GetComponent<StreetCarWaypointStart>();
                            if (startComponent)
                            {
                                startComponent.routeName = routeName;
                                startComponent.scheduleList = currentRouteSchedule;
                            }
                            EditorState = EditorStates.AddStreetCarRoute;
                            break;
                        case EditorStates.AddStreetCarWaypoint:
                            //Spawn waypoint and pass in parameters
                            GameObject waypoint = Instantiate(streetCarWaypointPrefab);
                            waypoint.transform.position = worldPos;
                            StreetCarWaypoint waypointComponent = waypoint.GetComponent<StreetCarWaypoint>();
                            if (waypointComponent)
                            {
                                waypointComponent.routeName = routeName;
                                if (lastWaypointReference)
                                {
                                    lastWaypointReference.nextWaypoint = waypointComponent;
                                }
                            }
                            EditorState = EditorStates.AddStreetCarRoute;
                            break;
                        case EditorStates.AddStreetCarEnd:
                            //Spawn EndPoint and pass in parameters
                            GameObject endWaypoint = Instantiate(streetCarWaypointEndPrefab);
                            endWaypoint.transform.position = worldPos;
                            StreetCarWaypoint endwWaypointComponent = endWaypoint.GetComponent<StreetCarWaypoint>();
                            if (endwWaypointComponent)
                            {
                                endwWaypointComponent.routeName = routeName;
                                if (lastWaypointReference)
                                {
                                    lastWaypointReference.nextWaypoint = endwWaypointComponent;
                                }
                            }
                            EditorState = EditorStates.Default;
                            break;
                        default:
                            break;
                    }
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
            if (streetCarRouteInputField)
            {
                routeName = streetCarRouteInputField.text;
            }
            String[] timings = input.Split(',');
            currentRouteSchedule = timings.Select(int.Parse).ToList();
            EditorState = EditorStates.AddStreetCarStart;
        }

        Vector3 ScreenToWorldPos(Vector3 screenPos)
        {
            screenPos.z = Camera.main.transform.position.y - planeTransform.position.y;
            return Camera.main.ScreenToWorldPoint(screenPos);
        }
    }
}

