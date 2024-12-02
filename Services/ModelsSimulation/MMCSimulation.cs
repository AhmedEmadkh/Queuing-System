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
            throw new NotImplementedException();
        }
    }
}
