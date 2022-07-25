using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class TaskDrive : Node
    {
        private Transform transform = default;
        private Transform[] waypoints = default;
        private int currentWaypointIndex = 0;
        
        public TaskDrive(Transform transform, Transform[] waypoints)
        {
            this.transform = transform;
            this.waypoints = waypoints;
        }
        
        public override NodeState Evaluate()
        {
            Transform nextWaypoint = waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, nextWaypoint.position) < 2.0f)
            {
                if (currentWaypointIndex + 1 == waypoints.Length)
                {
                    //ClearData("currentWaypointIndex");
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
                transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, StreetCarBehaviourTree.speed * Time.deltaTime);
                transform.LookAt(nextWaypoint.position);
            }
    
            state = NodeState.Running;
            return state;
        }

    }
}