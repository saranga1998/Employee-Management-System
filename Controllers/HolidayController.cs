using EMS_Project.Models;
using EMS_Project.Repository.Holiday;
using Microsoft.AspNetCore.Mvc;

namespace EMS_Project.Controllers
{
    public class HolidayController : Controller
    {
        private readonly ILogger<HolidayController> _logger;
        private readonly IHolidayRepository _IholidayRepository;

        //Injecting Services to controller
        public HolidayController(ILogger<HolidayController> logger, IHolidayRepository holidayRepository)
        {
            _logger = logger;
            _IholidayRepository = holidayRepository;
        }


        //Add Holiday Get Method
        [HttpGet]
        public IActionResult AddHoliday()
        {
            return View();
        }


        //Add Holiday Post Method
        [HttpPost]
        public async Task<IActionResult> AddHoliday(Holiday holiday)
        {

            try
            {
                await _IholidayRepository.AddHoliday(holiday);
                return RedirectToAction("AddHoliday");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Holiday");
            }
            return View(holiday);

        }


        //Get Method for All Holiday Details
        [HttpGet]
        public async Task<IActionResult> HolidayDetails()
        {
            var Holidays = await _IholidayRepository.GetAllHolidays();
            return View(Holidays);
        }


        //Delete Holiday by ID
        [HttpGet]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var result = await _IholidayRepository.DeleteHoliday(id);
            if (result == false)
            {
                return NotFound();
            }
            return RedirectToAction("HolidayDetails");
        }


        //Edit Holiday Get Method
        [HttpGet]
        public async Task<IActionResult> EditHoliday(int id)
        {
            var holiday = await _IholidayRepository.GetHolidayById(id);
            if (holiday == null) 
            {
                return NotFound();
            }
            return View(holiday);
        }


        //Edit Holiday Post Method
        [HttpPost]
        public async Task<IActionResult> EditHoliday(Holiday holiday)
        {
            try
            {
                await _IholidayRepository.UpdateHoliday(holiday);
                return RedirectToAction("HolidayDetails");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Holiday");
            }
            return View(holiday);
        }
    }
}
