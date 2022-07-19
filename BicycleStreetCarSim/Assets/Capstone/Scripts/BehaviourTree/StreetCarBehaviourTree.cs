namespace Capstone.Scripts.BehaviourTree
{
    public class StreetCarBehaviourTree : Tree
    {
        public UnityEngine.Transform[] nextWaypoints = default;

        public static float speed = 2f;

        protected override Node SetupTree()
        {
            Node root = new TaskDrive(transform, nextWaypoints, speed);
            return root;
        }
        
    }
}