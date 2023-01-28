using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using EnhancedTimeline.Admin;
using EnhancedTimeline.Background;
using EnhancedTimeline.Properties;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Background;

namespace EnhancedTimeline
{
  public class EnhancedTimelinePluginDefinition : PluginDefinition
  {
    public override Guid Id => PluginIds.PluginId;
    public override Image Icon => Resources.Server;
    public override string Name => Resources.PluginName;
    public override string Manufacturer => Resources.Manufacturer;
    public override string VersionString => Resources.PluginVersion;
    
    public override List<BackgroundPlugin> BackgroundPlugins { get; } = new List<BackgroundPlugin>();
    public override List<ItemNode> ItemNodes { get; } = new List<ItemNode>();

    public override void Init()
    {
      ItemNodes.Add(
        new ItemNode (
          PluginKinds.EnhancedTimelineKind,
          Guid.Empty, 
          Resources.SingularName, Resources.DummyItem,
          Resources.PluralName, Resources.DummyItem,
          Category.VideoIn, true,
          ItemsAllowed.Many,
          new TimelineSequenceDefinitionItemManager(),
          null
        ));

      if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.SmartClient)
      {
        BackgroundPlugins.Add(new EnhancedTimelineBackgroundPlugin());
      }

      if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Administration)
      {
      }
    }

    public override void Close()
    {
      BackgroundPlugins.Clear();
      ItemNodes.Clear();
    }
  }
}