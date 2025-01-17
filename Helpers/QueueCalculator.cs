﻿using Queuing_System.Models;

namespace Queuing_System.Helpers
{
	public class QueueCalculator : IQueueCalculator
	{
        public ResultModel CalculateQueueParams(QueueModel model)
        {
            try
            {
                double lambda = model.ArrivalTime;
                double mu = model.ServiceTime;

                return model.QueueType switch
                {
                    QueueType.MM1 => Calculate_M_M_1(model, lambda, mu),
                    QueueType.MM1k => Calculate_M_M_1_k(model, lambda, mu),
                    QueueType.MMc => Calculate_M_M_c(model, lambda, mu),
                    QueueType.MMck => Calculate_M_M_c_k(model, lambda, mu),
                    _ => throw new InvalidOperationException("Invalid Queue Type")
                };
            }
            catch (ArgumentException ex)
            {
                throw new ApplicationException($"Validation Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An Error Occured While Calculating Queue Parameters: {ex.Message}", ex);
            }
        }

        #region MM1
        public ResultModel Calculate_M_M_1(QueueModel model, double lambda, double mu)
        {
            if (lambda <= 0 || mu <= 0)
            {
                throw new ArgumentException("Lambda (arrival rate) and Mu (service rate) must be greater than zero.");
            }

            // Calculate traffic intensity (ρ)
            double r = lambda / mu;
            if (r >= 1)
            {
                throw new InvalidOperationException("The system is unstable as arrival rate is greater than or equal to service rate.");
            }

            // Calculate Lq (average number of customers in the queue)
            model.Lq = Math.Pow(r, 2) / (1 - r);

            // Calculate L (average number of customers in the system)
            model.L = model.Lq + r;

            // Calculate W (average time a customer spends in the system)
            model.W = model.L / lambda;

            // Calculate Wq (average time a customer spends in the queue)
            model.Wq = model.Lq / lambda;

            // Return results
            return new ResultModel
            {
                L = model.L,
                Lq = model.Lq,
                W = model.W,
                Wq = model.Wq
            };
        }
        #endregion
        #region MM1K
        public ResultModel Calculate_M_M_1_k(QueueModel model, double lambda, double mu)
        {
            throw new NotImplementedException();
        } 
        #endregion
        #region MMC
        public ResultModel Calculate_M_M_c(QueueModel model, double lambda, double mu)
        {
            if (model.NumberOfServers <= 0)
            {
                throw new ArgumentException("Number of servers must be greater than zero.");
            }
            if (lambda <= 0 || mu <= 0)
            {
                throw new ArgumentException("Lambda (arrival rate) and Mu (service rate) must be greater than zero.");
            }

            // Calculate traffic intensity (ρ)
            double r = lambda / mu;

            // Calculate P0 (probability that the system is empty)
            double P0 = 0;
            if ((r / model.NumberOfServers) < 1)
            {
                // Case: ρ / c < 1
                double sumPart = 0;
                for (int i = 0; i <= model.NumberOfServers - 1; i++)
                {
                    sumPart += Math.Pow(r, i) / Factorial(i);
                }

                double lastPart = ((Math.Pow(r, model.NumberOfServers) * model.NumberOfServers) /
                                   (Factorial(model.NumberOfServers) * (model.NumberOfServers - r)));

                P0 = 1.0 / (sumPart + lastPart);
            }
            else
            {
                // Case: ρ / c >= 1
                double sumPart = 0;
                for (int i = 0; i <= model.NumberOfServers - 1; i++)
                {
                    sumPart += Math.Pow(r, i) / Factorial(i);
                }

                double lastPart = (Math.Pow(r, model.NumberOfServers) /
                                   (Factorial(model.NumberOfServers) * ((model.NumberOfServers * mu) / ((model.NumberOfServers * mu) - lambda))));

                P0 = 1.0 / (sumPart + lastPart);
            }

            // Calculate Lq (average number of customers in the queue)
            double numerator = Math.Pow(r, model.NumberOfServers) * lambda * mu;
            double denominator = Factorial(model.NumberOfServers - 1) *
                                 Math.Pow((model.NumberOfServers * mu) - lambda, 2);
            model.Lq = (numerator / denominator) * P0;

            // Calculate L (average number of customers in the system)
            model.L = model.Lq + r;

            // Calculate W (average time a customer spends in the system)
            model.W = (model.Lq / lambda) + (1 / mu);

            // Calculate Wq (average time a customer spends in the queue)
            model.Wq = model.Lq / lambda;

            // Return results
            return new ResultModel
            {
                L = model.L,
                Lq = model.Lq,
                W = model.W,
                Wq = model.Wq
            };
        }
        #endregion
        #region MMCK
        public ResultModel Calculate_M_M_c_k(QueueModel model, double lambda, double mu)
        {
            throw new NotImplementedException();
        } 
        #endregion
        #region Helper Methods
        // Helper method for factorial calculation
        private static double Factorial(int n)
        {
            if (n == 0 || n == 1) return 1;
            double result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }
        #endregion
    }
}
