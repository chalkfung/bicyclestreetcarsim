using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class CheckJunctionInFOV : Node
    {
        private Transform transform;
        private static int trafficLightMask = 1 << 10;
        private static int junctionMask = 1 << 9;
        public CheckJunctionInFOV(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                Collider[] trafficLightColliders = Physics.OverlapSphere(
                    transform.position, StreetCarBehaviourTree.fovRange, trafficLightMask);
                
                Collider[] junctionColliders = Physics.OverlapSphere(
                    transform.position, 0.0f, junctionMask);
                
                if (trafficLightColliders.Length > 0 
                    && trafficLightColliders[0].transform.GetComponent<MeshRenderer>().material.color == Color.red 
                    && junctionColliders.Length == 0)
                {
                    parent.parent.SetData("target", trafficLightColliders[0].transform);
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
