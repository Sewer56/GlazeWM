using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Windows.Threading;
using GlazeWM.Bar.Common;
using GlazeWM.Bar.Components;
using GlazeWM.Domain.Monitors;
using GlazeWM.Domain.UserConfigs;
using GlazeWM.Infrastructure.Utils;

namespace GlazeWM.Bar
{
  public class BarViewModel : ViewModelBase
  {
    public readonly Subject<bool> WindowClosing = new();

    public Monitor Monitor { get; }
    public Dispatcher Dispatcher { get; }
    public BarConfig BarConfig { get; }

    public BarPosition Position => BarConfig.Position;
    public string Background => XamlHelper.FormatColor(BarConfig.Background);
    public string Foreground => XamlHelper.FormatColor(BarConfig.Foreground);
    public string FontFamily => BarConfig.FontFamily;
    public string FontWeight => BarConfig.FontWeight;
    public string FontSize => XamlHelper.FormatSize(BarConfig.FontSize);
    public string BorderColor => XamlHelper.FormatColor(BarConfig.BorderColor);
    public string BorderWidth => XamlHelper.FormatRectShorthand(BarConfig.BorderWidth);
    public string BorderRadius => XamlHelper.FormatSize(BarConfig.BorderRadius);
    public string Padding => XamlHelper.FormatRectShorthand(BarConfig.Padding);
    public double Opacity => BarConfig.Opacity;

    private TextComponentViewModel _componentSeparatorLeft => new(
        this, new TextComponentConfig
        {
          Text = BarConfig.ComponentSeparator.LabelLeft
            ?? BarConfig.ComponentSeparator.Label
        }
    );

    private TextComponentViewModel _componentSeparatorCenter => new(
        this, new TextComponentConfig
        {
          Text = BarConfig.ComponentSeparator.LabelCenter
            ?? BarConfig.ComponentSeparator.Label
        }
    );

    private TextComponentViewModel _componentSeparatorRight => new(
        this, new TextComponentConfig
        {
          Text = BarConfig.ComponentSeparator.LabelRight
            ?? BarConfig.ComponentSeparator.Label
        }
    );

    public List<ComponentViewModel> ComponentsLeft =>
      InsertComponentSeparator(
        CreateComponentViewModels(BarConfig.ComponentsLeft),
          _componentSeparatorLeft);

    public List<ComponentViewModel> ComponentsCenter =>
      InsertComponentSeparator(
        CreateComponentViewModels(BarConfig.ComponentsCenter),
          _componentSeparatorCenter);

    public List<ComponentViewModel> ComponentsRight =>
      InsertComponentSeparator(
        CreateComponentViewModels(BarConfig.ComponentsRight),
          _componentSeparatorRight);

    private static List<ComponentViewModel> InsertComponentSeparator(
      List<ComponentViewModel> componentViewModels, TextComponentViewModel componentSeparator
    )
    {
      componentViewModels.Intersperse(componentSeparator);
      return componentViewModels;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        WindowClosing.Dispose();
      }

      base.Dispose(disposing);
    }

    private List<ComponentViewModel> CreateComponentViewModels(
      List<BarComponentConfig> componentConfigs)
    {
      return componentConfigs.ConvertAll<ComponentViewModel>(config => config switch
      {
        BatteryComponentConfig bsc => new BatteryComponentViewModel(this, bsc),
        BindingModeComponentConfig bmc => new BindingModeComponentViewModel(this, bmc),
        ClockComponentConfig ccc => new ClockComponentViewModel(this, ccc),
        TextComponentConfig tcc => new TextComponentViewModel(this, tcc),
        CPUStatsComponentConfig cscc => new CPUStatsComponentViewModel(this, cscc),
        GPUStatsComponentConfig gscc => new GPUStatsComponentViewModel(this, gscc),
        RAMStatsComponentConfig rscc => new RAMStatsComponentViewModel(this, rscc),
        WeatherComponentConfig wcc => new WeatherComponentViewModel(this, wcc),
        NetworkComponentConfig ncc => new NetworkComponentViewModel(this, ncc),
        TilingDirectionComponentConfig tdc => new TilingDirectionComponentViewModel(this, tdc),
        WorkspacesComponentConfig wcc => new WorkspacesComponentViewModel(this, wcc),
        WindowTitleComponentConfig wtcc => new WindowTitleComponentViewModel(this, wtcc),
        SystemTrayComponentConfig stcc => new SystemTrayComponentViewModel(this, stcc),
        MediaComponentConfig mcc => new MediaComponentViewModel(this, mcc),
        ScriptedTextConfig stc => new ScriptedTextComponentViewModel(this, stc),
        TextFileConfig stc => new TextFileComponentViewModel(this, stc),
        _ => throw new ArgumentOutOfRangeException(nameof(config)),
      });
    }

    public BarViewModel(Monitor monitor, Dispatcher dispatcher, BarConfig barConfig)
    {
      Monitor = monitor;
      Dispatcher = dispatcher;
      BarConfig = barConfig;
    }
  }
}
