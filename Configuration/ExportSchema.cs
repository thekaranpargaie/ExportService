using System;
using System.Collections.Generic;

namespace Exportify.Core.Configuration
{
    public class ExportSchema<T>
    {
        public List<ExportColumn<T>> Columns { get; } = new();

        public ExportSchema<T> AddColumn(string header, Func<T, object?> selector)
        {
            Columns.Add(new ExportColumn<T> { Header = header, ValueSelector = selector });
            return this;
        }
    }
}
