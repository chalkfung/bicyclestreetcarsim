namespace Capstone.Scripts.BehaviourTree
{
    public class PedestrianBehaviourTree : Tree
    {
        public UnityEngine.Transform endPoint = default;

        public static float speed = 0.5f;

        protected override Node SetupTree()
        {
            Node root = new TaskWalk(transform, endPoint);
            return root;
        }
    }
}