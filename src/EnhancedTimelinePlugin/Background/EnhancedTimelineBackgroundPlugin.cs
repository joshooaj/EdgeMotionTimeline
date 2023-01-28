using System;
using System.Collections.Generic;
using EnhancedTimeline.Admin;
using EnhancedTimeline.Properties;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;
using VideoOS.Platform.Messaging;

namespace EnhancedTimeline.Background
{
  public class EnhancedTimelineBackgroundPlugin : BackgroundPlugin
  {
    private readonly Dictionary<ImageViewerAddOn, List<EnhancedTimelineSequenceSource>> _sequenceSources =
      new Dictionary<ImageViewerAddOn, List<EnhancedTimelineSequenceSource>>();

    private List<Item> _definitions;
    private object _registration;

    public override Guid Id { get; } = new Guid(Resources.EnhancedTimelineBackgroundPluginId);
    public override string Name => nameof(EnhancedTimelineBackgroundPlugin);

    public override List<EnvironmentType> TargetEnvironments { get; } = new List<EnvironmentType>
    {
      EnvironmentType.SmartClient
    };

    public override void Init()
    {
      MipLog.Info("Subscribing to NewImageViewerControlEvent.");
      ClientControl.Instance.NewImageViewerControlEvent += NewImageViewerControlEvent;
      _definitions =
        Configuration.Instance.GetItemConfigurations(PluginIds.PluginId, null, PluginKinds.EnhancedTimelineKind);
      _registration = EnvironmentManager.Instance.RegisterReceiver(ConfigurationUpdated,
        new MessageIdFilter(MessageId.Server.ConfigurationChangedIndication));
    }

    private object ConfigurationUpdated(Message message, FQID destination, FQID sender)
    {
      if (message.RelatedFQID.Kind != PluginKinds.EnhancedTimelineKind)
      {
        return null;
      }

      Configuration.Instance.RefreshConfiguration(message.RelatedFQID.ServerId, PluginKinds.EnhancedTimelineKind);

      return null;
    }

    public override void Close()
    {
      ClientControl.Instance.NewImageViewerControlEvent -= NewImageViewerControlEvent;
      _definitions.Clear();
      EnvironmentManager.Instance.UnRegisterReceiver(_registration);
    }

    private void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
    {
      MipLog.Info($"New ImageViewerAddOn loaded. ImageViewerType = {imageViewerAddOn.ImageViewerType}");
      if (imageViewerAddOn.ImageViewerType != ImageViewerType.CameraViewItem)
      {
        return;
      }

      RegisterSequenceSources(imageViewerAddOn);
      imageViewerAddOn.PropertyChangedEvent += ImageViewerPropertyChanged;
      imageViewerAddOn.CloseEvent += ImageViewerClosed;
    }

    private void RegisterSequenceSources(ImageViewerAddOn imageViewerAddOn)
    {
      UnregisterSequenceSources(imageViewerAddOn);
      if ((_definitions?.Count ?? 0) == 0 || imageViewerAddOn.CameraFQID == null)
      {
        return;
      }

      if (!_sequenceSources.ContainsKey(imageViewerAddOn))
      {
        _sequenceSources[imageViewerAddOn] = new List<EnhancedTimelineSequenceSource>();
      }
      
      MipLog.Info($"Registering timeline sequence sources for camera with ID {imageViewerAddOn.CameraFQID.ObjectId}.");
      foreach (var item in _definitions)
      {
        if (!item.Properties.ContainsKey(SequenceDefinitionProperties.StartEvent) ||
            !item.Properties.ContainsKey(SequenceDefinitionProperties.StopEvent) ||
            !item.Properties.ContainsKey(SequenceDefinitionProperties.RibbonColor))
        {
          MipLog.Info(
            $"Ignoring timeline definition \"{item.Name}\" because one or more configuration values are invalid.");
        }

        var sequenceSource = new EnhancedTimelineSequenceSource(imageViewerAddOn.CameraFQID, new SequenceDefinition(item));
        _sequenceSources[imageViewerAddOn].Add(sequenceSource);
        imageViewerAddOn.RegisterTimelineSequenceSource(sequenceSource);
      }
    }

    private void UnregisterSequenceSources(ImageViewerAddOn imageViewerAddOn)
    {
      // The imageViewerAddOn should be in the dictionary, but the entry may be null if CameraFQID
      // was null when the ImageViewerAddOn was created.
      if (!_sequenceSources.ContainsKey(imageViewerAddOn) || _sequenceSources[imageViewerAddOn] == null)
      {
        return;
      }

      MipLog.Info("Unregistering timeline sequence sources");
      foreach (var sequenceSource in _sequenceSources[imageViewerAddOn])
      {
        imageViewerAddOn.UnregisterTimelineSequenceSource(sequenceSource);
        sequenceSource.Dispose();
      }

      _sequenceSources[imageViewerAddOn].Clear();
    }

    private void ImageViewerPropertyChanged(object sender, EventArgs e)
    {
      if (!(sender is ImageViewerAddOn imageViewerAddOn))
      {
        return;
      }

      if (!_sequenceSources.ContainsKey(imageViewerAddOn))
      {
        MipLog.Error("Could not find imageViewerAddOn instance in _sequenceSources dictionary.");
        return;
      }

      if (imageViewerAddOn.CameraFQID == _sequenceSources[imageViewerAddOn][0].CameraFqid)
      {
        return;
      }

      RegisterSequenceSources(imageViewerAddOn);
    }

    private void ImageViewerClosed(object sender, EventArgs e)
    {
      if (!(sender is ImageViewerAddOn imageViewerAddOn))
      {
        return;
      }

      MipLog.Info("An ImageViewerAddOn instance is closing");
      UnregisterSequenceSources(imageViewerAddOn);
      RemoveImageViewerAddOn(imageViewerAddOn);
    }

    private void RemoveImageViewerAddOn(ImageViewerAddOn imageViewerAddOn)
    {
      MipLog.Info("Unsubscribing to ImageViewerAddOn events and removing reference");
      imageViewerAddOn.PropertyChangedEvent -= ImageViewerPropertyChanged;
      imageViewerAddOn.CloseEvent -= ImageViewerClosed;
      _sequenceSources.Remove(imageViewerAddOn);
    }
  }
}