using System;
using System.Drawing;
using System.Windows.Forms;

namespace EnhancedTimeline.Admin
{
  public partial class TimelineSequenceDefinitionAddUserControl : UserControl
  {
    public TimelineSequenceDefinitionAddUserControl()
    {
      InitializeComponent();
      var random = new Random();
      RibbonColor = $"#{Color.FromArgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256)).Name}";
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

    public bool ValidateItem()
    {
      return true;
    }
  }
}
