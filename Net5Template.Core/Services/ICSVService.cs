using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Services
{
    public interface ICSVService
    {
        string WriteToString<T>(IEnumerable<T> table);
        string WriteToString<T>(IEnumerable<T> table, bool header, bool quoteall);
        string WriteToString<T>(IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0);
        StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, string comaSeparator, string textSeparator);
        StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table);
        StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, bool header, bool quoteall);
        StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0);
        StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0, string comaSeparator, string textSeparator);
        string RemoveInvalidChars(string s);
        string TruncateComments(string s);
        byte[] GetCSVBytes<T>(IEnumerable<T> list);
    }
}
