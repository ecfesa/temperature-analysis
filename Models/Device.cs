namespace temperature_analysis.Models
{
    public class Device
    {
        public string device_id { get; set; }
        public string service { get; set; }
        public string service_path { get; set; }
        public string entity_name { get; set; }
        public string entity_type { get; set; }
        public string transport { get; set; }
        public string protocol { get; set; }
    }
}
