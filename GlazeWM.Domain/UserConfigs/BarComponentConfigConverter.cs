using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GlazeWM.Domain.UserConfigs
{
  public class BarComponentConfigConverter : JsonConverter<BarComponentConfig>
  {
    public override BarComponentConfig Read(
      ref Utf8JsonReader reader,
      Type typeToConvert,
      JsonSerializerOptions options)
    {
      using var jsonObject = JsonDocument.ParseValue(ref reader);

      // Get the type of bar component (eg. "workspaces").
      var typeDiscriminator = jsonObject.RootElement.GetProperty("type").ToString();

      return typeDiscriminator switch
      {
        "battery" =>
          JsonSerializer.Deserialize<BatteryComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "binding mode" =>
          JsonSerializer.Deserialize<BindingModeComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "clock" =>
          JsonSerializer.Deserialize<ClockComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "text" =>
          JsonSerializer.Deserialize<TextComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "network" =>
          JsonSerializer.Deserialize<NetworkComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "tiling direction" =>
          JsonSerializer.Deserialize<TilingDirectionComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "window title" =>
        JsonSerializer.Deserialize<WindowTitleComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "cpu stats" =>
        JsonSerializer.Deserialize<CPUStatsComponentConfig>(
          jsonObject.RootElement.ToString(),
          options
        ),
        "weather" =>
        JsonSerializer.Deserialize<WeatherComponentConfig>(
          jsonObject.RootElement.ToString(),
          options
        ),
        "gpu stats" =>
          JsonSerializer.Deserialize<GPUStatsComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "ram stats" =>
          JsonSerializer.Deserialize<RAMStatsComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "system tray" =>
          JsonSerializer.Deserialize<SystemTrayComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "media" =>
          JsonSerializer.Deserialize<MediaComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "workspaces" =>
          JsonSerializer.Deserialize<WorkspacesComponentConfig>(
            jsonObject.RootElement.ToString(),
            options
          ),
        "scripted text" => JsonSerializer.Deserialize<ScriptedTextConfig>(
          jsonObject.RootElement.ToString(),
          options
        ),
        "text file" => JsonSerializer.Deserialize<TextFileConfig>(
          jsonObject.RootElement.ToString(),
          options
        ),
        _ => throw new ArgumentException($"Invalid component type '{typeDiscriminator}'."),
      };
    }

    /// <summary>
    /// Serializing is not needed, so it's fine to leave it unimplemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public override void Write(
      Utf8JsonWriter writer,
      BarComponentConfig value,
      JsonSerializerOptions options)
    {
      throw new NotImplementedException();
    }
  }
}
