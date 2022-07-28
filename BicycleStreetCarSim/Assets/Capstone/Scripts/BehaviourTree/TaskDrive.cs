using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class TaskDrive : Node
    {
        private Transform transform = default;
        private Transform[] waypoints = default;
        private int currentWaypointIndex = 0;
        private static int pedestrianLayer = 1 << 13;
        public string route = default;
        public int startTime = 0;
        public TaskDrive(Transform transform, Transform[] waypoints, string route, int startTime)
        {
            this.transform = transform;
            this.waypoints = waypoints;
            this.route = route;
            this.startTime = startTime;
        }
        
        public override NodeState Evaluate()
        {
            if (waypoints == null || waypoints.Length == 0)
                return NodeState.Success;
            
            Transform nextWaypoint = waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, nextWaypoint.position) < 2.0f)
            {
                if (currentWaypointIndex + 1 == waypoints.Length)
                {
                    //ClearData("currentWaypointIndex");
                    Object.Destroy(transform.gameObject);
                    if (route != "")
                    {
                        Debug.Log("Street Car route: " + route+ "completed, startTiming: " + startTime + ", end Timing:"  + GameController.Instance.Timer() + "in minutes:" + (GameController.Instance.Timer() - startTime) /60);
                    }
                    UnityEngine.Object.Destroy(this.transform.gameObject);
                    return NodeState.Success;
                }
                else
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                    parent.SetData("currentWaypointIndex", currentWaypointIndex);
                }
            }
            else
            {
                Collider[] pedColliders = Physics.OverlapBox(transform.position, transform.localScale / 2f,
                    Quaternion.identity, pedestrianLayer);
                if (pedColliders.Length < 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, StreetCarBehaviourTree.speed * Time.deltaTime);
                    transform.LookAt(nextWaypoint.position);
                }
                
            }
    
            state = NodeState.Running;
            return state;
        }

    }
}