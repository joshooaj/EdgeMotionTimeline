using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using VideoOS.Platform;

namespace EnhancedTimeline.Admin
{
  public class SequenceDefinition
  {
    public string Title { get; }
    public string StartEvent { get; }
    public string StopEvent { get; }
    public string RibbonColor { get; }

    public SequenceDefinition(Item item)
    {
      Title = item.Name;
      StartEvent = item.Properties[SequenceDefinitionProperties.StartEvent] ?? "";
      StopEvent = item.Properties[SequenceDefinitionProperties.StopEvent] ?? "";
      try
      {
        var color = ColorTranslator.FromHtml(item.Properties[SequenceDefinitionProperties.RibbonColor]);
        RibbonColor = ColorTranslator.ToHtml(color);
      }
      catch
      {
        RibbonColor = ColorTranslator.ToHtml(Color.DodgerBlue);
      }
    }
  }
}
