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
            // Initialize server end times
            List<double> serverEndTimes = new List<double>(new double[NumberOfServers]);
            Queue<int> waitingQueue = new Queue<int>();

            foreach (var person in PersonsList)
            {
                double arrivalTime = person.ArrivalTime;

                // Check server availability
                int availableServer = serverEndTimes.FindIndex(endTime => endTime <= arrivalTime);
                if (availableServer != -1)
                {
                    // Serve the person immediately
                    person.StartTime = arrivalTime;
                    person.DepartureTime = arrivalTime + person.ServiceTime;
                    person.Served = true;

                    // Update server end time
                    serverEndTimes[availableServer] = person.DepartureTime;

                    // No waiting time
                    WaitingTimes.Add(0);
                    WaitingTimesInQueue.Add(0);
                }
                else
                {
                    // Add to waiting queue
                    waitingQueue.Enqueue(PersonsList.IndexOf(person));
                }

                // Serve waiting queue when a server becomes available
                while (waitingQueue.Count > 0)
                {
                    int waitingCustomerIndex = waitingQueue.Peek();
                    int freeServer = serverEndTimes.FindIndex(endTime => endTime <= arrivalTime);

                    if (freeServer == -1) break; // No free server, exit loop

                    var waitingCustomer = PersonsList[waitingCustomerIndex];

                    // Serve the next customer in the queue
                    waitingCustomer.StartTime = serverEndTimes[freeServer];
                    waitingCustomer.DepartureTime = waitingCustomer.StartTime + waitingCustomer.ServiceTime;
                    waitingCustomer.Served = true;

                    // Update server end time
                    serverEndTimes[freeServer] = waitingCustomer.DepartureTime;

                    // Record waiting time
                    double waitingTime = waitingCustomer.StartTime - waitingCustomer.ArrivalTime;
                    double waitingTime_Q = (waitingCustomer.StartTime - waitingCustomer.ArrivalTime) + waitingCustomer.ServiceTime;
                    WaitingTimes.Add(waitingTime);
                    WaitingTimesInQueue.Add(waitingTime_Q);

                    // Remove from queue
                    waitingQueue.Dequeue();
                }
                TimeEventList.Add(person.ArrivalTime);
                TimeEventList.Add(person.DepartureTime);
            }
            TimeEventList = TimeEventList.OrderBy(t => t).ToList();
        }
    }
}
