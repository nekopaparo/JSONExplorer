using System;
using System.Collections.Generic;
using System.IO;

namespace JSONExplorer.Models
{
    public static class JsonExplorer
    {
        private static int Index;
        // Json <=> string
        public static string ToString(object obj)
        {
            string _string = null;

            Type type = obj.GetType();
            if (type == typeof(Dictionary<string, object>))
            {
                _string = OutputAllDictionary(_string, obj as Dictionary<string, object>, null);
            }
            else if (type == typeof(List<object>))
            {
                _string = OutputAllList(_string, obj as List<object>, null);
            }
            else if (type == typeof(string))
            {
                _string = String.Format("\"{0}\"", obj);
            }
            // Double / int
            else
            {
                _string = obj.ToString();
            }

            return _string;
        }
        public static object ToOject(string data)
        {
            Index = 0;

            string DATA = RemoveBlank(data);
            char c = DATA[0];
            if (c is '{')
            {
                return AddDictionaryValue(DATA, GetDictionary);
            }
            else if (c is '[')
            {
                return AddListValue(DATA, GetList);
            }
            else
            {
                return (c is '\"' || c is '\'') ?
                     GetWord(DATA, c) : GetDigit(DATA);
            }

        }
        // 新增 Dictionary or Array
        public static Dictionary<string, object> GetDictionary => new Dictionary<string, object>();
        public static List<object> GetList => new List<object>();
        // Input 輸入Object
        private static Dictionary<string, object> AddDictionaryValue(string DATA, Dictionary<string, object> dictionary)
        {
            string key = null;
            object value = null;
            bool isValue = false;
            char c;
            while (DATA[++Index] != '}')
            {
                c = DATA[Index];

                if (isValue)
                {
                    if (c == '{')
                    {
                        value = AddDictionaryValue(DATA, GetDictionary);
                    }
                    else if (c == '[')
                    {
                        value = AddListValue(DATA, GetList);
                    }
                    else if (c == '\"' || c == '\'')
                    {
                        value = GetWord(DATA, c);
                    }
                    else
                    {
                        value = GetDigit(DATA);
                    }
                    isValue = false;
                }
                else
                {
                    if (c is '\"' || c is '\'')
                    {
                        key = GetWord(DATA, c);
                    }
                    else if (c is ':')
                    {
                        isValue = true;
                    }
                }

                if (value != null && key != null)
                {
                    //dictionary.Add(key, value);
                    dictionary[key] = value;
                    key = null;
                    value = null;
                }
            }

            return dictionary;
        }
        private static List<object> AddListValue(string DATA, List<object> array)
        {
            object value = null;
            char c;
            while (DATA[++Index] != ']')
            {
                c = DATA[Index];

                if (c is '{')
                {
                    value = AddDictionaryValue(DATA, GetDictionary);
                }
                else if (c is '[')
                {
                    value = AddListValue(DATA, GetList);
                }
                else if (c is '\"' || c is '\'')
                {
                    value = GetWord(DATA, c);
                }
                else if (Char.IsDigit(c))
                {
                    value = GetDigit(DATA);
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
        private static string GetWord(string DATA, char end)
        {
            string word = null;
            while (DATA[++Index] != end)
            {
                word += DATA[Index];
            }
            return word;
        }
        private static object GetDigit(string DATA)
        {
            --Index;
            string word = null;
            bool isString = false;
            bool isDouble = false;
            char c;
            while (DATA[++Index] != ',')
            {
                c = DATA[Index];

                if (c is '}' || c is ']')
                {
                    --Index;
                    break;
                }
                else if (!isString)
                {
                    if (c is '.')
                    {
                        if (isDouble)
                        {
                            isString = true;
                        }
                        else
                        {
                            isDouble = true;
                        }
                    }
                    else if (word == null && c is '-')
                    {
                        //
                    }
                    else if (c < '0' || c > '9')
                    {
                        isString = true;
                    }
                }
                word += c;
            }

            if (isString)
            {
                return word;
            }
            else if (isDouble)
            {
                if (Double.TryParse(word, out Double D))
                {
                    return D;
                }
            }
            else
            {
                if (int.TryParse(word, out int I))
                {
                    return I;
                }
            }

            return word;
        }
        // Output 輸出字串
        private static string OutputAllDictionary(string output, Dictionary<string, object> dictionary, string blank)
        {
            bool isFirst = true;
            Type valueType;
            string b = blank;
            blank += '\t';
            output += "{\r\n";
            foreach (KeyValuePair<string, object> kvp in dictionary)
            {
                // 末端用,分隔
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                {
                    output += ",\r\n";
                }
                // "key" : 
                output += String.Format("{0}\"{1}\" : ", blank, FilterString(kvp.Key));
                // 類型
                valueType = kvp.Value.GetType();
                // is dictionary
                if (valueType == typeof(Dictionary<string, object>))
                {
                    output = OutputAllDictionary(output, kvp.Value as Dictionary<string, object>, blank);
                }
                // is array
                else if (valueType == typeof(List<object>))
                {
                    output = OutputAllList(output, kvp.Value as List<object>, blank);
                }
                // is string 加上""作為標記
                else if (valueType == typeof(string))
                {
                    output += String.Format("\"{0}\"", FilterString(kvp.Value as string));
                }
                else
                {
                    output += FilterString(kvp.Value.ToString());
                }
            }
            output += ("\r\n" + b + '}');
            return output;
        }
        private static string OutputAllList(string output, List<object> os, string blank)
        {
            bool isFirst = true;
            Type valueType;
            string b = blank;
            blank += '\t';
            output += "[";
            foreach (object o in os)
            {
                // 末端用,分隔
                if (isFirst)
                {
                    isFirst = false;
                }
                else
                { 
                    output += ','; 
                }
                // 換行+前面的空白
                output += ("\r\n" + blank);
                // 類型
                valueType = o.GetType();
                // is dictionary
                if (valueType == typeof(Dictionary<string, object>))
                {
                    output = OutputAllDictionary(output, o as Dictionary<string, object>, blank);
                }
                // is array
                else if (valueType == typeof(List<object>))
                {
                    output = OutputAllList(output, o as List<object>, blank);
                }
                // is string 加上""作為標記
                else if (valueType == typeof(string))
                {
                    output += String.Format("\"{0}\"", FilterString(o as string));
                }
                else
                {
                    output += FilterString(o.ToString());
                }
            }
            output += ("\r\n" + b + ']');
            return output;
        }
        // 去除 "
        public static string FilterString(string str)
        {
            List<char> result = new List<char>();

            foreach (char c in str)
            {
                if (c is '\"')
                {
                    result.Add('\'');
                }
                else
                {
                    result.Add(c);
                }
            }
            return new string(result.ToArray());
        }
        // 去除空白
        public static string RemoveBlank(string str)
        {
            List<char> result = new List<char>();

            bool isWord = false;

            foreach (char c in str)
            {
                if (c is '\"')
                {
                    isWord = !isWord;
                }

                if ((c is ' ' || c is '\t' || c is '\n' || c is '\r') && !isWord)
                {
                    continue;
                }

                result.Add(c);
            }
            return new string(result.ToArray());
        }
        // 讀取檔案
        public static Dictionary<string, object> ReadFile(string path)
        {
            string input = null, readText;
            Index = 0;
            try
            {
                StreamReader Reader = new StreamReader(path);
                while ((readText = Reader.ReadLine()) != null)
                {
                    input += RemoveBlank(readText);
                }

                return AddDictionaryValue(input, GetDictionary);
            }
            catch (FileNotFoundException ex)
            {
                //System.Console.WriteLine(ex.ToString());
                return null;
            }
            
        }
        // 輸出檔案
        public static bool WriteFile(string path, string output)
        {
            try
            {
                //new StreamWriter(path).WriteLine(Output);
                File.WriteAllText(path, output);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return false;
            }
            
        }
        public static bool WriteFile(string path, Dictionary<string, object> Json)
        {
            //new StreamWriter(path).WriteLine(Output);
            string output = OutputAllDictionary(null, Json, null);
            try
            {
                File.WriteAllText(path, output);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
