
using EMS_Project.Data;
using Microsoft.EntityFrameworkCore;

namespace EMS_Project.Repository.Holiday
{
    public class HolidayRepository : IHolidayRepository
    {
        private readonly AppDbContext _appDbContext;

        //injecting AppDbContext to Constructor
        public HolidayRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Add Holiday Logic
        public async Task AddHoliday(Models.Holiday date)
        {
            var HolidayId = new Models.Holiday
            {
                DayId = int.Parse(date.Holiday1.Value.ToString("yyyyMMdd")),
                Holiday1 = date.Holiday1,
                Title = date.Title,
            };
            await _appDbContext.Holidays.AddAsync(HolidayId);
            await _appDbContext.SaveChangesAsync();
        }


        //Get All Holidays Logic
        public async Task<List<Models.Holiday>> GetAllHolidays()
        {
            var Days = await _appDbContext.Holidays.ToListAsync();
            List<Models.Holiday> holidays = new List<Models.Holiday>();
            foreach (var day in Days)
            {
                var holiday = new Models.Holiday
                {
                    DayId = day.DayId,
                    Holiday1  = day.Holiday1,
                    Title = day.Title,

                };
                holidays.Add(holiday);
            }

            return holidays;
        }
        

        //Delete Hoiday by ID
        public async Task<bool> DeleteHoliday(int id)
        {
            var day = await _appDbContext.Holidays.FindAsync(id);
            if (day == null)
            {
                return false;
            }
            else
            {
                _appDbContext.Holidays.Remove(day);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
        }

        
        //Get Holiday By ID
        public async Task<Models.Holiday> GetHolidayById(int id)
        {
            var day = await _appDbContext.Holidays.FindAsync(id);
            var result = new Models.Holiday
            {
                DayId = day.DayId,
                Holiday1 = day.Holiday1,
                Title = day.Title,
            };
            return result;
        }


        //Update Hoilday 
        public async Task<bool> UpdateHoliday(Models.Holiday holiday)
        {
            var EditHoliday = await _appDbContext.Holidays.FindAsync(holiday.DayId);

            if(EditHoliday == null)
            {
                return false;
            }
            else
            {
                EditHoliday.DayId = holiday.DayId;
                EditHoliday.Title = holiday.Title;
                EditHoliday.Holiday1 = holiday.Holiday1;
                await _appDbContext.SaveChangesAsync();
                return true;
            }
        }
    }
}
