namespace Queuing_System.Models
{
    public class Person
    {
        public double ArrivalTime { get; set; }
        public double ServiceTime { get; set; }
        public string State { get; set; } = "e";
        public double StartTime { get; set; }
        public double DepartureTime { get; set; }
        public bool Served { get; set; }
        public bool IsBlocked { get; set; }

        public void SetState(string state)
        {
            State = state;
        }
    }
}
