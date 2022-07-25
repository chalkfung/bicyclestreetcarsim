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
        public UnityEngine.UI.Button startSimulationButton = default;
        public UnityEngine.UI.Text startSimulationButtonText = default;
        public UnityEngine.UI.Button addJunctionButton = default;
        public UnityEngine.UI.Button addStreetCarStopButton = default;
        public UnityEngine.UI.Button addStreetCarRouteButton = default;
        public UnityEngine.UI.Button addWaypointButton = default;
        public UnityEngine.UI.Button addWaypointEnd = default;
        public UnityEngine.UI.Button addBicycleRouteButton = default;
        public UnityEngine.UI.Button cancelButton = default;
        public TMPro.TMP_InputField streetCarRouteInputField = default;
        public TMPro.TMP_InputField routeScheduleInputField = default;
        [SerializeField] private Transform planeTransform = default;
        [SerializeField] private GameObject junctionPrefab = default;
        [SerializeField] private GameObject streetCarStopPrefab = default;
        [SerializeField] private GameObject streetCarWaypointStartPrefab = default;
        [SerializeField] private GameObject streetCarWaypointPrefab = default;
        [SerializeField] private GameObject streetCarWaypointEndPrefab = default;
        [SerializeField] private GameObject bicycleWaypointStartPrefab = default;
        [SerializeField] private GameObject bicycleWaypointPrefab = default;
        [SerializeField] private GameObject bicycleWaypointEndPrefab = default;
        [SerializeField] private GameObject pedestrianPrefab = default;
        private List<Waypoint> editingReferences = default;
        private Waypoint lastWaypointReference = default;
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
        private EditorStates editorState = EditorStates.Default;
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
                            if (routeScheduleInputField)
                                routeScheduleInputField.text = "";
                            routeName = "";
                            currentRouteSchedule = null;
                            lastWaypointReference = null;
                            editingReferences = null;
                            GameController.Instance.IsPlaying = false;
                            addJunctionButton.gameObject.SetActive(true);
                            addStreetCarStopButton.gameObject.SetActive(true);
                            addStreetCarRouteButton.gameObject.SetActive(true);
                            addBicycleRouteButton.gameObject.SetActive(true);
                            selectionPanel.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(true);
                            break;
                        case EditorStates.AddJunction:
                            selectionPanel.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addWaypointButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddStreetCarStop:
                            selectionPanel.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.PendingStreetCarRouteDetails:
                            streetCarRouteInputField.gameObject.SetActive(true);
                            routeScheduleInputField.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddBicycleStart:
                        case EditorStates.AddStreetCarStart:
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            mouseReleased = true;
                            break;
                        case EditorStates.AddBicycleRoute:
                        case EditorStates.AddStreetCarRoute:
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(true);
                            addWaypointEnd.gameObject.SetActive(true);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.AddBicycleWaypoint:
                        case EditorStates.AddStreetCarWaypoint:
                            cancelButton.gameObject.SetActive(true);
                            addWaypointButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            mouseReleased = true;
                            break;
                        case EditorStates.AddBicycleEnd:
                        case EditorStates.AddStreetCarEnd:
                            cancelButton.gameObject.SetActive(true);
                            addWaypointEnd.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            mouseReleased = true;
                            break; 
                        case EditorStates.PendingBicycleRouteDetails:
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(true);
                            addJunctionButton.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            addBicycleRouteButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            startSimulationButton.gameObject.SetActive(false);
                            break;
                        case EditorStates.Simulation:
                            GameController.Instance.IsPlaying = true;
                            startSimulationButton.gameObject.SetActive(true);
                            cancelButton.gameObject.SetActive(false);
                            addWaypointEnd.gameObject.SetActive(false);
                            addWaypointButton.gameObject.SetActive(false);
                            streetCarRouteInputField.gameObject.SetActive(false);
                            routeScheduleInputField.gameObject.SetActive(false);
                            addJunctionButton.gameObject.SetActive(false);
                            addStreetCarStopButton.gameObject.SetActive(false);
                            addStreetCarRouteButton.gameObject.SetActive(false);
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
            Simulation,
            AddJunction,
            AddStreetCarStop,
            AddStreetCarRoute,
            AddStreetCarStart,
            AddStreetCarWaypoint,
            AddStreetCarEnd,
            PendingStreetCarRouteDetails,
            PendingBicycleRouteDetails,
            AddBicycleRoute,
            AddBicycleStart,
            AddBicycleWaypoint,
            AddBicycleEnd
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

            if (addBicycleRouteButton != null)
            {
                addBicycleRouteButton.onClick.AddListener(OnAddBicycleRoute);
            }
            if (addWaypointButton != null)
            {
                addWaypointButton.onClick.AddListener(OnAddWaypointButton);
            }
            if (addWaypointEnd != null)
            {
                addWaypointEnd.onClick.AddListener(OnAddStreetCarEndButton);
            }
            if(routeScheduleInputField != null)
            {
                routeScheduleInputField.onSubmit.AddListener(OnInputFieldsEnter);
            }

            if (startSimulationButton)
            {
                startSimulationButton.onClick.AddListener(OnSimulationButton);
            }
            if(cancelButton != null)
            {
                cancelButton.onClick.AddListener(OnCancelButton);
            }
            if (selectionPanel != null)
            {
                selectionPanel.SelectionEndDrag += OnDragSelectionEnd;
            }

            if (startSimulationButtonText == null)
            {
                startSimulationButtonText = startSimulationButton.transform.GetComponentInChildren<UnityEngine.UI.Text>();
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
            if (startSimulationButton)
            {
                startSimulationButton.onClick.RemoveListener(OnSimulationButton);
            }
            if(cancelButton != null)
            {
                cancelButton.onClick.RemoveListener(OnCancelButton);
            }
            if (addStreetCarRouteButton != null)
            {
                addStreetCarRouteButton.onClick.RemoveListener(OnAddStreetCarRoute);
            }
            if (addWaypointButton != null)
            {
                addWaypointButton.onClick.RemoveListener(OnAddWaypointButton);
            }
            if (addWaypointEnd != null)
            {
                addWaypointEnd.onClick.RemoveListener(OnAddStreetCarEndButton);
            }
            if(routeScheduleInputField != null)
            {
                routeScheduleInputField.onSubmit.RemoveListener(OnInputFieldsEnter);
            }
        }

        private void Update()
        {
            if( EditorState == EditorStates.AddStreetCarStart || EditorState == EditorStates.AddStreetCarEnd || EditorState == EditorStates.AddStreetCarWaypoint ||
                EditorState == EditorStates.AddBicycleStart || EditorState == EditorStates.AddBicycleEnd || EditorState == EditorStates.AddBicycleWaypoint)
            {
                if (Input.GetMouseButtonDown(0) && mouseReleased)
                {
                    Vector3 worldPos = ScreenToWorldPos(Input.mousePosition);
                    mouseReleased = !mouseReleased;
                    switch(EditorState)
                    {
                        case EditorStates.AddBicycleStart:
                        case EditorStates.AddStreetCarStart:
                            //Spawn StartPoint and pass in parameters
                            GameObject startWaypoint = EditorState == EditorStates.AddStreetCarStart 
                                ? Instantiate(streetCarWaypointStartPrefab) 
                                : Instantiate(bicycleWaypointStartPrefab);
                            startWaypoint.transform.position = worldPos;
                            WaypointStart startComponent = startWaypoint.GetComponent<WaypointStart>();
                            if (startComponent)
                            {
                                startComponent.routeName = routeName;
                                startComponent.scheduleList = currentRouteSchedule;
                                startComponent.pedestrianPrefab = pedestrianPrefab;
                                startComponent.FormatSchedules();
                                lastWaypointReference = startComponent;
                            }

                            editingReferences = new List<Waypoint>();
                            editingReferences.Add(startComponent);
                            EditorState = EditorState == EditorStates.AddStreetCarStart 
                                ? EditorStates.AddStreetCarRoute 
                                : EditorStates.AddBicycleRoute;
                            break;
                        case EditorStates.AddBicycleWaypoint:
                        case EditorStates.AddStreetCarWaypoint:
                            //Spawn waypoint and pass in parameters
                            GameObject waypoint = EditorState == EditorStates.AddStreetCarWaypoint 
                                ? Instantiate(streetCarWaypointPrefab) 
                                : Instantiate(bicycleWaypointPrefab);
                            waypoint.transform.position = worldPos;
                            Waypoint waypointComponent = waypoint.GetComponent<Waypoint>();
                            if (waypointComponent)
                            {
                                if (lastWaypointReference)
                                {
                                    lastWaypointReference.nextWaypoint = waypointComponent;
                                    lastWaypointReference = waypointComponent;
                                }
                            }
                            editingReferences.Add(waypointComponent);
                            EditorState = EditorState == EditorStates.AddStreetCarWaypoint 
                                ? EditorStates.AddStreetCarRoute 
                                : EditorStates.AddBicycleRoute;
                            break;
                        case EditorStates.AddBicycleEnd:
                        case EditorStates.AddStreetCarEnd:
                            //Spawn EndPoint and pass in parameters
                            GameObject endWaypoint = EditorState == EditorStates.AddStreetCarEnd 
                                ? Instantiate(streetCarWaypointEndPrefab) 
                                : Instantiate(bicycleWaypointEndPrefab);
                            endWaypoint.transform.position = worldPos;
                            Waypoint endWaypointComponent = endWaypoint.GetComponent<Waypoint>();
                            if (endWaypointComponent)
                            {
                                endWaypointComponent.routeName = routeName;
                                if (lastWaypointReference)
                                {
                                    lastWaypointReference.nextWaypoint = endWaypointComponent;
                                }
                            }
                            editingReferences.Add(endWaypointComponent);
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
        void OnAddWaypointButton()
        {
            EditorState = EditorState == EditorStates.AddStreetCarRoute ? EditorStates.AddStreetCarWaypoint : EditorStates.AddBicycleWaypoint;
        }
        void OnAddStreetCarEndButton()
        {
            EditorState = EditorState == EditorStates.AddStreetCarRoute ? EditorStates.AddStreetCarEnd : EditorStates.AddBicycleEnd;
        }
        void OnCancelButton()
        {
            switch(EditorState)
            {
                case EditorStates.AddBicycleWaypoint:
                case EditorStates.AddBicycleEnd:
                    mouseReleased = true;
                    EditorState = EditorStates.AddBicycleRoute;
                    break;
                case EditorStates.AddStreetCarWaypoint:
                case EditorStates.AddStreetCarEnd:
                    mouseReleased = true;
                    EditorState = EditorStates.AddStreetCarRoute;
                    break;
                default:
                    if (editingReferences != null)
                    {
                        foreach(Waypoint waypoint in editingReferences)
                        {
                            DestroyImmediate(waypoint.gameObject);
                        }
                    }
                    EditorState = EditorStates.Default;
                    break;
            }
        }
        void OnAddStreetCarRoute()
        {
            EditorState = EditorStates.PendingStreetCarRouteDetails;
        }
        
        void OnAddBicycleRoute()
        {
            EditorState = EditorStates.PendingBicycleRouteDetails;
        }
        void OnInputFieldsEnter(string input)
        {
            if (streetCarRouteInputField && EditorState == EditorStates.PendingStreetCarRouteDetails)
            {
                routeName = streetCarRouteInputField.text;
            }
            String[] timings = input.Split(',');
            currentRouteSchedule = timings.Select(int.Parse).ToList();
            EditorState = (EditorState == EditorStates.PendingStreetCarRouteDetails)
                ? EditorStates.AddStreetCarStart
                : EditorStates.AddBicycleStart;
        }

        void OnSimulationButton()
        {
            EditorState = (EditorState == EditorStates.Simulation) ? EditorStates.Default : EditorStates.Simulation;
            startSimulationButtonText.text =
                (EditorState == EditorStates.Simulation) ? "Stop Simulation" : "Start Simulation";
        }
        Vector3 ScreenToWorldPos(Vector3 screenPos)
        {
            screenPos.z = Camera.main.transform.position.y - planeTransform.position.y;
            return Camera.main.ScreenToWorldPoint(screenPos);
        }
    }
}

