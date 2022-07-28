using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Capstone.Scripts.BehaviourTree
{
    public class WaitBicycle
    {
        public DateTime start;
        public bool waited;
    }
    public class TaskWalk : Node
    {
        private Transform transform = default;
        private Transform endPoint = default;
        private float pedMaxDistance = 1.0f;
        private float bikeMaxDistance = 3.0f;
        private static int pedestrianLayer = 1 << 13;
        private static int bicycleLayer = 1 << 14;
        private Dictionary<Collider, WaitBicycle> waitBicycles;
        public TaskWalk(Transform transform, Transform endPoint)
        {
            this.transform = transform;
            this.endPoint = endPoint;
        }
        
        public override NodeState Evaluate()
        {
            if (endPoint == null)
            {
                return NodeState.Success;
            }
            
            if (Vector3.Distance(transform.position, endPoint.position) < 0.01f)
            {
                //transform.GetComponent<Animator>().SetBool("BasicMotions@Idle01", true);
                //transform.GetComponent<Animator>().SetBool("BasicMotions@Walk01", false);
                transform.position = Vector3.MoveTowards(transform.position, endPoint.position,0);
                transform.LookAt(endPoint.position);
                Object.Destroy(this.transform.gameObject);
                return NodeState.Success;
            }
            
            //transform.GetComponent<Animator>().SetBool("BasicMotions@Walk01", true);
           
            
            if (GameController.Instance.IsPlaying)
            {
                Collider[] pedColliders = Physics.OverlapSphere(
                    transform.position + Vector3.Normalize(endPoint.transform.position - transform.position) * pedMaxDistance * 0.5f , pedMaxDistance, pedestrianLayer);
                Collider[] bikeColliders = Physics.OverlapSphere(
                    transform.position + Vector3.Normalize(endPoint.transform.position - transform.position) *
                    bikeMaxDistance * 0.5f, bikeMaxDistance, bicycleLayer);

                if (waitBicycles == null)
                    waitBicycles = new Dictionary<Collider, WaitBicycle>();

                foreach (var bikeCollider in bikeColliders)
                {
                    if (waitBicycles.ContainsKey(bikeCollider))
                    {
                        if ((DateTime.Now - waitBicycles[bikeCollider].start).Seconds > 1.0f)
                        {
                            waitBicycles[bikeCollider].waited = true;
                        }
                    }
                    else
                    {
                        waitBicycles.Add(bikeCollider, new WaitBicycle()
                        {
                            start = DateTime.Now,
                            waited = false
                        });
                    }
                }

                bool go = true;
                foreach (var key in waitBicycles.Keys)
                {
                    if (!waitBicycles[key].waited)
                    {
                        go = false;
                        break;
                    }
                }

                if (go && pedColliders.Length < 2)
                {
                    transform.position = Vector3.MoveTowards(transform.position, endPoint.position, PedestrianBehaviourTree.speed * Time.deltaTime);
                    transform.LookAt(endPoint.position);
                }
            }
            

            state = NodeState.Running;
            return state;
        }
    }
}