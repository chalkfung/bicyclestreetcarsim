using UnityEngine;
namespace Capstone.Scripts.BehaviourTree
{
    public class TaskWaitPedestrianAlight : Node
    {
        private Transform transform;
        private int counter = 0;
        private static int pedestrianMask = 13;

        public TaskWaitPedestrianAlight(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target != null)
            {
                if (counter < StreetCarBehaviourTree.alightingPedestrian)
                {
                    Collider[] pedestrianCollider = Physics.OverlapSphere(transform.position, 1.0f, pedestrianMask);
                    if (pedestrianCollider.Length == 1)
                    {
                        if (StreetCarBehaviourTree.pedestrianPrefab != null)
                        {
                            GameObject pedestrian = StreetCarBehaviourTree.Instantiate(StreetCarBehaviourTree.pedestrianPrefab);
                            Transform[] spawnPoints = target.GetComponentsInChildren<Transform>();
                            Transform closest = spawnPoints.Length > 0 ? spawnPoints[0] : null;
                            float distance = float.MaxValue;
                            foreach (Transform childTransform in spawnPoints)
                            {
                                if (childTransform != target.GetComponent<Transform>() && Vector3.Distance(transform.position, childTransform.position) < distance)
                                {
                                    closest = childTransform;
                                    distance = Vector3.Distance(transform.position, childTransform.position);
                                }

                            }

                            pedestrian.GetComponent<PedestrianBehaviourTree>().endPoint = closest;
                            ++counter;
                        }
                    }
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target.position,
                        0.0f);
                    state = NodeState.Running;
                    return state;
                }
                else
                {
                    counter = 0;
                    ClearData("target");
                    state = NodeState.Success;
                    return state;
                }
            }

            counter = 0;
            ClearData("target");
            state = NodeState.Success;
            return state;
        }
    }
}