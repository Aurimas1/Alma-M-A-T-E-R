using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace API.Services
{
    public class Gaa
    {
        private readonly HttpClient httpClient;

        private const string GetAll = "https://content.googleapis.com/calendar/v3/calendars/primary/events?timeMax={endTime}&timeMin={startTime}"; //"&key=AIzaSyAa8yy0GdcGPHdtD083HiGGx_S0vMPScDM"; // time pattern 2019-04-10T10:00:00-07:00

        public Gaa(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CalendarResponse> GetEvents(string accessToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var result = await httpClient.GetAsync("https://www.googleapis.com/calendar/v3/calendars/primary/events?timeMax=2019-04-10T10%3A00%3A00-07%3A00&timeMin=2019-04-04T10%3A00%3A00-07%3A00");
            var jobject = JObject.Parse(await result.Content.ReadAsStringAsync());
            var response = jobject.ToObject<CalendarResponse>();
            return response;
        }
    }
}
