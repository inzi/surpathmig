using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

public static class SurpathStringExtensions
{
    // these are for when you have Model.class.class1 but the posting class only objectd.class1
    // idfor and namefor will do class1.property, when the posting data really needs the property or object
    // these will map class 1 to the rood of the posted data
    
    public static string DropRootHtmlIdForClass(this string _htmlName, string key = "")
    {
        var separator = '_';
        var parts = _htmlName.Split(separator).Skip(1).ToArray();
        //separater.ToString().join("12345").join(("(", ")"));
        if (parts.Length > 1)
        {
            _htmlName = string.Join(separator.ToString(), parts);
        }
        else
        {
            return _htmlName;
        }
        return _htmlName;
    }
    public static string DropRootHtmlNameForClass(this string _htmlName)
    {
        var separator = '.';
        var parts = _htmlName.Split(separator).Skip(1).ToArray();
        //separater.ToString().join("12345").join(("(", ")"));
        if (parts.Length > 1)
        {
            _htmlName = string.Join(separator.ToString(), parts);
        }
        else
        {
            return _htmlName;
        }
        return _htmlName;
    }

    public static bool IsJson(this string source)
    {
        if (source == null)
            return false;

        try
        {
            JsonDocument.Parse(source);
            return true;
        }
        catch (System.Text.Json.JsonException)
        {
            return false;
        }
    }
}