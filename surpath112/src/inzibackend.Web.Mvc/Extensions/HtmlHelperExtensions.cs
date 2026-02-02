using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using inzibackend.Surpath.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;

public static class ANZHtmlHelpers
{
    public static string NameForJQ<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
    {
        var expressionBody = (expression as LambdaExpression)?.Body;
        var sb = new StringBuilder();
        GetSerializeJSONName(expressionBody, sb);
        return sb.ToString().TrimStart('[').TrimEnd(']'); // Trim leading and trailing brackets
    }

    private static void GetSerializeJSONName(Expression expression, StringBuilder sb)
    {
        if (expression == null)
            return;

        switch (expression.NodeType)
        {
            case ExpressionType.Parameter:
                // Do nothing for parameter type
                break;

            case ExpressionType.ArrayIndex:
                var arrayIndexExpression = (BinaryExpression)expression;
                GetSerializeJSONName(arrayIndexExpression.Left, sb);
                sb.Append('[');
                GetSerializeJSONName(arrayIndexExpression.Right, sb);
                sb.Append(']');
                break;

            case ExpressionType.Call:
                var methodCallExpression = (MethodCallExpression)expression;
                // Check if the method call is an indexer access, e.g., get_Item
                if (methodCallExpression.Method.Name == "get_Item")
                {
                    GetSerializeJSONName(methodCallExpression.Object, sb);
                    // Append an empty set of brackets to denote the array/collection access
                    sb.Append("[]");
                }
                else
                {
                    // Handle other types of method calls if necessary
                    GetSerializeJSONName(methodCallExpression.Object, sb);
                    sb.Append('.');
                    sb.Append(methodCallExpression.Method.Name);
                }
                break;

            case ExpressionType.MemberAccess:
                var memberExpression = (MemberExpression)expression;
                if (memberExpression.Expression != null) // Check if there's a preceding expression
                {
                    GetSerializeJSONName(memberExpression.Expression, sb);
                    if (sb.Length > 0 && sb[sb.Length - 1] != '[') // Ensure not to append '.' after '['
                    {
                        sb.Append('[');
                    }
                }
                sb.Append(memberExpression.Member.Name);
                if (memberExpression.Expression != null)
                {
                    sb.Append(']');
                }
                break;

            default:
                sb.Append('[');
                sb.Append(expression.ToString());
                sb.Append(']');
                break;
        }
    }

    public static IHtmlContent EnumDropDownListFor<TModel>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, Enum>> expression,
        string dropdownName,
        Type enumType,
        object htmlAttributes = null)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("The provided type must be an enum.");
        }

        // Compiling and executing the expression to get the current value of the enum property in the model
        var func = expression.Compile();
        var model = htmlHelper.ViewData.Model;
        var selectedValue = model != null ? func(model).ToString() : null;

        // Preparing the SelectListItems for the dropdown
        var items = Enum.GetValues(enumType).Cast<Enum>().Select(enumValue => new SelectListItem
        {
            Text = enumValue.GetDescription(),
            Value = enumValue.ToString(),
            Selected = enumValue.ToString() == selectedValue
        }).ToList();

        // Using the provided dropdown name for the 'name' attribute
        return htmlHelper.DropDownList(dropdownName, items, htmlAttributes);
    }


    public static IHtmlContent EnumDropDownList<TEnum>(this IHtmlHelper htmlHelper, string name, TEnum? selectedValue = null, string optionLabel = null, object htmlAttributes = null) where TEnum : struct, IConvertible
    {
        if (!typeof(TEnum).IsEnum)
            throw new ArgumentException("TEnum must be an enumerated type");

        var values = Enum.GetValues(typeof(TEnum)).Cast<Enum>();

        var items = values.Select(enumValue => new SelectListItem
        {
            Text = enumValue.GetDescription(),
            Value = enumValue.ToString(),
            Selected = enumValue.Equals(selectedValue)
        }).ToList();

        if (optionLabel != null)
        {
            items.Insert(0, new SelectListItem { Text = optionLabel, Value = string.Empty });
        }

        return htmlHelper.DropDownList(name, items, htmlAttributes);
    }


    public static IHtmlContent EnumDropDownListForFilter(this IHtmlHelper htmlHelper, string name, Type enumType, object selectedValue = null, string optionLabel = null, object htmlAttributes = null)
    {
        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Provided type must be an enum.");
        }

        var enumValues = Enum.GetValues(enumType).Cast<Enum>();

        var items = enumValues.Select(e => new SelectListItem
        {
            Text = e.GetDescription(),
            Value = Convert.ToInt32(e).ToString(),
            Selected = selectedValue != null && e.Equals(selectedValue)
        }).ToList();

        if (optionLabel != null)
        {
            items.Insert(0, new SelectListItem { Text = optionLabel, Value = string.Empty });
        }

        return htmlHelper.DropDownList(name, items, htmlAttributes);
    }
    private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
    {
        Type realModelType = modelMetadata.ModelType;

        Type underlyingType = Nullable.GetUnderlyingType(realModelType);
        if (underlyingType != null)
        {
            realModelType = underlyingType;
        }

        return realModelType;
    }
}



