
using UnityEngine;
namespace Capstone.Scripts
{
    public class GameController : MonoBehaviour
    {
        private bool isPlaying = false;
        
        // 1 sec in real time is 4 min in game
        private float timeScale = 15.0f; 
        private float timer = 0.0f;
        private void OnEnable()
        {
            IsPlaying = true;
        }
        private void Update()
        {
            timer += timeScale * Time.deltaTime;
            Debug.Log(timer);
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
                }
                else
                {
                    Time.timeScale = 0;
                }
            }
        }
    }
}
