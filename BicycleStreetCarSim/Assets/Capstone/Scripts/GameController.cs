
using UnityEngine;
namespace Capstone.Scripts
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; private set; }
        private bool isPlaying = false;

        public GameObject StreetCarPrefab = default;
        // 1 sec in real time is 4 min in game
        private float timeScale = 15.0f; 
        private float timer = 0.0f;
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }
        private void OnEnable()
        {
            IsPlaying = false;
        }
        private void Update()
        {
            timer += Time.deltaTime;
        }

        public float Timer()
        {
            return timer;
        }
        public bool IsPlaying
        {
            get 
            {
                return isPlaying;
            }
            set {
                isPlaying = value;
                if (value)
                {
                    Time.timeScale = timeScale;
                    timer = 0.0f;
                }
                else
                {
                    Time.timeScale = 1.0f;
                }
            }
        }
    }
}