//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;

//using System;

//using System.Linq.Expressions;

//using System.Text;

//public static class ANZHtmlHelpers

//{

//    public static string NameForJQ<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)

//    {

//        var expressionBody = (expression as LambdaExpression)?.Body;

//        var sb = new StringBuilder();

//        GetSerializeJSONName(expressionBody, sb);

//        return sb.ToString();

//    }


//    private static void GetSerializeJSONName(Expression expression, StringBuilder sb)
//    {

//        if (expression == null)
//            return;

//        switch (expression.NodeType)
//        {

//            case ExpressionType.Parameter:
//                break;

//            case ExpressionType.ArrayIndex:
//                var arrayIndexExpression = (BinaryExpression)expression;
//                GetSerializeJSONName(arrayIndexExpression.Left, sb);
//                sb.Append('[');
//                GetSerializeJSONName(arrayIndexExpression.Right, sb);
//                sb.Append(']');
//                break;

//            case ExpressionType.Call:
//                var methodCallExpression = (MethodCallExpression)expression;
//                GetSerializeJSONName(methodCallExpression.Object, sb);
//                sb.Append('[');
//                sb.Append(methodCallExpression.Method.Name);
//                sb.Append(']');
//                break;

//            case ExpressionType.MemberAccess:
//                var memberExpression = (MemberExpression)expression;
//                GetSerializeJSONName(memberExpression.Expression, sb);
//                sb.Append('[');
//                sb.Append(memberExpression.Member.Name);
//                sb.Append(']');
//                break;

//            default:

//                sb.Append(expression.ToString());
//                break;

//        }

//    }

//}

//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System;
//using System.Linq.Expressions;
//using System.Text;

//public static class ANZHtmlHelpers
//{
//    public static string NameForJQ<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, string propertyName)
//    {
//        var expressionBody = (expression as LambdaExpression)?.Body;
//        var sb = new StringBuilder();
//        GetSerializeJSONName(expressionBody, sb);

//        return sb.ToString();
//    }

//    private static void GetJqueryName(string propertyName, StringBuilder sb)
//    {
//        if (string.IsNullOrEmpty(propertyName))
//            return;

//        var tokens = propertyName.Split(new[] { '.', '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
//        foreach (var token in tokens)
//        {
//            sb.Append('[');
//            sb.Append(token);
//            sb.Append(']');
//        }
//    }
//    private static void GetSerializeJSONName(Expression expression, StringBuilder sb)
//    {
//        if (expression == null)
//            return;

