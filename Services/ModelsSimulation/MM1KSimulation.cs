﻿namespace Queuing_System.Services.ModelsSimulation
{
    public class MM1KSimulation : QueueSimulationBase
    {
        public MM1KSimulation(int simPersons, int numServers, double avgArrivalTime, double avgServiceTime) 
            : base(simPersons, numServers, avgArrivalTime, avgServiceTime)
        {
        }

        protected override void SimulateArrivalAndDeparture()
        {
            throw new NotImplementedException();
        }
    }
}
