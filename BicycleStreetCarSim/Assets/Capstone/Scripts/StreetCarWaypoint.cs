using UnityEngine;
namespace Capstone.Scripts
{
    public class StreetCarWaypoint : MonoBehaviour
    {
        public string routeName { get; set; } = default;
        public StreetCarWaypoint nextWaypoint { get; set; } = default;
    }
}

