namespace HttpDemo.Models
{
    public class Connection
    {
        public string Id { get; set; }
        public long Time { get; set; }
        public ConnectionType ConnectionType { get; set; }
        public int CallsForConnection { get; set; }
    }
}
