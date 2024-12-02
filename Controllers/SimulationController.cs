using Microsoft.AspNetCore.Mvc;
using Queuing_System.Services.ModelsSimulation;
using Queuing_System.Services;

namespace Queuing_System.Controllers
{
    public class SimulationController : Controller
    {
        private readonly PlotService _plotService;

        public SimulationController(PlotService plotService)
        {
            _plotService = plotService;
        }

        public IActionResult MM1Simulation(double arrivalTime, double serviceTime, int numberOfServers, int? totalCapacity,int simPersons)
        {
            // Run the M/M/1 simulation
            var simulation = new MM1Simulation(simPersons, arrivalTime, serviceTime);
            simulation.RunSimulation();

            // Get simulation results
            var result = simulation.GetResults();

            // Generate a plot
            var plotModel = _plotService.CreateQueuePlot(result.TimeEvents, result.QueueLengths);
            byte[] imageBytes = _plotService.ExportPlotToPng(plotModel, 800, 400);
            string base64Image = Convert.ToBase64String(imageBytes);

            // Pass data to the view
            ViewBag.PlotImage = base64Image;
            //ViewBag.QueueLengths = result.QueueLengths;
            //ViewBag.TimePoints = result.TimeEvents;

            return View("SimulationResults",result);
        }

        [HttpGet]
        public IActionResult MMcSimulation(double arrivalTime, double serviceTime, int numberOfServers, int? totalCapacity)
        {
            // Run the M/M/c simulation logic here...
            return View("SimulationResults");
        }

        [HttpGet]
        public IActionResult MM1kSimulation(double arrivalTime, double serviceTime, int numberOfServers, int? totalCapacity)
        {
            // Run the M/M/1/k simulation logic here...
            return View("SimulationResults");
        }

        [HttpGet]
        public IActionResult MMckSimulation(double arrivalTime, double serviceTime, int numberOfServers, int? totalCapacity)
        {
            // Run the M/M/c/k simulation logic here...
            return View("SimulationResults");
        }
    }
}
