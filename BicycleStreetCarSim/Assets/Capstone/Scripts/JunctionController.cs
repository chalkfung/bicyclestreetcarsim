using System;
using System.Collections;
using System.Collections.Generic;
using Capstone.Scripts;
using UnityEngine;

namespace Capstone
{
    public class JunctionController : MonoBehaviour
    {
        private Transform topIndicator = default;
        private Transform bottomIndicator = default;
        private Transform leftIndicator = default;
        private Transform rightIndicator = default;
        private JunctionState junctionState = default;
        private float repeatRateInSeconds = 120.0f;
        private float timer = 0.0f;
        enum JunctionState
        {
            TopBottom,
            LeftRight
        }

        private void OnEnable()
        {
            topIndicator = this.transform.Find("TopTrafficIndicator");
            bottomIndicator = this.transform.Find("BottomTrafficIndicator");
            leftIndicator = this.transform.Find("LeftTrafficIndicator");
            rightIndicator = this.transform.Find("RightTrafficIndicator");
            junctionState = JunctionState.TopBottom;
            ChangeTrafficLights();
        }

        private void Update()
        {
            if (GameController.Instance.IsPlaying)
            {
                if (timer < repeatRateInSeconds)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0.0f;
                    ChangeTrafficLights();
                }
            }
        }

        void ChangeTrafficLights()
        {
            if (topIndicator != null && bottomIndicator != null && leftIndicator != null && rightIndicator != null)
            {
                //Debug.Log("changing");
                topIndicator.GetComponent<MeshRenderer>().material.color = (junctionState == JunctionState.TopBottom) ? Color.green : Color.red;
                bottomIndicator.GetComponent<MeshRenderer>().material.color = (junctionState == JunctionState.TopBottom) ? Color.green : Color.red;
                leftIndicator.GetComponent<MeshRenderer>().material.color = (junctionState == JunctionState.LeftRight) ? Color.green : Color.red;
                rightIndicator.GetComponent<MeshRenderer>().material.color = (junctionState == JunctionState.LeftRight) ? Color.green : Color.red;
                junctionState = (junctionState == JunctionState.TopBottom) ? JunctionState.LeftRight : JunctionState.TopBottom;
            }
        }
    }
}

