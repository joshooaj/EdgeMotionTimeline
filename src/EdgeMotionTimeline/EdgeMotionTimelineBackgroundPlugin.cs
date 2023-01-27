using System;
using System.Collections.Generic;
using EdgeMotionTimeline.Properties;
using VideoOS.Platform;
using VideoOS.Platform.Background;
using VideoOS.Platform.Client;

namespace EdgeMotionTimeline
{
  public class EdgeMotionTimelineBackgroundPlugin : BackgroundPlugin
  {
    public override Guid Id { get; } = new Guid(Resources.CustomTimelineBackgroundPluginId);
    public override string Name => nameof(EdgeMotionTimelineBackgroundPlugin);

    public override List<EnvironmentType> TargetEnvironments { get; } = new List<EnvironmentType>
    {
      EnvironmentType.SmartClient
    };

    public override void Init()
    {
      MipLogger.WriteInfo("Subscribing to NewImageViewerControlEvent.");
      ClientControl.Instance.NewImageViewerControlEvent += NewImageViewerControlEvent;
    }

    public override void Close()
    {
      ClientControl.Instance.NewImageViewerControlEvent -= NewImageViewerControlEvent;
    }

    private readonly Dictionary<ImageViewerAddOn, EdgeMotionTimelineSequenceSource> _sequenceSources =
      new Dictionary<ImageViewerAddOn, EdgeMotionTimelineSequenceSource>();
    private void NewImageViewerControlEvent(ImageViewerAddOn imageViewerAddOn)
    {
      MipLogger.WriteInfo($"New ImageViewerAddOn loaded. ImageViewerType = {imageViewerAddOn.ImageViewerType}");
      if (imageViewerAddOn.ImageViewerType != ImageViewerType.CameraViewItem)
      {
        return;
      }
      
      RegisterSequenceSource(imageViewerAddOn);
      imageViewerAddOn.PropertyChangedEvent += ImageViewerPropertyChanged;
      imageViewerAddOn.CloseEvent += ImageViewerClosed;
    }

    private void RegisterSequenceSource(ImageViewerAddOn imageViewerAddOn)
    {
      // CameraFQID might be null if the view item is empty or the camera cannot be found in the configuration.
      // The user may add a camera to the view item later, at which point this method will be called again.
      EdgeMotionTimelineSequenceSource sequenceSource = null;
      if (imageViewerAddOn.CameraFQID != null)
      {
        MipLogger.WriteInfo($"Registering timeline sequence source for camera with ID {imageViewerAddOn.CameraFQID.ObjectId}.");
        sequenceSource = new EdgeMotionTimelineSequenceSource(imageViewerAddOn.CameraFQID);
        imageViewerAddOn.RegisterTimelineSequenceSource(sequenceSource);
      }
      else
      {
        MipLogger.WriteInfo($"No camera found in ImageViewerAddOn instance yet.");
      }
      _sequenceSources[imageViewerAddOn] = sequenceSource;
    }

    private void UnregisterSequenceSource(ImageViewerAddOn imageViewerAddOn)
    {
      // The imageViewerAddOn should be in the dictionary, but the entry may be null if CameraFQID
      // was null when the ImageViewerAddOn was created.
      if (!_sequenceSources.ContainsKey(imageViewerAddOn) || _sequenceSources[imageViewerAddOn] == null)
      {
        return;
      }
      MipLogger.WriteInfo("Unregistering timeline sequence source");
      imageViewerAddOn.UnregisterTimelineSequenceSource(_sequenceSources[imageViewerAddOn]);
      _sequenceSources[imageViewerAddOn].Dispose();
      _sequenceSources[imageViewerAddOn] = null;
    }

    private void ImageViewerPropertyChanged(object sender, EventArgs e)
    {
      if (!(sender is ImageViewerAddOn imageViewerAddOn))
      {
        return;
      }

      if (!_sequenceSources.ContainsKey(imageViewerAddOn))
      {
        MipLogger.WriteError("Could not find imageViewerAddOn instance in _sequenceSources dictionary.");
        return;
      }

      if (imageViewerAddOn.CameraFQID == _sequenceSources[imageViewerAddOn].CameraFqid)
      {
        return;
      }

      UnregisterSequenceSource(imageViewerAddOn);

      if (imageViewerAddOn.CameraFQID != null)
      {
        RegisterSequenceSource(imageViewerAddOn);
      }
    }

    private void ImageViewerClosed(object sender, EventArgs e)
    {
      if (!(sender is ImageViewerAddOn imageViewerAddOn))
      {
        return;
      }
      MipLogger.WriteInfo("An ImageViewerAddOn instance is closing");
      UnregisterSequenceSource(imageViewerAddOn);
      RemoveImageViewerAddOn(imageViewerAddOn);
    }

    private void RemoveImageViewerAddOn(ImageViewerAddOn imageViewerAddOn)
    {
      MipLogger.WriteInfo("Unsubscribing to ImageViewerAddOn events and removing reference");
      imageViewerAddOn.PropertyChangedEvent -= ImageViewerPropertyChanged;
      imageViewerAddOn.CloseEvent -= ImageViewerClosed;
      _sequenceSources.Remove(imageViewerAddOn);
    }
  }
}