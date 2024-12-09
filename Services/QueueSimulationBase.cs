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
        private int? capcity;

        public int? Capacity
        {
            get { return capcity ; }
            set { capcity = value.HasValue ? value.Value : 0; }
        }

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

        protected void UpdateQueueLengthsWithCapacity()
        {
            QueueLengths.Clear();
            int currentQueueLength = 0;

            // A list to track the blocked persons based on arrival time and queue state
            var blockedPersons = new List<Person>();

            foreach (var time in TimeEventList.OrderBy(t => t))
            {
                // Check for arriving persons at this time
                var arrivingPersons = PersonsList.Where(p => p.ArrivalTime == time).ToList();

                foreach (var person in arrivingPersons)
                {
                    // If capacity is defined and the queue length exceeds capacity, block the person
                    if (Capacity.HasValue && currentQueueLength < Capacity.Value)
                    {
                        // Person enters the queue
                        currentQueueLength++;
                    }
                    else if(currentQueueLength > 0)
                    {
                        // Person is blocked, so we don't increment currentQueueLength
                        person.IsBlocked = true;
                        blockedPersons.Add(person); // Track blocked persons
                    }
                }

                // Check for departing persons at this time and update the queue length
                var departingPersons = PersonsList.Where(p => p.DepartureTime == time).ToList();
                foreach (var person in departingPersons)
                {
                    if (currentQueueLength > 0)
                    {
                        currentQueueLength--;
                    }
                }

                // Add the current queue length to the QueueLengths list
                if (currentQueueLength != 0)
                {
                    QueueLengths.Add(currentQueueLength);
                }
                else if (currentQueueLength == 0)
                {
                    TimeEventList.Remove(TimeEventList.FirstOrDefault(t => t == time));
                }
            }
        }
        protected void UpdateQueueLengths()
        {
            QueueLengths.Clear();
            int currentQueueLength = 0;

            // A list to track the blocked persons based on arrival time and queue state
            var blockedPersons = new List<Person>();

            foreach (var time in TimeEventList.OrderBy(t => t))
            {
                // Check for arriving persons at this time
                var arrivingTimes = PersonsList.Where(p => p.ArrivalTime == time).Select(p => p.ArrivalTime).ToList();

                // Check for departing persons at this time and update the queue length
                var departingTimes = PersonsList.Where(p => p.DepartureTime == time).Select(p => p.DepartureTime).ToList();
                if (arrivingTimes.Contains(time))
                    currentQueueLength++;

                if (departingTimes.Contains(time))
                    currentQueueLength--;



                // Add the current queue length to the QueueLengths list
                QueueLengths.Add(currentQueueLength);
            }
        }



        public void RunSimulation()
        {
            SimulateArrivalAndDeparture();
            if(Capacity.HasValue)
            {
                UpdateQueueLengthsWithCapacity();
            }
            else
            {
                UpdateQueueLengths();
            }
        }

        public SimulationResults GetResults()
        {
            var isBlockedList = new List<bool>();

            // Only calculate blocked status if Capacity is defined
            if (Capacity.HasValue)
            {
                for (int i = 0; i < PersonsList.Count; i++)
                {
                    bool isBlocked = i < QueueLengths.Count && QueueLengths[i] > Capacity.Value;
                    isBlockedList.Add(isBlocked);
                }
            }

            return new SimulationResults
            {
                TimeEvents = TimeEventList,
                QueueLengths = QueueLengths,
                WaitingTimes = WaitingTimes,
                WaitingTimesInQueue = WaitingTimesInQueue,
                Persons = PersonsList,
                IsBlocked = isBlockedList,
                Capacity = Capacity // Include Capacity if applicable
            };
        }

        ///private void RemoveBlockedPersons()
        ///{
        ///    // Filter out blocked persons based on the `IsBlocked` property
        ///    var blockedPersons = PersonsList.Where(p => p.IsBlocked).ToList();
        ///    foreach (var blockedPerson in blockedPersons)
        ///    {
        ///        // Remove both ArrivalTime and DepartureTime from the TimeEventList
        ///        TimeEventList.Remove(blockedPerson.ArrivalTime);
        ///        TimeEventList.Remove(blockedPerson.DepartureTime);
        ///    }
        ///    // Sort the TimeEventList after removing events
        ///    TimeEventList = TimeEventList.OrderBy(t => t).ToList();
        ///}
        #endregion
    }
}
