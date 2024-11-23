namespace EMS_Project.Repository.Holiday
{
    public interface IHolidayRepository
    {
        //Add Holiday Logic
        Task AddHoliday(Models.Holiday holiday);


        //Get All Holidays Logic
        Task<List<Models.Holiday>> GetAllHolidays();


        //Get Holiday by ID
        Task<Models.Holiday> GetHolidayById(int id);


        //Delete Holiday Logic
        Task<bool> DeleteHoliday(int id);


        //Update Holiday Logic
        Task<bool> UpdateHoliday(Models.Holiday holiday);
    }
}
