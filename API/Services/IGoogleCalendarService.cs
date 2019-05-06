using System.Threading.Tasks;

namespace API.Services
{
    public interface IGoogleCalendarService
    {
        Task<CalendarResponse> GetEvents(string accessToken);
    }
}
