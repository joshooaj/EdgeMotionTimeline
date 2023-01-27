using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using EdgeMotionTimeline.Properties;
using VideoOS.Platform;
using VideoOS.Platform.Client;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Proxy.AlarmClient;
using VideoOS.Platform.Util;

namespace EdgeMotionTimeline
{
  public class EdgeMotionTimelineSequenceSource : TimelineSequenceSource, IDisposable
  {
    private IAlarmClient _alarmClient;
    private readonly object _lock = new object();
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<Task> _tasks = new List<Task>();
    public FQID CameraFqid { get; }
    public override Guid Id { get; }
    public override string Title => Resources.BackgroundTimelineSourceTitle;
    public override TimelineSequenceSourceType SourceType => TimelineSequenceSourceType.Ribbon;
    public override Color RibbonContentColor => Color.DarkMagenta;

    public EdgeMotionTimelineSequenceSource(FQID cameraFqid)
    {
      var id = Guid.NewGuid();
      Id = id;
      MipLogger.WriteInfo($"Initializing new sequence source {id} for camera {cameraFqid.ObjectId}");
      CameraFqid = cameraFqid;
      _cancellationTokenSource = new CancellationTokenSource();
    }
    public override void StartGetSequences(IEnumerable<TimeInterval> intervals)
    {
      MipLogger.WriteInfo($"Request for sequences received for camera with ID {CameraFqid.ObjectId}.");
      foreach (var interval in intervals)
      {
        var task = Task.Run(() =>
        {
          var stopwatch = Stopwatch.StartNew();
          MipLogger.WriteInfo($"Retrieving sequences from {interval.StartTime.ToLocalTime()} to {interval.EndTime.ToLocalTime()} for camera with ID {CameraFqid.ObjectId}.");
          try
          {
            lock (_lock)
            {
              if (_alarmClient == null)
              {
                MipLogger.WriteInfo($"Retrieving a new AlarmClient instance.");
                _alarmClient = new AlarmClientManager().GetAlarmClient(CameraFqid.ServerId);
                MipLogger.WriteInfo($"AlarmClient instance retrieved.");
              }
            }

            var result = new TimelineSourceQueryResult(interval);
            var startFilter = BuildFilter("Motion Started Driver", interval);
            var stopFilter = BuildFilter("Motion Stopped Driver", interval);
            var startEventsTask = Task.Run(() => _alarmClient.GetEventLines(0, int.MaxValue, startFilter),
              _cancellationTokenSource.Token);
            var stopEventsTask = Task.Run(() => _alarmClient.GetEventLines(0, int.MaxValue, stopFilter),
              _cancellationTokenSource.Token);
            Task.WaitAll(new Task[] {startEventsTask, stopEventsTask}, _cancellationTokenSource.Token);

            var startEvents = startEventsTask.Result;
            var stopEvents = stopEventsTask.Result;
            var sequences = new List<TimelineDataArea>();
            result.Sequences = sequences;
            var stopIndex = -1;
            foreach (var startEvent in startEvents)
            {
              var startTime = startEvent.Timestamp;
              var endTime = DateTime.UtcNow;
              if (sequences.Count > 0 && startTime < sequences[sequences.Count - 1].Interval.EndTime)
              {
                MipLogger.WriteInfo($"Ignoring start event because timestamp is earlier than the last stop event.");
                continue;
              }

              for (++stopIndex; stopIndex < stopEvents.Length; stopIndex++)
              {
                if (stopEvents[stopIndex].Timestamp < startEvent.Timestamp)
                {
                  if (stopIndex == 0)
                  {
                    sequences.Add(new TimelineDataArea(new TimeInterval(interval.StartTime, stopEvents[0].Timestamp)));
                  }
                  else
                  {
                    MipLogger.WriteInfo($"Ignoring stop event because timestamp is earlier than the start event.");
                  }
                  continue;
                }

                endTime = stopEvents[stopIndex].Timestamp;
                break;
              }

              sequences.Add(new TimelineDataArea(new TimeInterval(startTime, endTime)));
            }

            MipLogger.WriteInfo(
              $"Found {sequences.Count} sequences in {stopwatch.Elapsed.TotalSeconds:N1} seconds between {interval.StartTime.ToLocalTime()} and {interval.EndTime.ToLocalTime()}");
            OnSequencesRetrieved(new List<TimelineSourceQueryResult> {result});
          }
          catch (Exception ex)
          {
            MipLogger.WriteError($"Exception thrown: {ex}");
          }
        }, _cancellationTokenSource.Token);
        task.ConfigureAwait(false);
        _tasks.Add(task);
        task.ContinueWith(t => _tasks.Remove(t));
      }
    }

    private EventFilter BuildFilter(string message, TimeInterval interval)
    {
      return new EventFilter
      {
        Conditions = new[]
        {
          new Condition
          {
            Target = Target.CameraId,
            Operator = Operator.Equals,
            Value = CameraFqid.ObjectId
          },
          new Condition
          {
            Target = Target.Message,
            Operator = Operator.Equals,
            Value = message
          },
          new Condition
          {
            Target = Target.Timestamp,
            Operator = Operator.GreaterThan,
            Value = interval.StartTime
          },
          new Condition
          {
            Target = Target.Timestamp,
            Operator = Operator.LessThan,
            Value = interval.EndTime
          }
        },
        Orders = new[]
        {
          new OrderBy
          {
            Target = Target.Timestamp,
            Order = Order.Ascending
          }
        }
      };
    }

    public void Dispose()
    {
      _cancellationTokenSource.Cancel();
      Task.WaitAll(_tasks.ToArray());
      _tasks.Clear();
      _cancellationTokenSource?.Dispose();
    }
  }
}
