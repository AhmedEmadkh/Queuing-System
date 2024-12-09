using Microsoft.AspNetCore.Mvc;
using Queuing_System.Helpers;
using Queuing_System.Models;
using Queuing_System.Services;

namespace Queuing_System.Controllers
{
	public class QueueController : Controller
	{
        private readonly IQueueCalculator _calculator;
        private readonly PlotService _plotService;

        public QueueController(IQueueCalculator calculator)
        {
            _calculator = calculator;
            _plotService = new PlotService();
        }
        public IActionResult Index()
		{
            var model = new QueueModel();
			return View(model);
		}

		public IActionResult Calculate(QueueModel model)
		{
			if(ModelState.IsValid)
			{

                // Determine the type of the model
                model.QueueType = DetermineQueueType(model);

                try
                {
                    var result = _calculator.CalculateQueueParams(model);
                    return View(result);
                }
                catch (ApplicationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(nameof(Index),model);

        }
        public IActionResult Simulate(QueueModel model)
        {
            try
            {
                // Check if the model state is valid
                if (ModelState.IsValid)
                {

                    // Determine the queue type
                    model.QueueType = DetermineQueueType(model);

                    // Ensure all required properties have valid values
                    if (((int)model.QueueType) > 0)  // Add a condition to check if QueueType is valid
                    {
                        

                        // Redirect to the appropriate simulation action in SimulationController
                        return RedirectToAction(model.QueueType.ToString() + "Simulation", "Simulation", new
                        {
                            arrivalTime = model.ArrivalTime,
                            serviceTime = model.ServiceTime,
                            numberOfServers = model.NumberOfServers,
                            totalCapacity = model.TotalCapacity,
                            model.simPersons
                        });
                    }
                    else
                    {
                        // Return a view with an error message if QueueType is invalid
                        ModelState.AddModelError(string.Empty, "Invalid Queue Type.");
                        return View(nameof(Index), model);
                    }
                }
                else
                {
                    // If validation fails, return to the same view with validation errors
                    ModelState.AddModelError(string.Empty, "Invalid Queue Type.");
                    return View(nameof(Index), model);
                }
            }
            catch (ArgumentException ex)
            {
                // Handle specific argument exceptions
                ModelState.AddModelError(string.Empty, $"Validation Error: {ex.Message}");
                return View(nameof(Index), model);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                ModelState.AddModelError(string.Empty, $"An error occurred while calculating queue parameters: {ex.Message}");
                return View(nameof(Index), model);
            }
        }

        // Function to detirmince the Type of the Model
        private QueueType DetermineQueueType(QueueModel model)
        {
            if (model.NumberOfServers == 1 && !model.TotalCapacity.HasValue)
            {
                return QueueType.MM1; // M/M/1 Queue
            }
            else if (model.NumberOfServers > 1 && !model.TotalCapacity.HasValue)
            {
                return QueueType.MMc; // M/M/c Queue
            }
            else if (model.NumberOfServers == 1 && model.TotalCapacity > 1)
            {
                return QueueType.MM1k; // M/M/1/k Queue
            }
            else if (model.NumberOfServers > 1 && model.TotalCapacity > 1)
            {
                return QueueType.MMck; // M/M/c/K Queue
            }

            throw new InvalidOperationException("Invalid queue configuration");
        }
    }
}
