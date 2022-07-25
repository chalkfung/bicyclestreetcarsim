using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class TaskWalk : Node
    {
        private Transform transform = default;
        private Transform endPoint = default;

        public TaskWalk(Transform transform, Transform endPoint)
        {
            this.transform = transform;
            this.endPoint = endPoint;
        }
        
        public override NodeState Evaluate()
        {
            if (Vector3.Distance(transform.position, endPoint.position) < 0.01f)
            {
                //transform.GetComponent<Animator>().SetBool("BasicMotions@Idle01", true);
                //transform.GetComponent<Animator>().SetBool("BasicMotions@Walk01", false);
                transform.position = Vector3.MoveTowards(transform.position, endPoint.position,0);
                transform.LookAt(endPoint.position);
                return NodeState.Success;
            }
            
            //transform.GetComponent<Animator>().SetBool("BasicMotions@Walk01", true);
            
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, PedestrianBehaviourTree.speed * Time.deltaTime);
                transform.LookAt(endPoint.position);

            state = NodeState.Running;
            return state;
        }
    }
}