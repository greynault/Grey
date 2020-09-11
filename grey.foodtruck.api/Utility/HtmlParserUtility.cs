using System;
using System.Drawing;

namespace grey.foodtruck.utilities
{
    public class HtmlParserUtility
    {
        private string html;
        public HtmlParserUtility(string html)
        {
            this.html = html;
        }

        public string GetValueForKey(string key)
        {
            string[] sections = html.Split(new string[] { "<tbody>" }, StringSplitOptions.RemoveEmptyEntries);
            int keyIndex = GetIndexOfKeyFromHeader(sections[0], key);
            if (keyIndex < 0)
            {
                return string.Empty;
            }

            return GetValueFromBodySection(sections[1], keyIndex);
        }

        private string GetValueFromBodySection(string bodySection, int index)
        {
            string[] bodyItems = bodySection.Split(new string[] { "<td" }, StringSplitOptions.RemoveEmptyEntries);
            string bodyContent = bodyItems[index].Trim();
            bodyContent = bodyContent.Substring(bodyContent.IndexOf(">") + 1);
            bodyContent = bodyContent.Substring(0, bodyContent.IndexOf("</td>"));
            return bodyContent.Trim();
        }

        private int GetIndexOfKeyFromHeader(string headerSection, string key)
        {
            string endMarker = "</th>";

            string[] headers = headerSection.Split(new string[] { "<th>" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < headers.Length; i++)
            {
                if (!headers[i].Contains(endMarker))
                {
                    continue;
                }

                string singleHeader = headers[i].Substring(0, headers[i].IndexOf(endMarker));
                if (string.Equals(key, singleHeader.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }

            return -1;
        }

        public PointF GetPointForKey(string key)
        {
            string pointAsString = GetValueForKey(key);
            if (string.IsNullOrWhiteSpace(pointAsString))
            {
                return new PointF();
            }

            string x = pointAsString.Substring(pointAsString.IndexOf("("));
            x = x.Substring(1, x.IndexOf(" "));

            string y = pointAsString.Substring(pointAsString.IndexOf(x) + x.Length);
            y = y.Substring(0, y.IndexOf(")"));

            if (!float.TryParse(x, out float xFinal))
            {
                return new PointF();
            }

            if (!float.TryParse(y, out float yFinal))
            {
                return new PointF();
            }

            return new PointF(xFinal, yFinal);
        }
    }
}
