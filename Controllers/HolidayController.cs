using EMS_Project.Models;
using EMS_Project.Repository.Holiday;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace EMS_Project.Controllers
{
    public class HolidayController : Controller
    {
        private readonly ILogger<HolidayController> _logger;
        private readonly IHolidayRepository _IholidayRepository;
        private readonly IMemoryCache _memoryCache;
        private readonly string key = "HolidayKey";

        //Injecting Services to controller
        public HolidayController(ILogger<HolidayController> logger, IHolidayRepository holidayRepository,IMemoryCache memoryCache)
        {
            _logger = logger;
            _IholidayRepository = holidayRepository;
            _memoryCache = memoryCache;
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
            //Data fetching message from cache or DB
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if(_memoryCache.TryGetValue(key, out List<Holiday> Holidays))
            {
                _logger.Log(LogLevel.Information, "Data fetched from cache");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Data not fetched from cache");
                Holidays = await _IholidayRepository.GetAllHolidays();
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
                    SlidingExpiration = TimeSpan.FromSeconds(45),
                    Priority = CacheItemPriority.Normal
                };
                _memoryCache.Set(key, Holidays, cacheEntryOptions);
            }
            stopwatch.Stop();
            _logger.Log(LogLevel.Information, "Time taken to fetch data: " + stopwatch.ElapsedMilliseconds);
            
            return View(Holidays);
        }

        //Cache Clear Method
        public IActionResult ClearCache()
        {
            _memoryCache.Remove(key);
            _logger.Log(LogLevel.Information, "Cleared cache");
            return RedirectToAction("HolidayDetails");

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
