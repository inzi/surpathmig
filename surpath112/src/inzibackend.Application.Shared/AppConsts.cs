using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace inzibackend
{
    /// <summary>
    /// Some consts used in the application.
    /// </summary>
    public class AppConsts
    {
        /// <summary>
        /// Default page size for paged requests.
        /// </summary>
        public const int DefaultPageSize = 500;

        /// <summary>
        /// Maximum allowed page size for paged requests.
        /// </summary>
        public const int MaxPageSize = 1000;

        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public const string DefaultPassPhrase = "48b719d91a6d477cbc203364403ae418";

        public const int ResizedMaxProfilePictureBytesUserFriendlyValue = 1024;

        public const int MaxProfilePictureBytesUserFriendlyValue = 5;

        public const string TokenValidityKey = "token_validity_key";
        public const string RefreshTokenValidityKey = "refresh_token_validity_key";
        public const string SecurityStampKey = "AspNet.Identity.SecurityStamp";

        public const string TokenType = "token_type";

        public static string UserIdentifier = "user_identifier";

        public const string ThemeDefault = "default";
        public const string Theme0 = "theme0";
        public const string Theme2 = "theme2";
        public const string Theme3 = "theme3";
        public const string Theme4 = "theme4";
        public const string Theme5 = "theme5";
        public const string Theme6 = "theme6";
        public const string Theme7 = "theme7";
        public const string Theme8 = "theme8";
        public const string Theme9 = "theme9";
        public const string Theme10 = "theme10";
        public const string Theme11 = "theme11";
        public const string Theme12 = "theme12";
        public const string Theme13 = "theme13";

        public static TimeSpan AccessTokenExpiration = TimeSpan.FromDays(1);
        public static TimeSpan RefreshTokenExpiration = TimeSpan.FromDays(365);

        public const string DateTimeOffsetFormat = "yyyy-MM-ddTHH:mm:sszzz";

        /// <summary>
        /// Maximum allowed rows for Excel export operations to prevent memory exhaustion and timeouts.
        /// </summary>
        public const int MaxExportRows = 10000;

        /// <summary>
        /// Maximum length for filter string inputs to prevent SQL injection and performance issues.
        /// </summary>
        public const int MaxFilterLength = 500;

        public static readonly KeyValuePair<string, string>[] USStates = {
            new KeyValuePair<string, string>("Alabama", "AL"),
            new KeyValuePair<string, string>("Alaska", "AK"),
            new KeyValuePair<string, string>("Arizona", "AZ"),
            new KeyValuePair<string, string>("Arkansas", "AR"),
            new KeyValuePair<string, string>("California", "CA"),
            new KeyValuePair<string, string>("Colorado", "CO"),
            new KeyValuePair<string, string>("Connecticut", "CT"),
            new KeyValuePair<string, string>("District of Columbia", "DC"),
            new KeyValuePair<string, string>("Delaware", "DE"),
            new KeyValuePair<string, string>("Florida", "FL"),
            new KeyValuePair<string, string>("Georgia", "GA"),
            new KeyValuePair<string, string>("Hawaii", "HI"),
            new KeyValuePair<string, string>("Idaho", "ID"),
            new KeyValuePair<string, string>("Illinois", "IL"),
            new KeyValuePair<string, string>("Indiana", "IN"),
            new KeyValuePair<string, string>("Iowa", "IA"),
            new KeyValuePair<string, string>("Kansas", "KS"),
            new KeyValuePair<string, string>("Kentucky", "KY"),
            new KeyValuePair<string, string>("Louisiana", "LA"),
            new KeyValuePair<string, string>("Maine", "ME"),
            new KeyValuePair<string, string>("Maryland", "MD"),
            new KeyValuePair<string, string>("Massachusetts", "MA"),
            new KeyValuePair<string, string>("Michigan", "MI"),
            new KeyValuePair<string, string>("Minnesota", "MN"),
            new KeyValuePair<string, string>("Mississippi", "MS"),
            new KeyValuePair<string, string>("Missouri", "MO"),
            new KeyValuePair<string, string>("Montana", "MT"),
            new KeyValuePair<string, string>("Nebraska", "NE"),
            new KeyValuePair<string, string>("Nevada", "NV"),
            new KeyValuePair<string, string>("New Hampshire", "NH"),
            new KeyValuePair<string, string>("New Jersey", "NJ"),
            new KeyValuePair<string, string>("New Mexico", "NM"),
            new KeyValuePair<string, string>("New York", "NY"),
            new KeyValuePair<string, string>("North Carolina", "NC"),
            new KeyValuePair<string, string>("North Dakota", "ND"),
            new KeyValuePair<string, string>("Ohio", "OH"),
            new KeyValuePair<string, string>("Oklahoma", "OK"),
            new KeyValuePair<string, string>("Oregon", "OR"),
            new KeyValuePair<string, string>("Pennsylvania", "PA"),
            new KeyValuePair<string, string>("Rhode Island", "RI"),
            new KeyValuePair<string, string>("South Carolina", "SC"),
            new KeyValuePair<string, string>("South Dakota", "SD"),
            new KeyValuePair<string, string>("Tennessee", "TN"),
            new KeyValuePair<string, string>("Texas", "TX"),
            new KeyValuePair<string, string>("Utah", "UT"),
            new KeyValuePair<string, string>("Vermont", "VT"),
            new KeyValuePair<string, string>("Virginia", "VA"),
            new KeyValuePair<string, string>("Washington", "WA"),
            new KeyValuePair<string, string>("West Virginia", "WV"),
            new KeyValuePair<string, string>("Wisconsin", "WI"),
            new KeyValuePair<string, string>("Wyoming", "WY")
        };

       
    }
}
