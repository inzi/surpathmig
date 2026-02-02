using System.Collections.Generic;

namespace inzibackend.DashboardCustomization.Dto
{
    public class WidgetOutput
    {
        public string Id { get; }

        public string Name { get; }

        public string Description { get; }

        public List<WidgetFilterOutput> Filters { get; set; }

        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();

        public WidgetOutput(string id, string name, string description, List<WidgetFilterOutput> filters = null, Dictionary<string, string> settings = null)
        {
            Id = id;
            Name = name;
            Description = description;
            Filters = filters;

        }
    }
}