//        switch (expression.NodeType)
//        {
//            case ExpressionType.Parameter:
//                break;
//            case ExpressionType.ArrayIndex:
//                var arrayIndexExpression = (BinaryExpression)expression;
//                GetSerializeJSONName(arrayIndexExpression.Left, sb);
//                sb.Append('[');
//                GetSerializeJSONName(arrayIndexExpression.Right, sb);
//                sb.Append(']');
//                break;
//            case ExpressionType.Call:
//                var methodCallExpression = (MethodCallExpression)expression;
//                GetSerializeJSONName(methodCallExpression.Object, sb);
//                sb.Append('[');
//                sb.Append(methodCallExpression.Method.Name);
//                sb.Append(']');
//                break;
//            case ExpressionType.MemberAccess:
//                var memberExpression = (MemberExpression)expression;
//                GetSerializeJSONName(memberExpression.Expression, sb);
//                sb.Append('[');
//                sb.Append(memberExpression.Member.Name);
//                sb.Append(']');
//                break;
//            default:
//                sb.Append(expression.ToString());
//                break;
//        }
//    }
//}


//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using System.Text.RegularExpressions;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;

//public static class HtmlHelperExtensions
//{
//    private static readonly ModelExpressionProvider ExpressionHelper = new ModelExpressionProvider(new EmptyModelMetadataProvider());

//    public static string ANZNameForNoModel<TModel, TValue>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression)
//    {
//        var expressionString = ExpressionHelper.GetExpressionText(expression);

//        // Step 1: Replace numeric indices with empty brackets []
//        expressionString = Regex.Replace(expressionString, @"\[\d*\]", "[]");

//        // Step 2: Extract the property path without the model prefix
//        int firstDotIndex = expressionString.IndexOf('.');
//        if (firstDotIndex >= 0)
//        {
//            expressionString = expressionString.Substring(firstDotIndex + 1);
//        }

//        // Step 3: Replace the last property access (.) with the bracket notation for properties
//        int lastDotIndex = expressionString.LastIndexOf('.');
//        if (lastDotIndex >= 0)
//        {
//            expressionString = expressionString.Substring(0, lastDotIndex) +
//                               "[" + expressionString.Substring(lastDotIndex + 1) + "]";
//        }

//        return expressionString;
//    }

//    public static string ANZNameForx<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
//    {
//        //var regex = new Regex(@"\[(.*?)\]");
//        //string path = ModelExpressionProvider.GetExpressionText(expression);

//        //// Replace numerical indexes with empty brackets to match the desired format
//        //path = regex.Replace(path, "[]");

//        //return path;
//        var expressionString = ExpressionHelper.GetExpressionText(expression);

//        // Using Regex to replace numeric indices with empty brackets []
//        var pattern = new Regex(@"\[\d+\]");
//        var replacement = "[]";
//        expressionString = pattern.Replace(expressionString, replacement);

//        // Ensure the last segment, if a property name, does not get enclosed in quotes
//        // Directly appending property name within brackets without quotes
//        expressionString = Regex.Replace(expressionString, @"\.(.*?)$", "[$1]");

//        return expressionString;
//    }

//    public static string ANZNameFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
//    {
//        var names = new StringBuilder();
//        BuildExpressionPath(expression.Body, names);
//        return names.ToString().TrimEnd('.');
//    }

//    private static void BuildExpressionPath(Expression expression, StringBuilder names)
//    {
//        switch (expression)
//        {
//            case MemberExpression memberExpression:
//                BuildExpressionPath(memberExpression.Expression, names);
//                names.Append($"{memberExpression.Member.Name}.");
//                break;
//            case MethodCallExpression methodCallExpression when methodCallExpression.Method.Name == "get_Item":
//                BuildExpressionPath(methodCallExpression.Arguments[0], names);
//                if (methodCallExpression.Arguments.Count > 0 && methodCallExpression.Arguments[0] is ConstantExpression indexExpr)
//                {
//                    names.Append($"[{indexExpr.Value}].");
//                }
//                break;
//        }
//    }
//}
