namespace Queuing_System.Models
{
    public class SimulationResults
    {
        public List<double> TimeEvents { get; set; } = new List<double>();
        public List<int> QueueLengths { get; set; } = new List<int>();
        public List<double> WaitingTimes { get; set; } = new List<double>();
        public List<double> WaitingTimesInQueue { get; set; } = new List<double>();
        public List<Person> Persons { get; set; } = new List<Person>();

        public List<double> ArrivalTime { get; set; }
        public List<double> ServiceTime { get; set; }
        public List<bool> IsBlocked { get; set; } = new List<bool>();
        public int? Capacity { get; set; }
    }
}
