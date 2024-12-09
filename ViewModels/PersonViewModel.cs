namespace Queuing_System.ViewModels
{
    public class PersonViewModel
    {
        public double ArrivalTime { get; set; }
        public double ServiceTime { get; set; }
        public double? StartTime { get; set; }
        public double? DepartureTime { get; set; }
        public bool Blocked { get; set; }
    }

}
