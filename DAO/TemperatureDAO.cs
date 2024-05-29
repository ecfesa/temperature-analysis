using System.Net.Http.Headers;
using System.Text.Json;

namespace temperature_analysis.DAO
{
    public class TemperatureDAO
    {
        private static readonly HttpClient client = new HttpClient();
        private const string _baseUri = "http://10.5.10.34";

        public async Task<string> GetStatus(string device)
        {
            string url = $"{_baseUri}:1026/v2/entities/{device}/attrs/state";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return responseBody;
        }

        private string GetLastMinute()
        {
            DateTime currentTime = DateTime.UtcNow.AddMinutes(-1);
            return currentTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        }

        public async Task<double> GetMinuteAverage(string entityName)
        {
            string timeFrom = GetLastMinute();
            string url = $"{_baseUri}:8666/STH/v1/contextEntities/type/Lamp/id/{entityName}/attributes/luminosity?dateFrom={timeFrom}&lastN=100";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonDocument.Parse(responseBody).RootElement;
                var luminosityValues = data
                    .GetProperty("contextResponses")[0]
                    .GetProperty("contextElement")
                    .GetProperty("attributes")[0]
                    .GetProperty("values");

                List<double> values = new List<double>();
                foreach (var value in luminosityValues.EnumerateArray())
                {
                    values.Add(value.GetProperty("attrValue").GetDouble());
                }

                return values.Count > 0 ? values.Average() : 0;
            }
            else
            {
                Console.WriteLine($"Error fetching data: {response.StatusCode}");
                return 0;
            }
        }

        static async Task SendCommand(bool turnOn, string entityName)
        {
            string url = $"{_baseUri}:1026/v2/entities/{entityName}/attrs";
            string command = turnOn ? "on" : "off";
            var payload = new
            {
                type = "command",
                value = ""
            };
            var commandPayload = new Dictionary<string, object> { { command, payload } };
            string json = JsonSerializer.Serialize(commandPayload);

            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            await client.PatchAsync(url, content);
        }

        public async Task CalculateState(double luminosity, string entityName)
        {
            if (luminosity > 50)
            {
                await SendCommand(false, entityName);
            }
            else
            {
                await SendCommand(true, entityName);
            }
        }

        public async Task<List<string>> GetDevices()
        {
            string url = $"{_baseUri}:4041/iot/devices";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
            var devices = new List<string>();
            var data = JsonDocument.Parse(responseBody).RootElement;

            foreach (var device in data.GetProperty("devices").EnumerateArray())
            {
                devices.Add(device.GetProperty("entity_name").GetString());
            }

            return devices;
        }

        public async Task<List<JsonElement>> GetPastData(int lastN)
        {
            string url = $"{_baseUri}:8666/STH/v1/contextEntities/type/Lamp/id/urn:ngsi-ld:Lamp:002/attributes/luminosity?lastN={lastN}";

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("fiware-service", "smart");
            client.DefaultRequestHeaders.Add("fiware-servicepath", "/");

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var data = JsonDocument.Parse(responseBody).RootElement;
                var luminosityData = data
                    .GetProperty("contextResponses")[0]
                    .GetProperty("contextElement")
                    .GetProperty("attributes")[0]
                    .GetProperty("values");

                var luminosityList = new List<JsonElement>();
                foreach (var value in luminosityData.EnumerateArray())
                {
                    luminosityList.Add(value);
                }

                return luminosityList;
            }
            else
            {
                Console.WriteLine($"Error fetching data: {response.StatusCode}");
                return new List<JsonElement>();
            }
        }
    }
}