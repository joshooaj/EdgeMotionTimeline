using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using EdgeMotionTimeline.Properties;
using VideoOS.Platform;
using VideoOS.Platform.Background;

namespace EdgeMotionTimeline
{
  public class EdgeMotionTimelinePluginDefinition : PluginDefinition
  {
    private static readonly Image TopTreeNodeImage;
    private static readonly Guid TimelinePluginId = new Guid(Resources.PluginId);

    public override Guid Id => TimelinePluginId;

    public override string Name => Resources.PluginName;

    public override string Manufacturer => Resources.Manufacturer;

    public override string VersionString => Assembly.GetAssembly(typeof(EdgeMotionTimelinePluginDefinition)).GetName().Version.ToString();

    public override Image Icon => TopTreeNodeImage;

    public override List<BackgroundPlugin> BackgroundPlugins { get; } = new List<BackgroundPlugin>();

    static EdgeMotionTimelinePluginDefinition()
    {
      TopTreeNodeImage = Resources.Server;
    }

    public override void Init()
    {
      if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
      {
        BackgroundPlugins.Add(new EdgeMotionTimelineBackgroundPlugin());
      }
    }

    public override void Close()
    {
      BackgroundPlugins.Clear();
    }
  }
}
