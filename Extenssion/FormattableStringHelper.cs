using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Extensions
{
    public static class FormattableStringHelper
    {
        public static string Builder(FormattableString urlForma)
        {
            var stringForma = urlForma.GetArguments().Select(p =>
            FormattableString.Invariant($"a"));
            object[] escapedParameters = stringForma.Select(s => (object)Uri.EscapeDataString(s)).ToArray();
            return string.Format(urlForma.Format, escapedParameters);
        }
    }
}