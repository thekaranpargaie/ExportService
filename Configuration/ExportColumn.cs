using System;

namespace Exportify.Core.Configuration
{
    public class ExportColumn<T>
    {
        public string Header { get; set; } = string.Empty;
        public Func<T, object?> ValueSelector { get; set; } = _ => null!;
    }
}
