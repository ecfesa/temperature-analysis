using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using temperature_analysis.Models;
using temperature_analysis.Utils;

namespace temperature_analysis.DAO
{
    public class DeviceDAO
    {
        private readonly HttpClient _httpClient;

        public DeviceDAO()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<DeviceViewModel>> GetAll()
        {
            try
            {
                string url = $"{Constants.FiwareUrl}:4041/iot/devices";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("fiware-service", "smart");
                request.Headers.Add("fiware-servicepath", "/");

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var devices = JsonSerializer.Deserialize<DeviceRequest>(jsonString);

                List<DeviceViewModel> deviceViewModels = new List<DeviceViewModel>();

                foreach (var device in devices.devices)
                {
                    DeviceViewModel deviceViewModel = new DeviceViewModel
                    {
                        DeviceId = device.device_id,
                        Service = device.service,
                        ServicePath = device.service_path,
                        EntityName = device.entity_name,
                        EntityType = device.entity_type,
                        Transport = device.transport,
                        Protocol = device.protocol
                    };
                    deviceViewModels.Add(deviceViewModel);
                }

                return deviceViewModels;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public async Task<bool> Insert(DeviceViewModel device)
        {
            try
            {
                var wrapper = new DeviceRequest()
                {
                    devices = new List<Device>()
                    {
                        new Device
                        {
                            device_id = device.DeviceId,
                            service = device.Service,
                            service_path = device.ServicePath,
                            entity_name = device.EntityName,
                            entity_type = device.EntityType,
                            transport = device.Transport,
                            protocol = device.Protocol,
                        }
                    }
                };

                var serializedDevice = JsonSerializer.Serialize(wrapper);
                var content = new StringContent(serializedDevice, Encoding.UTF8, "application/json");
                var url = $"{Constants.FiwareUrl}:4041/iot/devices";

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("fiware-service", "smart");
                request.Headers.Add("fiware-servicepath", "/");
                request.Content = content;

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Insertion failed with status code {response.StatusCode} ({errorMessage})");
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return false;
            }
        }

        public async Task<bool> Delete(string deviceId)
        {
            try
            {
                var url = $"{Constants.FiwareUrl}:4041/iot/devices/{deviceId}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Add("fiware-service", "smart");
                request.Headers.Add("fiware-servicepath", "/");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return true;
                else
                {
                    Console.WriteLine($"Deletion failed with status code {response.StatusCode}");
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return false;
            }
        }

        public async Task<DeviceViewModel> Get(string deviceId)
        {
            try
            {
                var url = $"{Constants.FiwareUrl}:4041/iot/devices/{deviceId}";

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("fiware-service", "smart");
                request.Headers.Add("fiware-servicepath", "/");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var device = JsonSerializer.Deserialize<Device>(jsonString);

                    DeviceViewModel deviceViewModel = new DeviceViewModel
                    {
                        DeviceId = device.device_id,
                        Service = device.service,
                        ServicePath = device.service_path,
                        EntityName = device.entity_name,
                        EntityType = device.entity_type,
                        Transport = device.transport,
                        Protocol = device.protocol
                    };

                    return deviceViewModel;
                }
                else
                {
                    Console.WriteLine($"Failed to get device. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

    }
}
