namespace Queuing_System.Services.ModelsSimulation
{
    public class MM1Simulation : QueueSimulationBase
    {
        public MM1Simulation(int simPersons, double avgArrivalTime, double avgServiceTime)
            : base(simPersons, 1, avgArrivalTime, avgServiceTime) { }

        protected override void Simulate()
        {
            // M/M/1 specific logic
            foreach (var person in PersonsList)
            {
                person.ArrivalTime *= (AvgArrivalTime * 2);
                person.ServiceTime *= (AvgServiceTime * 2);
            }
        }
    }
}
