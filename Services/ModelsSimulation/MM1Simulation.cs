using Queuing_System.Models;

namespace Queuing_System.Services.ModelsSimulation
{
    public class MM1Simulation : QueueSimulationBase
    {
        public MM1Simulation(int simPersons, double avgArrivalTime, double avgServiceTime)
            : base(simPersons, 1, avgArrivalTime, avgServiceTime) 
        {
        }


        protected override void SimulateArrivalAndDeparture()
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
    }
}
