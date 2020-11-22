using Net5Template.Core.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Infrastructure.Formatters
{
    public class CSVService : ICSVService
    {
        public string WriteToString<T>(IEnumerable<T> table)
        {
            return WriteToStringBuilder(table, true, true).ToString();
        }
        public string WriteToString<T>(IEnumerable<T> table, bool header, bool quoteall)
        {
            return WriteToStringBuilder<T>(table, header, quoteall, false).ToString();
        }
        public string WriteToString<T>(IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0)
        {
            return WriteToStringBuilder(table, header, quoteall, ignoreIfRowCount0).ToString();
        }
        public StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, string comaSeparator, string textSeparator)
        {
            return WriteToStringBuilder(table, true, true, true, comaSeparator, textSeparator);
        }
        public StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table)
        {
            return WriteToStringBuilder(table, true, true);
        }
        public StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, bool header, bool quoteall)
        {
            return WriteToStringBuilder<T>(table, header, quoteall, false);
        }
        public StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0)
        {
            return WriteToStringBuilder(table, header, quoteall, ignoreIfRowCount0, ";", "\"");
        }
        public StringBuilder WriteToStringBuilder<T>(IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0, string comaSeparator, string textSeparator)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("sep=" + comaSeparator + System.Environment.NewLine);
            WriteToStream(sb, table, header, quoteall, ignoreIfRowCount0, comaSeparator, textSeparator);
            return sb;
        }
        private void WriteToStream<T>(StringBuilder sb, IEnumerable<T> table, bool header, bool quoteall, bool ignoreIfRowCount0, string comaSeparator, string textSeparator)
        {
            if (ignoreIfRowCount0)
            {
                if (table.Count() == 0)
                    return;
            }

            var element = table.FirstOrDefault();
            var columns = TypeDescriptor.GetProperties(element).Cast<PropertyDescriptor>().Select(a => a.Name);
            if (header)
            {
                for (int i = 0; i < columns.Count(); i++)
                {
                    WriteItem(sb, columns.ElementAt(i), quoteall, textSeparator);
                    if (i < columns.Count() - 1)
                        sb.Append(comaSeparator);
                    else
                        sb.Append('\n');
                }
            }
            foreach (var row in table)
            {
                for (int i = 0; i < columns.Count(); i++)
                {
                    var c = columns.ElementAt(i);
                    var o = row.GetType().GetProperty(c).GetValue(row, null);
                    if (o is DateTime
                        && ((DateTime)o) == DateTime.MinValue)
                        o = DBNull.Value;
                    WriteItem(sb, o, quoteall, textSeparator);
                    if (i < columns.Count() - 1)
                        sb.Append(comaSeparator);
                    else
                        sb.Append('\n');
                }
            }
        }
        private void WriteItem(StringBuilder sb, object item, bool quoteall, string textSeparator)
        {
            if (item == null)
                return;
            string s = RemoveInvalidChars(item.ToString());
            if (item is System.Collections.IList
                || (item is DateTime && ((DateTime)item).Equals(DateTime.MinValue)))
                s = string.Empty;
            if (quoteall || s.IndexOfAny((textSeparator + ",\x0A\x0D").ToCharArray()) > -1)
                sb.Append(textSeparator + s.Replace(textSeparator, textSeparator + textSeparator).Replace("\r", string.Empty).Replace("\t", " ") + textSeparator);
            else
                sb.Append(s);
        }
        public string RemoveInvalidChars(string s)
        {
            char[] c = null;
            if (!string.IsNullOrEmpty(s) && s.Length > 0)
                c = s.ToCharArray().Where(
                    a => char.GetUnicodeCategory(a) != UnicodeCategory.OtherSymbol
                        && char.GetUnicodeCategory(a) != UnicodeCategory.PrivateUse).ToArray();

            if (c != null && c.Length > 0)
                return new string(c);
            else
                return string.Empty;
        }
        public string TruncateComments(string s)
        {
            if (!string.IsNullOrEmpty(s) && s.Length > 3900)
                return s.Remove(3900);
            else if (!string.IsNullOrEmpty(s))
                return s;
            else
                return string.Empty;
        }

        public byte[] GetCSVBytes<T>(IEnumerable<T> list)
        {
            return ASCIIEncoding.ASCII.GetBytes(WriteToString(list));
        }
    }
}