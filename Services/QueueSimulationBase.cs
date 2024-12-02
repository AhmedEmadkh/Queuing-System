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
        protected abstract void Simulate(); // Core simulation logic for specific models
        //protected abstract void SimulateArrivalAndDeparture(Person person); // Simulate Arrival and Departure According to the Model 
        #endregion

        #region Shared Methods
        private void GeneratePersons(int count)
        {
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                PersonsList.Add(new Person
                {
                    ArrivalTime = Math.Round(random.NextDouble(), 2),
                    ServiceTime = Math.Round(random.NextDouble(), 2)
                });
            }
            PersonsList = PersonsList.OrderBy(p => p.ArrivalTime).ToList();
        }

        protected void ClockSimulation()
        {
            double currentTime = 0;

            foreach (var person in PersonsList)
            {
                if (currentTime <= person.ArrivalTime)
                {
                    person.StartTime = person.ArrivalTime;
                    WaitingTimes.Add(person.ServiceTime);
                    WaitingTimesInQueue.Add(0);
                }
                else
                {
                    person.StartTime = currentTime;
                    WaitingTimes.Add((person.StartTime - person.ArrivalTime) + person.ServiceTime);
                    WaitingTimesInQueue.Add(person.StartTime - person.ArrivalTime);
                }

                person.DepartureTime = person.StartTime + person.ServiceTime;
                TimeEventList.Add(person.ArrivalTime);
                TimeEventList.Add(person.DepartureTime);

                currentTime = person.DepartureTime;
            }

            TimeEventList = TimeEventList.OrderBy(t => t).ToList();
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

        // Public Method to Run the Simulation
        public void RunSimulation()
        {
            Simulate(); // Calls the derived class implementation
            ClockSimulation();
            UpdateQueueLengths();
        }

        // Results Getter
        public SimulationResults GetResults()
        {
            return new SimulationResults
            {
                TimeEvents = TimeEventList,
                QueueLengths = QueueLengths,
                WaitingTimes = WaitingTimesInQueue,
                WaitingTimesInQueue = WaitingTimesInQueue,
            };
        } 
        #endregion
    }
}
