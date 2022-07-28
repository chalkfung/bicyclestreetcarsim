﻿using System.Collections.Generic;

namespace Capstone.Scripts.BehaviourTree
{
    public class BicycleBehaviourTree : Tree
    {
        public UnityEngine.Transform[] nextWaypoints = default;
        public static float speed = 2f;
        public static float fovRange = 6.0f;
        public string routeName = default;
        public int startTime = default;
        protected override Node SetupTree()
        {
            Node root = new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new CheckJunctionInFOV(transform),
                    new TaskWaitTrafficLight(transform),
                }),
                new TaskDrive(transform, nextWaypoints,  routeName, startTime),
            });
            return root;
        }
    }
}