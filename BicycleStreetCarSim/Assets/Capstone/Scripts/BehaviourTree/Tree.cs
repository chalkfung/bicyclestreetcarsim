using UnityEngine;

namespace Capstone.Scripts.BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {

        private Node root = null;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if (root != null && GameController.Instance.IsPlaying)
            {
                root.Evaluate();
            }
                
        }

        protected abstract Node SetupTree();

    }
}