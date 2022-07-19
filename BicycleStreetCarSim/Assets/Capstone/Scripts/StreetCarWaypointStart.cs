using System;
using System.Collections.Generic;
using System.Linq;
using Capstone.Scripts.BehaviourTree;
using UnityEngine;

namespace Capstone.Scripts
{
    public class StreetCarWaypointStart : StreetCarWaypoint
    {
        public List<int> scheduleList = default;
        private Dictionary<int, bool> scheduleListToSecondsFromMidnight = default;

        private void Update()
        {
            if (scheduleListToSecondsFromMidnight != null)
            {
                foreach (var timing in scheduleListToSecondsFromMidnight.Keys.ToList())
                {
                    if (GameController.Instance.Timer() >= timing && !scheduleListToSecondsFromMidnight[timing])
                    {
                        if (GameController.Instance.StreetCarPrefab != null)
                        {
                            GameObject streetCar = Instantiate(GameController.Instance.StreetCarPrefab);
                            streetCar.transform.position = this.transform.position;
                            List<Transform> nextWaypoints = new List<Transform>();
                            StreetCarWaypoint waypointRef = this.nextWaypoint;
                            while (waypointRef != null)
                            {
                                nextWaypoints.Add(waypointRef.transform);
                                waypointRef = waypointRef.nextWaypoint;
                            }

                            streetCar.GetComponent<StreetCarBehaviourTree>().nextWaypoints = nextWaypoints.ToArray();
                        }

                        scheduleListToSecondsFromMidnight[timing] = true;
                    }
                }
            }
            
        }

        private int scheduleToSecondsFromMidnight(int schedule)
        {
            int hours = schedule / 100;
            int minutes = (schedule < 100) ? schedule : (hours * 100);
            return hours * 3600 + minutes * 60;
        }

        public void FormatSchedules()
        {
            Dictionary<int,bool> schedules = new Dictionary<int, bool>();
            foreach (int schedule in scheduleList)
            {
                schedules.Add(scheduleToSecondsFromMidnight(schedule), false);
            }

            scheduleListToSecondsFromMidnight = schedules;
        }
    }
}
