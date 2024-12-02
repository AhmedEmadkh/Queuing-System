namespace Queuing_System.Models
{
	public class QueueModel
	{
		#region Given Parameters
		public double ArrivalTime { get; set; }    // 1 / λ (Time between customer arrivals)
		public double ServiceTime { get; set; }    // 1 / μ (Time to serve each customer)
        public int NumberOfServers { get; set; }   // C (Number of Servers)
        public int? TotalCapacity { get; set; }    // K (Total number of customer that system can handle) 
        public QueueType QueueType { get; set; }
        public int simPersons { get; set; }
        #endregion
        #region Calculated Properties
        public double L { get; set; }
        public double Lq { get; set; }
        public double W { get; set; }
        public double Wq { get; set; }

        public QueueModel() // Constructor to initialize
        {
            NumberOfServers = 1;
            L = 0;
            Lq = 0;
            W = 0;
            Wq = 0;
        }
        #endregion
    }
}
