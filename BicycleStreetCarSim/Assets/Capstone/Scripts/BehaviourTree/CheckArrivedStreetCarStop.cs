using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class CheckArrivedStreetCarStop : Node
    {
        private Transform transform;
        private static int waypointMask = 1 << 11;
        private static int streetCarStopMask = 1 << 12;
        public CheckArrivedStreetCarStop(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                Collider[] waypointCollider = Physics.OverlapSphere(
                    transform.position, 1.0f, waypointMask);
                
                Collider[] streetCarStopCollider = Physics.OverlapSphere(
                    transform.position, 0.0f, streetCarStopMask);
                
                if (waypointCollider.Length > 0 
                    && streetCarStopCollider.Length > 0)
                {
                    parent.parent.SetData("target", streetCarStopCollider[0].transform);
                    state = NodeState.Success;
                    return state;
                }
                state = NodeState.Failure;
                return state;
            }

            state = NodeState.Success;
            return state;

        }
    }
}