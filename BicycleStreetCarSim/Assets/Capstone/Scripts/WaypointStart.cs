using System;
using System.Collections.Generic;
using System.Linq;
using Capstone.Scripts.BehaviourTree;
using UnityEngine;

namespace Capstone.Scripts
{
    public class WaypointStart : Waypoint
    {
        public List<int> scheduleList = default;
        private Dictionary<int, bool> scheduleListToSecondsFromMidnight = default;
        public GameObject pedestrianPrefab = default;
        public Type type = default;
        public enum Type
        {
            StreetCar,
            Bicycle
        }
        private void Update()
        {
            if (scheduleListToSecondsFromMidnight == null)
            {
                FormatSchedules();
            }
            if (scheduleListToSecondsFromMidnight != null)
            {
                foreach (var timing in scheduleListToSecondsFromMidnight.Keys.ToList())
                {
                    if (GameController.Instance.Timer() >= timing && !scheduleListToSecondsFromMidnight[timing])
                    {
                        if (type == Type.StreetCar && GameController.Instance.StreetCarPrefab != null)
                        {
                            GameObject streetCar = Instantiate(GameController.Instance.StreetCarPrefab);
                            streetCar.transform.position = this.transform.position;
                            List<Transform> nextWaypoints = new List<Transform>();
                            Waypoint waypointRef = this.nextWaypoint;
                            while (waypointRef != null)
                            {
                                nextWaypoints.Add(waypointRef.transform);
                                waypointRef = waypointRef.nextWaypoint;
                            }

                            streetCar.GetComponent<StreetCarBehaviourTree>().routeName = routeName;
                            streetCar.GetComponent<StreetCarBehaviourTree>().startTime = timing;
                            streetCar.GetComponent<StreetCarBehaviourTree>().nextWaypoints = nextWaypoints.ToArray();
                            streetCar.GetComponent<StreetCarBehaviourTree>().pedestrianPrefab = pedestrianPrefab;
                        }
                        else
                        {
                            GameObject bicycle = Instantiate(GameController.Instance.BicyclePrefab);
                            bicycle.transform.position = this.transform.position;
                            List<Transform> nextWaypoints = new List<Transform>();
                            Waypoint waypointRef = this.nextWaypoint;
                            while (waypointRef != null)
                            {
                                nextWaypoints.Add(waypointRef.transform);
                                waypointRef = waypointRef.nextWaypoint;
                            }

                            bicycle.GetComponent<BicycleBehaviourTree>().nextWaypoints = nextWaypoints.ToArray();
                            
                        }

                        scheduleListToSecondsFromMidnight[timing] = true;
                    }
                }
            }
            
        }

        private int scheduleToSecondsFromMidnight(int schedule)
        {
            int hours = schedule / 100;
            int minutes = (schedule < 100) ? schedule : (schedule - hours * 100);
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
