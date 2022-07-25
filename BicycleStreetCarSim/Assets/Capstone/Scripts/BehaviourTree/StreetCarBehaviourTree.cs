using System.Collections.Generic;
using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public class StreetCarBehaviourTree : Tree
    {
        public UnityEngine.Transform[] nextWaypoints = default;
        public GameObject pedestrianPrefab = default;
        public static int alightingPedestrian = 5;
        public static int boardingPedestrian = 5;
        public static float speed = 2f;
        public static float fovRange = 6.0f;

        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>()
                    {
                        new CheckArrivedStreetCarStop(transform),
                        new TaskWaitPedestrianAlight(transform, pedestrianPrefab),
                        new TaskLeaveStreetCarStop(transform, nextWaypoints)
                    }
                ),
                new Sequence(new List<Node>
                {
                    new CheckJunctionInFOV(transform),
                    new TaskWaitTrafficLight(transform),
                }),
                new TaskDrive(transform, nextWaypoints),
            });
            return root;
        }
    }
}