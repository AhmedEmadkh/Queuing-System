using System;
using System.Collections.Generic;
using System.Linq;
using Queuing_System.Models;

namespace Queuing_System.Services.ModelsSimulation
{
    public class MM1KSimulation : QueueSimulationBase
    {

        public MM1KSimulation(int simPersons, double avgArrivalTime, double avgServiceTime, int capacity)
            : base(simPersons, numServers: 1, avgArrivalTime, avgServiceTime)
        {
            Capacity = capacity;
        }

        protected override void SimulateArrivalAndDeparture()
        {
            var serverAvailability = Enumerable.Repeat(0.0, NumberOfServers).ToList(); // Tracks availability for each server
            int currentSystemCount = 0; // Total persons in the system (servers + queue)

            foreach (var person in PersonsList)
            {
                // Check if the person can enter the system (servers + queue)
                if (currentSystemCount >= Capacity)
                {
                    person.IsBlocked = true; // Mark the person as blocked
                    continue; // Skip this person
                }

                // Mark the person as not blocked
                person.IsBlocked = false;

                // Determine the start time (either when they arrive or when a server is available)
                if (person.ArrivalTime >= serverAvailability.Min())
                {
                    person.StartTime = person.ArrivalTime;
                }
                else
                {
                    person.StartTime = serverAvailability.Min(); // Assign to the earliest available server
                }

                // Calculate departure time
                person.DepartureTime = person.StartTime + person.ServiceTime;

                // Update server availability
                int serverIndex = serverAvailability.IndexOf(serverAvailability.Min());
                serverAvailability[serverIndex] = person.DepartureTime;

                // Update event list
                TimeEventList.Add(person.ArrivalTime);
                TimeEventList.Add(person.DepartureTime);

                // Increment system count as the person enters the system
                currentSystemCount++;

                // Decrement system count when the person departs
                TimeEventList.Add(person.DepartureTime);
                currentSystemCount--;
            }

            // Sort the time events
            TimeEventList = TimeEventList.OrderBy(t => t).ToList();

            // Calculate waiting times for all non-blocked persons
            CalculateWaitingTimes();
        }



        private void CalculateWaitingTimes()
        {
            foreach (var person in PersonsList)
            {
                var waitingTime = person.DepartureTime - person.ArrivalTime;
                WaitingTimes.Add(waitingTime);

                var waitingTimeInQueue = person.StartTime - person.ArrivalTime;
                WaitingTimesInQueue.Add(waitingTimeInQueue);
            }
        }
    }
}
