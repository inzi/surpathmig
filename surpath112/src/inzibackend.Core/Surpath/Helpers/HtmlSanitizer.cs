using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace inzibackend.Surpath.Helpers
{
    /// <summary>
    /// Helper class for sanitizing HTML content to prevent XSS attacks
    /// </summary>
    public static class HtmlSanitizer
    {
        private static readonly Regex _scriptTagRegex = new Regex(@"<script[^>]*>.*?</script>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _eventAttributesRegex = new Regex(@"\s+(on\w+)=(['""]).*?\2", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _jsHrefRegex = new Regex(@"href\s*=\s*(['""])\s*javascript:.*?\1", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _iframeTagRegex = new Regex(@"<iframe[^>]*>.*?</iframe>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _objectTagRegex = new Regex(@"<object[^>]*>.*?</object>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _embedTagRegex = new Regex(@"<embed[^>]*>.*?</embed>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _formTagRegex = new Regex(@"<form[^>]*>.*?</form>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _metaTagRegex = new Regex(@"<meta[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _baseTagRegex = new Regex(@"<base[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex _dataAttributesRegex = new Regex(@"\s+data-\w+=(['""]).*?\1", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _styleAttributeRegex = new Regex(@"\s+style=(['""]).*?\1", RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static readonly Regex _svgTagRegex = new Regex(@"<svg[^>]*>.*?</svg>", RegexOptions.IgnoreCase | RegexOptions.Singleline);

        // List of allowed HTML tags
        private static readonly HashSet<string> _allowedTags = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "h1", "h2", "h3", "h4", "h5", "h6",
            "p", "div", "span", "br", "hr",
            "ul", "ol", "li", "dl", "dt", "dd",
            "table", "thead", "tbody", "tr", "th", "td",
            "a", "strong", "em", "b", "i", "u", "s", "small",
            "blockquote", "pre", "code", "img"
        };

        // List of allowed attributes
        private static readonly HashSet<string> _allowedAttributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "href", "src", "alt", "title", "class", "id", "name",
            "width", "height", "target", "rel", "type", "colspan", "rowspan"
        };

        /// <summary>
        /// Sanitizes HTML content by removing potentially dangerous elements and attributes
        /// </summary>
        /// <param name="html">The HTML content to sanitize</param>
        /// <returns>Sanitized HTML content</returns>
        public static string SanitizeHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            // Remove script tags and their content
            html = _scriptTagRegex.Replace(html, string.Empty);

            // Remove event attributes (onclick, onload, etc.)
            html = _eventAttributesRegex.Replace(html, string.Empty);

            // Remove javascript: hrefs
            html = _jsHrefRegex.Replace(html, string.Empty);

            // Remove iframe tags and their content
            html = _iframeTagRegex.Replace(html, string.Empty);

            // Remove object tags and their content
            html = _objectTagRegex.Replace(html, string.Empty);

            // Remove embed tags and their content
            html = _embedTagRegex.Replace(html, string.Empty);

            // Remove form tags and their content
            html = _formTagRegex.Replace(html, string.Empty);

            // Remove meta tags
            html = _metaTagRegex.Replace(html, string.Empty);

            // Remove base tags
            html = _baseTagRegex.Replace(html, string.Empty);

            // Remove data attributes
            html = _dataAttributesRegex.Replace(html, string.Empty);

            // Remove style attributes
            html = _styleAttributeRegex.Replace(html, string.Empty);

            // Remove SVG tags and their content
            html = _svgTagRegex.Replace(html, string.Empty);

            // Parse and sanitize HTML using tag and attribute whitelists
            html = SanitizeHtmlTags(html);

            return html;
        }

        /// <summary>
        /// Checks if the provided HTML content contains potentially dangerous elements or attributes
        /// </summary>
        /// <param name="html">The HTML content to check</param>
        /// <returns>True if the HTML is safe, false otherwise</returns>
        public static bool IsHtmlSafe(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return true;
            }

            // Check for script tags
            if (_scriptTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for event attributes
            if (_eventAttributesRegex.IsMatch(html))
            {
                return false;
            }

            // Check for javascript: hrefs
            if (_jsHrefRegex.IsMatch(html))
            {
                return false;
            }

            // Check for iframe tags
            if (_iframeTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for object tags
            if (_objectTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for embed tags
            if (_embedTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for form tags
            if (_formTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for meta tags
            if (_metaTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for base tags
            if (_baseTagRegex.IsMatch(html))
            {
                return false;
            }

            // Check for data attributes
            if (_dataAttributesRegex.IsMatch(html))
            {
                return false;
            }

            // Check for SVG tags
            if (_svgTagRegex.IsMatch(html))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Sanitizes HTML content by parsing and filtering tags and attributes based on whitelists
        /// </summary>
        /// <param name="html">The HTML content to sanitize</param>
        /// <returns>Sanitized HTML content</returns>
        private static string SanitizeHtmlTags(string html)
        {
            // Simple regex-based tag parser
            var tagRegex = new Regex(@"<(/?)([\w:\-]+)([^>]*)>|<!--.*?-->", RegexOptions.Singleline);

            return tagRegex.Replace(html, match =>
            {
                // Skip comments
                if (match.Value.StartsWith("<!--"))
                    return match.Value;

                string tag = match.Groups[2].Value.ToLowerInvariant();
                bool isClosingTag = match.Groups[1].Value == "/";
                string attributes = match.Groups[3].Value;

                // Check if tag is in the allowed list
                if (!_allowedTags.Contains(tag))
                    return string.Empty;

                // For closing tags, just return them as is
                if (isClosingTag)
                    return $"</{tag}>";

                // Process attributes for opening tags
                string sanitizedAttributes = SanitizeAttributes(attributes);
                return $"<{tag}{sanitizedAttributes}>";
            });
        }

        /// <summary>
        /// Sanitizes HTML attributes by filtering based on a whitelist
        /// </summary>
        /// <param name="attributesString">The attributes string to sanitize</param>
        /// <returns>Sanitized attributes string</returns>
        private static string SanitizeAttributes(string attributesString)
        {
            if (string.IsNullOrWhiteSpace(attributesString))
                return string.Empty;

            // Match quoted, unquoted, and valueless attributes
            var attributeRegex = new Regex(
                @"([\w:\-]+)(?:\s*=\s*(?:(['""])(.*?)\2|([^\s""'>]+)))?",
                RegexOptions.Singleline | RegexOptions.Compiled);

            var sanitizedAttributes = new StringBuilder();
            var seenAttributes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (Match match in attributeRegex.Matches(attributesString))
            {
                string name = match.Groups[1].Value.Trim().ToLowerInvariant();
                if (!seenAttributes.Add(name) || !_allowedAttributes.Contains(name))
                    continue;

                string value = match.Groups[3].Success
                    ? match.Groups[3].Value
                    : match.Groups[4].Success
                        ? match.Groups[4].Value
                        : null;

                // Special handling for href/src
                if ((name == "href" || name == "src") && value != null)
                {
                    var val = value.Trim();
                    if (!(val.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                          val.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                          val.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase) ||
                          val.StartsWith("/") ||
                          val.StartsWith("#") ||
                          !val.Contains(":")))
                    {
                        continue;
                    }
                }

                if (value != null)
                {
                    // HTML-encode the value for extra safety
                    string encodedValue = System.Net.WebUtility.HtmlEncode(value);
                    sanitizedAttributes.Append($" {name}=\"{encodedValue}\"");
                }
                else
                {
                    // Valueless attribute (e.g., disabled)
                    sanitizedAttributes.Append($" {name}");
                }
            }

            return sanitizedAttributes.ToString();
        }

        /// <summary>
        /// Validates if a file is of an allowed type for legal documents
        /// </summary>
        /// <param name="fileName">The name of the file to validate</param>
        /// <returns>True if the file type is allowed, false otherwise</returns>
        public static bool IsAllowedFileType(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();

            // Only allow HTML, CSS, and PDF files
            return extension == ".html" || extension == ".htm" || extension == ".css" || extension == ".pdf";
        }
    }
}