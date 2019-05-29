using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Services
{
    public class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly HttpClient httpClient;

        public GoogleCalendarService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CalendarResponse> GetEvents(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var result = await httpClient.GetAsync("https://www.googleapis.com/calendar/v3/calendars/primary/events?timeMax=2019-08-10T10%3A00%3A00-07%3A00&timeMin=2019-05-01T10%3A00%3A00-07%3A00");
            var responseString = await result.Content.ReadAsStringAsync();
            var jobject = JObject.Parse(responseString);
            var response = jobject.ToObject<CalendarResponse>();
            return response;
        }
    }
}
