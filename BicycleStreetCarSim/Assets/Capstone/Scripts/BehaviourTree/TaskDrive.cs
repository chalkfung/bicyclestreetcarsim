using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class TaskDrive : Node
    {
        private Transform transform = default;
        private Transform[] waypoints = default;
        private float speed = default;
        private int currentWaypointIndex = 0;
        
        public TaskDrive(Transform transform, Transform[] waypoints, float speed)
        {
            this.transform = transform;
            this.waypoints = waypoints;
            this.speed = speed;
        }
        
        public override NodeState Evaluate()
        {
            Transform nextWaypoint = waypoints[currentWaypointIndex];
            if (Vector3.Distance(transform.position, nextWaypoint.position) < 0.01f)
            {
                if (currentWaypointIndex + 1 == waypoints.Length)
                {
                    return NodeState.Success;
                }
                else
                {
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, this.speed * Time.deltaTime);
                transform.LookAt(nextWaypoint.position);
            }
    
            state = NodeState.Running;
            return state;
        }

    }
}