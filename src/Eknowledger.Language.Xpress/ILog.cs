using System.Collections.Generic;

namespace Eknowledger.Language.Xpress
{
    public interface ILog : ILogReader, ILogWriter
    {
        int Count { get; }
        int ErrorCount { get; }
        int DebugCount { get; }
        bool HasErrors { get; }
    }

    public interface ILogReader
    {
        IEnumerable<string> GetErrorMessages();
        IEnumerable<string> GetDebugMessages();

        string GetErrorStack();

        int ErrorCount { get; }
        int DebugCount { get; }
        bool HasErrors { get; }
    }

    public interface ILogWriter
    {
        void Error(string message);
        void Error(string format, params object[] args);
        void Debug(string message);
        void Debug(string format, params object[] args);
        void Clear();
    }
}
