using JSONExplorer.Models;
using JSONExplorer.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace JSONExplorer.ViewModels
{
    public struct Detail
    {
        public object Data { get; }
        public object Key { get; }
        public readonly Type DataType;
        public Detail(object Data, object Key)
        {
            this.Data = Data;
            this.Key = Key;
            this.DataType = Data.GetType();
        }

        public object GetValue()
        {
            if (Key == null)
            {
                return Data;
            }

            if (DataType == typeof(Dictionary<string, object>))
            {
                return (Data as Dictionary<string, object>)[Key as string];
            }
            else if (DataType == typeof(List<object>))
            {
                return (Data as List<object>)[(int)Key];
            }
            return null;
        }

        public string GetString()
        {
            return JsonExplorer.ToString(GetValue());
        }
        public string GetString(object value)
        {
            return JsonExplorer.ToString(value);
        }

        public bool SetValue(object newValue)
        {
            if (DataType == typeof(Dictionary<string, object>))
            {
                (Data as Dictionary<string, object>)[Key as string] = newValue;
            }
            else if (DataType == typeof(List<object>))
            {
                (Data as List<object>)[(int)Key] = newValue;
            }
            else
            {
                return false;
            }
            return true;
        }
        public bool SetValue(string newValue)
        {
            return SetValue(JsonExplorer.ToOject(newValue));
        }
    }

    public class ViewModel : NotifyPropertyChanged
    {
        public MainWindow MainWindow { get; set; }

        public Point GetPosition()
        {
            Point point = System.Windows.Input.Mouse.GetPosition(MainWindow);
            point.X += MainWindow.Left;
            point.Y += MainWindow.Top;
            return point;
        }

        private Dictionary<string, object> jsonData = null;
        public Dictionary<string, object> JsonData
        {
            get => jsonData;
            set
            {
                jsonData = (value == null) ? JsonExplorer.GetDictionary : value;
                JsonView = new JsonView();
            }
        }

        private string path = null;
        public string Path
        {
            get => path;
            set
            {
                if (!Regex.Match(value, @"\w+\.json\b").Success)
                {
                    value += ".json";
                }
                if (value != path)
                {
                    if (Regex.Match(value, @"\bNewJson\w+").Success)
                    {
                        
                        path = value.Substring(7);
                        JsonData = null;
                        SaveJson();
                    }
                    else
                    {
                        path = value;
                        JsonData = JsonExplorer.ReadFile(value);
                    }
                }
            }
        }

        public Setting Setting { get; }

        private JsonView jsonView = null;
        public JsonView JsonView
        {
            get => jsonView;
            set
            {
                if (value != jsonView)
                {
                    jsonView = value;
                    jsonView.AddNavbar(Path.Split('\\').Last(), new Detail(JsonData, null));
                    OnPropertyChanged(nameof(JsonView));
                }
            }
        }

        private KeyView keyView = null;
        public KeyView KeyView
        {
            get => keyView;
            set
            {
                if (value != keyView)
                {
                    keyView = value;
                    OnPropertyChanged(nameof(KeyView));
                }
            }
        }

        private ValueView valueView = null;
        public ValueView ValueView
        {
            get => valueView;
            set
            {
                if (value != valueView)
                {
                    valueView = value;
                    OnPropertyChanged(nameof(ValueView));
                }
            }
        }

        public ViewModel()
        {
            Setting = new Setting();
        }

        public bool OpenJson(string path)
        {
            JsonData = JsonExplorer.ReadFile(path);
            
            return JsonData != null;
        }
        public bool SaveJson()
        {
            return JsonExplorer.WriteFile(Path, JsonData);
        }
        public bool SaveJson(string path)
        {
            return JsonExplorer.WriteFile(path, JsonData);
        }
    }
}
