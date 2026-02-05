using System;
using System.Text;
using System.Security.Cryptography;

namespace inzibackend.Surpath.Statics;

public static class SlotIdGenerator
{
    //public static string Generate(int length)
    //{
    //    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Uppercase letters and digits
    //    StringBuilder stringBuilder = new StringBuilder(length);

    //    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
    //    {
    //        byte[] data = new byte[4]; // Buffer to hold the random bytes

    //        for (int i = 0; i < length; i++)
    //        {
    //            rng.GetBytes(data); // Fill the buffer with random bytes
    //            int value = BitConverter.ToInt32(data, 0); // Convert bytes to an int
    //            int randomIndex = Math.Abs(value) % chars.Length; // Use abs to ensure positive index, and mod to wrap index around chars length

    //            stringBuilder.Append(chars[randomIndex]);
    //        }
    //    }

    //    return stringBuilder.ToString();
    //}
    public static string Generate(int length, string preface = "")
    {
        const string chars = "ABCDEFGHJKMNOPQRSTUVWXYZ123456789"; // Uppercase letters and digits
        preface = preface.ToUpper();
        // Ensure preface is not longer than 2 characters. If it is, truncate it.
        preface = preface.Length > 2 ? preface.Substring(0, 2) : preface;

        StringBuilder stringBuilder = new StringBuilder(preface);

        using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
        {
            byte[] data = new byte[4]; // Buffer to hold the random bytes
            int randomPartLength = length - preface.Length; // Adjust the length for the random part

            for (int i = 0; i < randomPartLength; i++)
            {
                rng.GetBytes(data); // Fill the buffer with random bytes
                int value = BitConverter.ToInt32(data, 0); // Convert bytes to an int
                int randomIndex = Math.Abs(value) % chars.Length; // Ensure index is positive, and mod to wrap index

                stringBuilder.Append(chars[randomIndex]);
            }
        }

        return stringBuilder.ToString();
    }
}

