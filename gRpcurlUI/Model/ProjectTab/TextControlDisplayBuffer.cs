using System.Text;

namespace gRpcurlUI.Model.ProjectTab
{
    public class TextControlDisplayBuffer
    {
        private readonly StringBuilder buffer = new();

        public int MaxDisplayLength { get; set; } = 10000;

        public string ReadMoreChar { get; set; } = "...";

        public bool IsOverDisplay => buffer.Length > MaxDisplayLength;

        public string DisplayText
        {
            get
            {
                if (IsOverDisplay)
                {
                    var text = GetRowText()[..(MaxDisplayLength - ReadMoreChar.Length)];
                    return text + ReadMoreChar;
                }

                return GetRowText();
            }
        }

        public TextControlDisplayBuffer() { }

        public void SetText(string text)
        {
            Clear();
            AddText(text);
        }

        public void AddText(string text)
        {
            _ = buffer.Append(text);
        }

        public void Clear()
        {
            _ = buffer.Clear();
        }

        public string GetRowText()
        {
            return buffer.ToString();
        }
    }
}
