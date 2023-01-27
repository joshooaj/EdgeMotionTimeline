using System.Runtime.CompilerServices;
using VideoOS.Platform;

namespace EdgeMotionTimeline
{
  internal static class MipLogger
  {
    public static void WriteInfo(string message, [CallerMemberName] string caller = "")
    {
      EnvironmentManager.Instance.Log(false, caller, message);
    }

    public static void WriteError(
      string message,
      [CallerMemberName] string caller = "",
      [CallerLineNumber] int line = -1,
      [CallerFilePath] string path = "")
    {
      EnvironmentManager.Instance.Log(true, $"{caller}:{line} ({path})", message);
    }
  }
}
