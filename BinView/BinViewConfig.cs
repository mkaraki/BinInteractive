using Microsoft.JSInterop;
using System.Text.Json;

namespace BinView
{

    public static class BinViewConfig
    {
        public static void Init(IJSRuntime jsRuntime)
        {
            if (_jsRuntime == null)
                _jsRuntime = (IJSInProcessRuntime)jsRuntime;
            BinViewConfig.LoadConfig();
        }

        public static IJSInProcessRuntime _jsRuntime { private get; set; } = null;

        public static BinViewHighlightMode HighlightMode { get; set; } = BinViewHighlightMode.normal;

        public enum BinViewHighlightMode
        {
            normal,
            ascii,
            disabled,
        }

        public static string GetHighlightModeText()
        {
            switch (HighlightMode)
            {
                case BinViewHighlightMode.normal:
                    return "normal";
                case BinViewHighlightMode.ascii:
                    return "ascii";
                case BinViewHighlightMode.disabled:
                    return "disabled";
            }

            return "unknown";
        }

        public static void SetHighlightModeText(string text)
        {
            switch (text)
            {
                case "normal":
                    HighlightMode = BinViewHighlightMode.normal;
                    break;
                case "ascii":
                    HighlightMode = BinViewHighlightMode.ascii;
                    break;
                case "disabled":
                    HighlightMode = BinViewHighlightMode.disabled;
                    break;
                default:
                    throw new ArgumentException($"Invalid highlight mode text: {text}");
            }
        }

        public static void SaveConfig()
        {
            var configJson = JsonSerializer.Serialize(new BinViewConfigJsonType { 
                HighlightMode = GetHighlightModeText(),
            });

            Console.WriteLine(configJson);

            _jsRuntime.InvokeVoid("localStorage.setItem", "binViewConfig", configJson);
        }

        public static void LoadConfig()
        {
            var configJson = _jsRuntime.Invoke<string?>("localStorage.getItem", "binViewConfig");
            if (string.IsNullOrEmpty(configJson))
            {
                return;
            }

            var config = JsonSerializer.Deserialize<BinViewConfigJsonType>(configJson);

            if (config != null)
            {
                SetHighlightModeText(config.HighlightMode ?? "normal");
            }
        }

        private class BinViewConfigJsonType
        {
            public string? HighlightMode { get; set; } = "normal";
        }
    }
}
