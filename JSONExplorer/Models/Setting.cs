using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONExplorer.Models
{
    public class Setting : NotifyPropertyChanged
    {
        private const string path = "setter.json";
        private Dictionary<string, object> Setting_ { get; }

        private int textBoxFontSize;
        public int TextBoxFontSize
        {
            get => textBoxFontSize;
            set
            {
                if (textBoxFontSize != value)
                {
                    textBoxFontSize = value;
                    Setting_[nameof(TextBoxFontSize)] = value;
                    OnPropertyChanged(nameof(TextBoxFontSize));
                }
            }
        }
        public void ReSet()
        {
            TextBoxFontSize = 15;
        }
        public Setting()
        {
            Setting_ = JsonExplorer.ReadFile(path);
            if (Setting_ != null)
            {
                textBoxFontSize = (int)Setting_["TextBoxFontSize"];
            }
            else
            {
                Setting_ = JsonExplorer.GetDictionary;
                ReSet();
            }
        }
        ~ Setting()
        {
            JsonExplorer.WriteFile(path, Setting_);
        }
    }
}
