using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class TaskWaitTrafficLight : Node
    {
        private Transform transform;

        public TaskWaitTrafficLight(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target != null)
            {
                if (target.GetComponent<MeshRenderer>().material.color == Color.red)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target.position,
                        0.0f);
                    transform.LookAt(target.position);
                    state = NodeState.Running;
                    return state;
                }
                else
                {
                    ClearData("target");
                    state = NodeState.Success;
                    return state;
                }
            }
            ClearData("target");
            state = NodeState.Success;
            return state;
        }
    }

}
