using UnityEngine;
namespace Capstone.Scripts.BehaviourTree
{
    public class TaskWaitPedestrianAlight : Node
    {
        private Transform transform;
        private int counter = 0;
        private static int pedestrianMask = 1 << 13;
        private GameObject pedestrianPrefab = default;
        public TaskWaitPedestrianAlight(Transform transform, GameObject pedestrianPrefab)
        {
            this.transform = transform;
            this.pedestrianPrefab = pedestrianPrefab;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target != null)
            {
                if (counter < StreetCarBehaviourTree.alightingPedestrian)
                {
                    Collider[] pedestrianCollider = Physics.OverlapSphere(transform.position, 1.0f, pedestrianMask);
                    if (pedestrianCollider.Length < 1)
                    {
                        if (pedestrianPrefab != null)
                        {
                            GameObject pedestrian = StreetCarBehaviourTree.Instantiate(pedestrianPrefab);
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

                            pedestrian.transform.position = transform.position;
                            pedestrian.GetComponent<PedestrianBehaviourTree>().endPoint = closest;
                            
                            ++counter;
                        }
                    }
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target.position,
                        0.0f);
                    parent.SetData("alighting", counter);
                    state = NodeState.Running;
                    return state;
                }
                else
                {
                    counter = 0;
                    state = NodeState.Success;
                    return state;
                }
            }

            counter = 0;
            state = NodeState.Success;
            return state;
        }
    }
}