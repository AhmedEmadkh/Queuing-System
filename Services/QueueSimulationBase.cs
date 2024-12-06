using Queuing_System.Models;
using System;

namespace Queuing_System.Services
{
    public abstract class QueueSimulationBase
    {
        // Properties
        protected List<Person> PersonsList = new();
        protected List<double> TimeEventList = new();
        protected List<int> QueueLengths = new();
        protected List<double> WaitingTimes = new();
        protected List<double> WaitingTimesInQueue = new();

        protected int NumberOfServers;
        protected double AvgArrivalTime;
        protected double AvgServiceTime;

        // Constructor
        protected QueueSimulationBase(int simPersons, int numServers, double avgArrivalTime, double avgServiceTime)
        {
            NumberOfServers = numServers;
            AvgArrivalTime = avgArrivalTime;
            AvgServiceTime = avgServiceTime;

            GeneratePersons(simPersons);
        }

        #region Abstract methods
        protected abstract void SimulateArrivalAndDeparture(); // To be implemented in derived classes
        #endregion

        #region Shared Methods
        private void GeneratePersons(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                PersonsList.Add(new Person
                {
                    ArrivalTime = Math.Round(random.NextDouble() * AvgArrivalTime, 5),
                    ServiceTime = Math.Round(random.NextDouble() * AvgServiceTime, 5)
                });
            }
            PersonsList = PersonsList.OrderBy(p => p.ArrivalTime).ToList();
        }

        protected void UpdateQueueLengths()
        {
            QueueLengths.Clear();
            int currentQueueLength = 0;

            foreach (var time in TimeEventList)
            {
                if (PersonsList.Any(p => p.ArrivalTime == time))
                {
                    currentQueueLength++;
                }
                if (PersonsList.Any(p => p.DepartureTime == time) && currentQueueLength > 0)
                {
                    currentQueueLength--;
                }
                QueueLengths.Add(currentQueueLength);
            }
        }

        public void RunSimulation()
        {
            SimulateArrivalAndDeparture();
            UpdateQueueLengths();
        }

        public SimulationResults GetResults()
        {
            return new SimulationResults
            {
                TimeEvents = TimeEventList,
                QueueLengths = QueueLengths,
                WaitingTimes = WaitingTimes,
                WaitingTimesInQueue = WaitingTimesInQueue,
                ArrivalTime = PersonsList.Select(p => p.ArrivalTime).ToList(),
                ServiceTime = PersonsList.Select(p => p.ServiceTime).ToList()
            };
        }
        #endregion
    }
}
