using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eknowledger.Language.Xpress
{
    public class InMemoryLogger : ILog
    {
        private IList<KeyValuePair<string, string>> _messageBuffer;
        public static readonly string ErrorKey = "ERROR";
        public static readonly string MsgKey = "MSG";

        public InMemoryLogger()
        {
            _messageBuffer = new List<KeyValuePair<string, string>>();
        }

        public void Error(string message)
        {
            _messageBuffer.Add(new KeyValuePair<string, string>(ErrorKey, message));
        }

        public void Error(string format, params object[] args)
        {
            Error(string.Format(format, args));
        }

        public void Debug(string message)
        {
            _messageBuffer.Add(new KeyValuePair<string, string>(MsgKey, message));
        }

        public void Debug(string format, params object[] args)
        {
            Debug(string.Format(format, args));
        }

        public void Clear()
        {
            _messageBuffer.Clear();
        }

        public int Count { get { return _messageBuffer.Count; } }
        public int ErrorCount { get { return _messageBuffer.Where(b => b.Key.Equals(ErrorKey)).Count(); } }
        public int DebugCount { get { return _messageBuffer.Where(b => b.Key.Equals(MsgKey)).Count(); } }
        public bool HasErrors { get { return _messageBuffer.Where(b => b.Key.Equals(ErrorKey)).Any(); } }

        public IList<KeyValuePair<string, string>> Buffer
        {
            get { return _messageBuffer; }
        }

        public IEnumerable<string> GetErrorMessages()
        {
            return _messageBuffer.Where(m => m.Key.Equals(ErrorKey)).Select(m => m.Value).ToList();
        }

        public string GetErrorStack()
        {
            var sb = new StringBuilder();
            var errorMessages = GetErrorMessages();
            foreach (var msg in errorMessages)
                sb.AppendLine(msg);
            return sb.ToString();
        }

        public IEnumerable<string> GetDebugMessages()
        {
            return _messageBuffer.Where(m => m.Key.Equals(MsgKey)).Select(m => m.Value).ToList();
        }
    }
}
