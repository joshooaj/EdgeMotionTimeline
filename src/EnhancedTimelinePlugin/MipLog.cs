using System.Runtime.CompilerServices;
using VideoOS.Platform;

namespace EnhancedTimeline
{
  internal static class MipLog
  {
    public static void Info(string message, [CallerMemberName] string caller = "")
    {
      EnvironmentManager.Instance.Log(false, caller, message);
    }

    public static void Error(
      string message,
      [CallerMemberName] string caller = "",
      [CallerLineNumber] int line = -1,
      [CallerFilePath] string path = "")
    {
      EnvironmentManager.Instance.Log(true, $"{caller}:{line} ({path})", message);
    }
  }
}
