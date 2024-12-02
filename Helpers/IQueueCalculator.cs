using Queuing_System.Models;

namespace Queuing_System.Helpers
{
	public interface IQueueCalculator
	{
		public ResultModel CalculateQueueParams(QueueModel model);
		public ResultModel Calculate_M_M_1(QueueModel model, double lambda, double mu);
		public ResultModel Calculate_M_M_1_k(QueueModel model, double lambda, double mu);
		public ResultModel Calculate_M_M_c(QueueModel model, double lambda, double mu);
		public ResultModel Calculate_M_M_c_k(QueueModel model, double lambda, double mu);
	}
}
