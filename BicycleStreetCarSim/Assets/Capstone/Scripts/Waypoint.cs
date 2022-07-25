using UnityEngine;
namespace Capstone.Scripts
{
    public class Waypoint : MonoBehaviour
    {
        public string routeName { get; set; } = default;
        public Waypoint nextWaypoint { get; set; } = default;
    }
}

