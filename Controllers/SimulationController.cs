using Microsoft.AspNetCore.Mvc;
using Queuing_System.Services.ModelsSimulation;
using Queuing_System.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static SkiaSharp.HarfBuzz.SKShaper;
using Queuing_System.Models;

namespace Queuing_System.Controllers
{
    public class SimulationController : Controller
    {
        private readonly PlotService _plotService;

        public SimulationController(PlotService plotService)
        {
            _plotService = plotService;

        }

        public IActionResult MM1Simulation(double arrivalTime, double serviceTime, int numberOfServers,int simPersons, double L, double Lq, double W, double Wq)
        {
            // Run the M/M/1 simulation
            var simulation = new MM1Simulation(simPersons, arrivalTime, serviceTime);
            simulation.RunSimulation();

            // Get simulation results
            var result = simulation.GetResults();
            SetCalculationResults(L, Lq, W, Wq, result);

            Plot(result);

            return View("SimulationResults",result);
        }

        [HttpGet]
        public IActionResult MMcSimulation(double arrivalTime, double serviceTime, int numberOfServers, int simPersons, double L, double Lq, double W, double Wq)
        {
            try
            {
                if (arrivalTime <= 0 || serviceTime <= 0 || numberOfServers <= 0 || simPersons <= 0)
                {
                    ModelState.AddModelError(string.Empty, "All input values must be positive.");
                }

                // Run the M/M/C simulation
                var simulation = new MMCSimulation(simPersons, numberOfServers, arrivalTime, serviceTime);
                simulation.RunSimulation();

                // Get simulation results
                var result = simulation.GetResults();
                SetCalculationResults(L, Lq, W, Wq, result);

                Plot(result);
                return View("SimulationResults", result);
            }
            catch (Exception ex)
            {
                // Log error if necessary
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("SimulationResults");
            }
        }

        [HttpGet]
        public IActionResult MM1kSimulation(double arrivalTime, double serviceTime, int totalCapacity,int simPersons, double L, double Lq, double W, double Wq)
        {
            try
            {
                if (arrivalTime <= 0 || serviceTime <= 0 || simPersons <= 0)
                {
                    ModelState.AddModelError(string.Empty, "All input values must be positive.");
                }

                // Run the M/M/1/K simulation
                var simulation = new MM1KSimulation(simPersons, arrivalTime, serviceTime, totalCapacity);
                simulation.RunSimulation();

                // Get simulation results
                var result = simulation.GetResults();
                SetCalculationResults(L, Lq, W, Wq, result);

                Plot(result);
                return View("SimulationResults", result);
            }
            catch (Exception ex)
            {
                // Log error if necessary
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("SimulationResults");
            }
        }

        [HttpGet]
        public IActionResult MMckSimulation(double arrivalTime, double serviceTime, int numberOfServers, int totalCapacity, int simPersons, double L, double Lq, double W, double Wq)
        {
            try
            {
                if (arrivalTime <= 0 || serviceTime <= 0 || numberOfServers <= 0 || simPersons <= 0)
                {
                    ModelState.AddModelError(string.Empty, "All input values must be positive.");
                }

                // Run the M/M/C/K simulation
                var simulation = new MMCKSimulation(simPersons, numberOfServers, arrivalTime, serviceTime, totalCapacity);
                simulation.RunSimulation();

                // Get simulation results
                var result = simulation.GetResults();
                SetCalculationResults(L, Lq, W, Wq, result);

                Plot(result);
                return View("SimulationResults", result);
            }
            catch (Exception ex)
            {
                // Log error if necessary
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("SimulationResults");
            }
        }
        #region Private Helper Methods
        private void Plot(SimulationResults result)
        {
            // Generate a plot
            var plotModel = _plotService.CreateQueuePlot(result.TimeEvents, result.QueueLengths);
            byte[] imageBytes = _plotService.ExportPlotToPng(plotModel, 1200, 400);
            string base64Image = Convert.ToBase64String(imageBytes);

            ViewBag.PlotImage = base64Image;
        }
        private void SetCalculationResults(double L, double Lq, double W, double Wq, SimulationResults result)
        {
            result.L = L;
            result.Lq = Lq;
            result.W = W;
            result.Wq = Wq;
        } 
        #endregion
    }
}
