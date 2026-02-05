using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class SurpathGuidExtensions
{
    public static bool ActualGuid(this Guid? _guid)
    {
        if (_guid == null || _guid == Guid.Empty)
        {
            return false;
        }
        return true;
    }

    public static bool IsNullOrEmpty(this Guid? guid)
    {
        return (!guid.HasValue || guid.Value == Guid.Empty);
    }

    //public static bool HasValue(this Guid? guid)
    //{
    //    if (guid == null || guid == Guid.Empty)
    //    {
    //        return false;
    //    }

    //    if guid.HasValue

    //    if (Guid.TryParse(guid.ToString(), out var _guid))
    //        return _guid != Guid.Empty;
    //    return false;
    //}
}