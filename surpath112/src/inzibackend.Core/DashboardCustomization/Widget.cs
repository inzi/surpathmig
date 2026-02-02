using System.Collections.Generic;

namespace inzibackend.DashboardCustomization
{
    public class Widget
    {
        public string WidgetId { get; set; }

        public byte Height { get; set; }

        public byte Width { get; set; }

        public byte PositionX { get; set; }

        public byte PositionY { get; set; }

        public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();
    }
}
