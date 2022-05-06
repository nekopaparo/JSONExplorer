using JSONExplorer.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JSONExplorer.Models
{
    public class JsonExplorer
    {
        // 變數
        private string output = null;
        private Dictionary<string, object> JSON { get; set; }
        private string Data = null;
        private int index = 0;
        // 公開變數
        public string Output
        {
            get
            {
                if (output == null)
                {
                    OutputAllDictionary(GetJson, null);
                }
                return output;
            }
        }
        public Dictionary<string, object> GetJson => JSON;

        // 建構子
        public JsonExplorer(string path)
        {
            ReadJson(path);
            JSON = AddDictionaryValue(GetDictionary);
        }
        public string ToString(object obj)
        {
            string tmp = output;
            output = null;
            Type type = obj.GetType();
            if (type == typeof(Dictionary<string, object>))
            {
                OutputAllDictionary(obj as Dictionary<string, object>, null);
            }
            else if (type == typeof(List<object>))
            {
                OutputShowAllArray(obj as List<object>, null);
            }
            else
            {
                output = obj.ToString();
            }
            string result = output;
            output = tmp;
            return result;
        }
        public object ToValue(string data)
        {
            index = 0;
            string tmp = Data;
            Data = RemoveBlank(data);

            System.Console.Write(Data);

            object result = "error";

            if (Data[0] is '{')
            {
                result = AddDictionaryValue(GetDictionary);
            }
            else if (Data[0] is '[')
            {
                result = AddArrayValue(GetArray);
            }

            Data = tmp;

            return result;
        }
        // 新增 Dictionary or Array
        private static Dictionary<string, object> GetDictionary => new Dictionary<string, object>();
        private static List<object> GetArray => new List<object>();
        // 值輸入 
        private Dictionary<string, object> AddDictionaryValue(Dictionary<string, object> dictionary)
        {
            string key = null;
            object value = null;
            bool isValue = false;
            char c;
            while (Data[++index] is not '}')
            {
                c = Data[index];

                if (isValue)
                {
                    if (c == '{')
                    {
                        value = AddDictionaryValue(GetDictionary);
                    }
                    else if (c == '[')
                    {
                        value = AddArrayValue(GetArray);
                    }
                    else if (c == '\"')
                    {
                        value = GetWord();
                    }
                    else if (Char.IsDigit(c))
                    {
                        value = GetDigit();
                    }
                    else
                    {
                        System.Console.WriteLine("value error"); // 目前沒有
                        continue;
                    }
                    isValue = false;
                }
                else
                {
                    if (c is '\"') key = GetWord();
                    else if (c is ':') isValue = true;
                }

                if (value != null && key != null)
                {
                    dictionary.Add(key, value);
                    key = null;
                    value = null;
                }
            }

            return dictionary;
        }
        private List<object> AddArrayValue(List<object> array)
        {
            object value = null;
            char c;
            while (Data[++index] is not ']')
            {
                c = Data[index];

                if (c is '{')
                {
                    value = AddDictionaryValue(GetDictionary);
                }
                else if (c is '[')
                {
                    value = AddArrayValue(GetArray);
                }
                else if (c is '\"')
                {
                    value = GetWord();
                }
                else if (Char.IsDigit(c))
                {
                    value = GetDigit();
                }
                else
                {
                    //System.Console.WriteLine("value error 145 => {0}", c); // => , 目前都是逗號
                }
                if (value != null)
                {
                    array.Add(value);
                    value = null;
                }
            }
            return array;
        }
        // 取得 字串 or 數字
        private string GetWord()
        {
            string word = null;
            while (Data[++index] is not '\"') word += Data[index];
            return word;
        }
        private object GetDigit()
        {
            --index;
            string word = null;
            bool isDouble = false;
            char c;
            while (Data[++index] != ',')
            {
                c = Data[index];
                if (c is '}' or ']')
                {
                    --index;
                    break;
                }
                else if (c is '.')
                {
                    isDouble = true;
                }
                word += c;
            }
            return isDouble ? float.Parse(word) : int.Parse(word);
        }
        // Output 輸出字串
        private void OutputAllDictionary(Dictionary<string, object> dictionary, string blank)
        {
            bool isFirst = true;
            Type valueType;
            string b = blank;
            blank += '\t';
            output += "{\r\n";
            foreach (KeyValuePair<string, object> kvp in dictionary)
            {
                // 末端用,分隔
                if (isFirst) isFirst = false;
                else output += ",\r\n";
                // "key" : 
                output += String.Format("{0}\"{1}\" : ", blank, kvp.Key);
                // 類型
                valueType = kvp.Value.GetType();
                // is dictionary
                if (valueType == typeof(Dictionary<string, object>))
                {
                    OutputAllDictionary((Dictionary<string, object>)kvp.Value, blank);
                }
                // is array
                else if (valueType == typeof(List<object>))
                {
                    OutputShowAllArray((List<object>)kvp.Value, blank);
                }
                // is string 加上""作為標記
                else if (valueType == typeof(string))
                {
                    output += String.Format("\"{0}\"", kvp.Value);
                }
                else
                {
                    output += kvp.Value;
                }
            }
            output += ("\r\n" + b + '}');
        }
        private void OutputShowAllArray(List<object> os, string blank)
        {
            bool isFirst = true;
            Type valueType;
            string b = blank;
            blank += '\t';
            output += "[";
            foreach (object o in os)
            {
                // 末端用,分隔
                if (isFirst) isFirst = false;
                else output += ',';
                // 換行+前面的空白
                output += ("\r\n" + blank);
                // 類型
                valueType = o.GetType();
                // is dictionary
                if (valueType == typeof(Dictionary<string, object>))
                {
                    OutputAllDictionary((Dictionary<string, object>)o, blank);
                }
                // is array
                else if (valueType == typeof(List<object>))
                {
                    OutputShowAllArray((List<object>)o, blank);
                }
                // is string 加上""作為標記
                else if (valueType == typeof(string))
                {
                    output += String.Format("\"{0}\"", o);
                }
                else
                {
                    output += o;
                }
            }
            output += ("\r\n" + b + ']');
        }
        // 去除空白
        public string RemoveBlank(string str)
        {
            List<char> result = new();

            bool isWord = false;

            foreach (char c in str)
            {
                if (c is '\"')
                {
                    isWord = !isWord;
                }

                if ((c is ' ' or '\t' or '\n' or '\r') && !isWord)
                {
                    continue;
                }

                result.Add(c);
            }
            return new string(result.ToArray());
        }
        // 讀取檔案
        public void ReadJson(string path)
        {
            StreamReader readtext = new StreamReader(path);
            string readText;
            Data = null;
            while ((readText = readtext.ReadLine()) != null)
            {
                Data += RemoveBlank(readText);
            }
            index = 0;
        }
        // 輸出檔案
        public async Task WriteFile(string path)
        {
            //new StreamWriter(path).WriteLine(Output);
            await File.WriteAllTextAsync(path, Output);
        }
    }
}
