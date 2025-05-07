using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Teams
{
    public class LogoUrl
    {
        public string Value { get; private set; }

        public LogoUrl(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Logo URL cannot be empty");
            Value = value;
        }

        public static implicit operator string(LogoUrl logo) => logo.Value;
        public static implicit operator LogoUrl(string value) => new LogoUrl(value);
    }
}
