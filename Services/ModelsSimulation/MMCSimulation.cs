namespace Queuing_System.Services.ModelsSimulation
{
    public class MMCSimulation : QueueSimulationBase
    {
        public MMCSimulation(int simPersons, int numServers, double avgArrivalTime, double avgServiceTime)
            : base(simPersons, numServers, avgArrivalTime, avgServiceTime)
        {
        }

        protected override void SimulateArrivalAndDeparture()
        {
            var serverAvailability = Enumerable.Repeat(0.0, NumberOfServers).ToList();

            foreach (var person in PersonsList)
            {
                // Check if the arrival time is greater than or equal to the earliest server availability
                if (person.ArrivalTime >= serverAvailability.Min())
                {
                    person.StartTime = person.ArrivalTime;
                }
                else
                {
                    // Assign the earliest available server
                    person.StartTime = serverAvailability.Min();
                }

                // Calculate the departure time
                person.DepartureTime = person.StartTime + person.ServiceTime;

                // Update server availability
                int serverIndex = serverAvailability.IndexOf(serverAvailability.Min());
                serverAvailability[serverIndex] = person.DepartureTime;

                // Add to event lists
                TimeEventList.Add(person.ArrivalTime);
                TimeEventList.Add(person.DepartureTime);
            }

            TimeEventList = TimeEventList.OrderBy(t => t).ToList();

            // Remove blocked persons (those exceeding capacity)

            // Calculate waiting times
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
