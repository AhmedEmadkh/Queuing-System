using Queuing_System.Models;
using System;

namespace Queuing_System.Services
{
    public abstract class QueueSimulationBase
    {
        // Shared properties
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

        #region Abstract methods (to be implemented in derived classes)
         // Core simulation logic for specific models
        protected abstract void SimulateArrivalAndDeparture(); // Simulate Arrival and Departure According to the Model 
        #endregion

        #region Shared Methods
        private void GeneratePersons(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                PersonsList.Add(new Person
                {
                    ArrivalTime = Math.Round((double)random.NextDouble(), 5, MidpointRounding.AwayFromZero),
                    ServiceTime = Math.Round((double)random.NextDouble(), 5, MidpointRounding.AwayFromZero)
                });
            }
            PersonsList = PersonsList.OrderBy(p => p.ArrivalTime).ToList();
        }

        protected void UpdateQueueLengths()
        {
            QueueLengths.Clear();
            foreach (var time in TimeEventList)
            {
                if (PersonsList.Any(p => p.ArrivalTime == time))
                {
                    QueueLengths.Add(QueueLengths.LastOrDefault() + 1);
                }
                else if (PersonsList.Any(p => p.DepartureTime == time))
                {
                    QueueLengths.Add(QueueLengths.LastOrDefault() - 1);
                }
            }
        }
        protected void Simulate()
        {
            foreach (var person in PersonsList)
            {
                person.ArrivalTime *= (AvgArrivalTime * 2);
                person.ServiceTime *= (AvgServiceTime * 2);
            }
        }
        // Public Method to Run the Simulation
        public void RunSimulation()
        {
            Simulate(); // Calls the derived class implementation
            SimulateArrivalAndDeparture();
            UpdateQueueLengths();
        }

        // Results Getter
        public SimulationResults GetResults()
        {
            var ArrivalTime = PersonsList.Select(P => P.ArrivalTime).ToList();
            var ServiceTime = PersonsList.Select(P => P.ServiceTime).ToList();
            return new SimulationResults
            {
                TimeEvents = TimeEventList,
                QueueLengths = QueueLengths,
                WaitingTimes = WaitingTimes,
                WaitingTimesInQueue = WaitingTimesInQueue,
                ArrivalTime = ArrivalTime,
                ServiceTime = ServiceTime

            };
        } 
        #endregion
    }
}
