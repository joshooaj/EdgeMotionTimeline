using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Proxy.AlarmClient;

namespace EnhancedTimeline.Admin
{
  public class TimelineSequenceDefinitionItemManager : ItemManager
  {
    private TimelineSequenceDefinitionUserControl _userControl;
    private IAlarmClient _alarmClient;
    private Guid _eventSession;
    private Task _eventMonitorTask;
    private CancellationTokenSource _cancellationTokenSource;

    public override void Init()
    {
      _cancellationTokenSource = new CancellationTokenSource();
    }

    public override void Close()
    {
      _cancellationTokenSource.Cancel();
      StopEventMonitor();
    }

    public override OperationalState GetOperationalState(Item item)
    {
      return item.Enabled ? OperationalState.Ok : OperationalState.Disabled;
    }

    public override string GetItemName()
    {
      return _userControl?.ItemName ?? string.Empty;
    }

    public override Item GetItem(FQID fqid)
    {
      return Configuration.Instance.GetItemConfiguration(
        PluginIds.PluginId,
        PluginKinds.EnhancedTimelineKind,
        fqid.ObjectId);
    }

    public override List<Item> GetItems()
    {
      return GetItems(null);
    }

    public override List<Item> GetItems(Item parentItem)
    {
      return Configuration.Instance.GetItemConfigurations(PluginIds.PluginId, parentItem, PluginKinds.EnhancedTimelineKind);
    }

    public override ItemNodeUserControl GenerateOverviewUserControl()
    {
      return new TimelineSequenceDefinitionOverviewUserControl();
    }

    public override UserControl GenerateAddUserControl(Item parentItem, FQID suggestedFQID)
    {
      return new TimelineSequenceDefinitionAddUserControl();
    }

    public override bool ValidateAddUserControl(UserControl addUserControl)
    {
      if (!(addUserControl is TimelineSequenceDefinitionAddUserControl control))
      {
        return false;
      }
      return control.ValidateItem();
    }

    public override UserControl GenerateDetailUserControl()
    {
      if (_userControl == null)
      {
        _userControl = new TimelineSequenceDefinitionUserControl();
        _userControl.ConfigurationChangedByUser += ConfigurationChangedByUserHandler;
      }
      
      return _userControl;
    }

    private void StartEventMonitor()
    {
      if (_eventMonitorTask != null) return;
      var filter = new EventFilter
      {
        Conditions = new[]
        {
          new Condition
          {
            Target = Target.CategoryName,
            Operator = Operator.NotEquals,
            Value = Category.VideoIn.ToString()
          },
          new Condition
          {
            Target = Target.Timestamp,
            Operator = Operator.GreaterThan,
            Value = DateTime.UtcNow.AddHours(-12)
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
      _alarmClient = new AlarmClientManager().GetAlarmClient(Configuration.Instance.ServerFQID.ServerId);
      _eventSession = _alarmClient.StartEventLineSession(filter);
      _eventMonitorTask = Task.Run(() =>
      {
        while (!_cancellationTokenSource.IsCancellationRequested)
        {
          var data = _alarmClient.GetSessionEventLines(int.MaxValue, _eventSession);
          if (data.Inserted.Length > 0)
          {
            _userControl?.InsertEvents(data.Inserted);
          }
        }
      }, _cancellationTokenSource.Token);
      _eventMonitorTask.ConfigureAwait(false);
    }

    private void StopEventMonitor()
    {
      if (_eventSession != Guid.Empty)
      {
        _alarmClient?.StopEventLineSession(_eventSession);
      }
    }

    public override void FillUserControl(Item item)
    {
      CurrentItem = item;
      _userControl?.FillContent(item);
      StartEventMonitor();
    }

    public override void ClearUserControl()
    {
      CurrentItem = null;
      _userControl?.ClearContent();
    }

    public override void ReleaseUserControl()
    {
      if (_userControl == null) return;
      _userControl.ConfigurationChangedByUser -= ConfigurationChangedByUserHandler;
      _userControl = null;
    }

    public override bool ValidateAndSaveUserControl()
    {
      CurrentItem.Name = _userControl.ItemName;
      CurrentItem.Properties[SequenceDefinitionProperties.StartEvent] = _userControl.StartEvent;
      CurrentItem.Properties[SequenceDefinitionProperties.StopEvent] = _userControl.StopEvent;
      CurrentItem.Properties[SequenceDefinitionProperties.RibbonColor] = _userControl.RibbonColor;
      Configuration.Instance.SaveItemConfiguration(PluginIds.PluginId, CurrentItem);
      return true;
    }

    // This may not get used at all when using an add control
    public override Item CreateItem(Item parentItem, FQID suggestedFQID)
    {
      CurrentItem = new Item(suggestedFQID, "Enter a name");
      return CurrentItem;
    }

    public override Item CreateItem(Item parentItem, FQID suggestedFQID, UserControl addUserControl)
    {
      if (!(addUserControl is TimelineSequenceDefinitionAddUserControl control))
      {
        EnvironmentManager.Instance.ExceptionDialog(
          nameof(TimelineSequenceDefinitionItemManager), 
          "CreateItem",
          new InvalidOperationException($"Expected TimelineSequenceDefinitionAddUserControl but received {addUserControl?.GetType().FullName} instead.")
          );
        return null;
      }

      CurrentItem = new Item(suggestedFQID, control.ItemName)
      {
        Properties =
        {
          [SequenceDefinitionProperties.StartEvent] = control.StartEvent,
          [SequenceDefinitionProperties.StopEvent] = control.StopEvent,
          [SequenceDefinitionProperties.RibbonColor] = control.RibbonColor
        }
      };
      Configuration.Instance.SaveItemConfiguration(PluginIds.PluginId, CurrentItem);
      return CurrentItem;
    }

    public override void SetItemName(string name)
    {
      if (_userControl != null)
      {
        _userControl.ItemName = name;
      }
    }

    public override void DeleteItem(Item item)
    {
      if (item == null) return;
      Configuration.Instance.DeleteItemConfiguration(PluginIds.PluginId, item);
    }

    public override Dictionary<string, string> GetConfigurationReportProperties(Item item)
    {
      var settings = new Dictionary<string, string>();
      if (item != null)
      {
        settings["Name"] = item.Name;
        settings["Enabled"] = item.Enabled.ToString();
        settings[SequenceDefinitionProperties.StartEvent] = item.Properties[SequenceDefinitionProperties.StartEvent];
        settings[SequenceDefinitionProperties.StopEvent] = item.Properties[SequenceDefinitionProperties.StopEvent];
        settings[SequenceDefinitionProperties.RibbonColor] = item.Properties[SequenceDefinitionProperties.RibbonColor];
      }
      return settings;
    }
  }
}