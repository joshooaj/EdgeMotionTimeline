using System;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Platform;
using VideoOS.Platform.Proxy.Alarm;
using VideoOS.Platform.Proxy.AlarmClient;

namespace EnhancedTimeline.Admin
{
  public partial class TimelineSequenceDefinitionUserControl : UserControl
  {
    public TimelineSequenceDefinitionUserControl()
    {
      InitializeComponent();
      txtItemName.TextChanged += (sender, args) => OnConfigurationChangedByUser();
      txtStartEvent.TextChanged += (sender, args) => OnConfigurationChangedByUser();
      txtStopEvent.TextChanged += (sender, args) => OnConfigurationChangedByUser();
      txtRibbonColor.TextChanged += (sender, args) => OnConfigurationChangedByUser();
      txtRibbonColor.TextChanged += UpdateColorPreview;

      grid.ColumnCount = 3;
      grid.RowHeadersVisible = false;
      grid.Columns[0].Name = "Time";
      grid.Columns[1].Name = "Message";
      grid.Columns[2].Name = "Source";
    }

    private void UpdateColorPreview(object sender, EventArgs e)
    {
      try
      {
        var color = ColorTranslator.FromHtml(txtRibbonColor.Text);
        colorBox.BackColor = color;
      }
      catch
      {
        // Ignore and leave the color preview alone until the value is valid.
      }
    }

    public string ItemName
    {
      get => txtItemName.Text;
      set => txtItemName.Text = value;
    }

    public string StartEvent
    {
      get => txtStartEvent.Text;
      set => txtStartEvent.Text = value;
    }

    public string StopEvent
    {
      get => txtStopEvent.Text;
      set => txtStopEvent.Text = value;
    }

    public string RibbonColor
    {
      get => txtRibbonColor.Text;
      set => txtRibbonColor.Text = value;
    }

    public event EventHandler ConfigurationChangedByUser;

    protected virtual void OnConfigurationChangedByUser()
    {
      ConfigurationChangedByUser?.Invoke(this, EventArgs.Empty);
    }

    public void ClearContent()
    {
      ItemName = string.Empty;
      StartEvent = string.Empty;
      StopEvent = string.Empty;
      RibbonColor = string.Empty;
    }

    public void FillContent(Item item)
    {
      var sequenceDefinition = new SequenceDefinition(item);
      ItemName = sequenceDefinition.Title;
      StartEvent = sequenceDefinition.StartEvent;
      StopEvent = sequenceDefinition.StopEvent;
      RibbonColor = sequenceDefinition.RibbonColor;
    }

    public void InsertEvents(EventLine[] linesInserted)
    {
      Invoke((MethodInvoker) (() =>
      {
        foreach (var line in linesInserted)
        {
          while (grid.Rows.Count >= 100)
          {
            grid.Rows.RemoveAt(grid.Rows.Count - 1);
          }
          grid.Rows.Insert(0, line.Timestamp.ToLocalTime().ToString("G"), line.Message, line.SourceName);
        }
      }));
    }

    private void buttonColor_Click(object sender, EventArgs e)
    {
      var dialog = new ColorDialog
      {
        AllowFullOpen = true,
        AnyColor = true,
        FullOpen = true
      };
      if (dialog.ShowDialog() == DialogResult.OK)
      {
        RibbonColor = ColorTranslator.ToHtml(dialog.Color);
      }
    }
  }
}
