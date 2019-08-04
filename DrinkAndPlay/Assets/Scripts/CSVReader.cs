using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, string>> ReadWithHeaders(string fileName)
    {
        var table = new List<Dictionary<string, string>>();
        TextAsset csvFile = Resources.Load(fileName) as TextAsset;

        var lines = Regex.Split(csvFile.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return table;

        var headers = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 1; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var rowInTable = new Dictionary<string, string>();
            for (var j = 0; j < headers.Length && j < values.Length; j++)
            {
                //Clean value
                string value = values[j].Replace("\"\"", "\"");
                value = UnquoteString(value);
                value = value.Replace("\\", "");
                string finalvalue = value;

                rowInTable[headers[j]] = finalvalue;
            }
            table.Add(rowInTable);
        }
        return table;
    }

    public static List<string[]> Read(Section section)
    {
        if (section == null)
        {
            Debug.LogError("Trying to read the localization file of a 'null' section");
            return null;
        }

        var table = new List<string[]>();

        TextAsset csvFile = Resources.Load(section.name) as TextAsset;

        var lines = Regex.Split(csvFile.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return null;

        var headers = Regex.Split(lines[0], SPLIT_RE);

        for (var i = 0; i < lines.Length; i++)
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var rowInTable = new string[headers.Length];
            for (var j = 0; j < headers.Length && j < values.Length; j++)
            {
                //Clean value
                string value = values[j].Replace("\"\"", "\"");
                value = UnquoteString(value);
                value = value.Replace("\\", "");
                string finalvalue = value;

                rowInTable[j] = finalvalue;
            }
            table.Add(rowInTable);
        }
        return table;
    }

    public static string UnquoteString(string str)
    {
        if (String.IsNullOrEmpty(str))
            return str;

        int length = str.Length;
        if (length > 1 && str[0] == '\"' && str[length - 1] == '\"')
            str = str.Substring(1, length - 2);

        return str;
    }


    /*   // ORIGINAL METHOD //
    public static List<Dictionary<string, object>> Read(string fileName) 
    {
        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(fileName) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j].Replace("\"\"", "\"");
                value = UnquoteString(value);
                value = value.Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
    */
}