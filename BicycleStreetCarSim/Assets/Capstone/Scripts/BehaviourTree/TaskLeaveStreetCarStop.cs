using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class TaskLeaveStreetCarStop : Node
    {
        private Transform transform = default;
        private Transform[] waypoints = default;
        private static int streetCarStopMask = 1 << 12;
        public TaskLeaveStreetCarStop(Transform transform, Transform[] waypoints)
        {
            this.transform = transform;
            this.waypoints = waypoints;
        }

        public override NodeState Evaluate()
        {
            int currentWaypointIndex = (int)GetData("currentWaypointIndex");
            int alighting = (int)GetData("alighting");
            if (currentWaypointIndex > -1 && alighting >= StreetCarBehaviourTree.alightingPedestrian)
            {
                Transform nextWaypoint = waypoints[currentWaypointIndex];
                Collider[] streetCarStopColliders = Physics.OverlapSphere(
                    transform.position, 1.0f, streetCarStopMask);
                if (streetCarStopColliders.Length > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position,
                        StreetCarBehaviourTree.speed * Time.deltaTime);
                    transform.LookAt(nextWaypoint.position);
                }
                else
                {
                    parent.SetData("alighting", 0);
                    ClearData("target");
                    state = NodeState.Success;
                    return state;
                }

                ClearData("target");
                state = NodeState.Running;
                return state;
            }
            ClearData("target");
            state = NodeState.Running;
            return state;
        }
    }
}