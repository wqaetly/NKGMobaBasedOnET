using System;

namespace Box2DSharp.Common
{
    public struct Color
    {
        public byte R => unchecked((byte)(Value >> ARGBRedShift));

        public byte G => unchecked((byte)(Value >> ARGBGreenShift));

        public byte B => unchecked((byte)(Value >> ARGBBlueShift));

        public byte A => unchecked((byte)(Value >> ARGBAlphaShift));

        public long Value { get; }

        private const int ARGBAlphaShift = 24;

        private const int ARGBRedShift = 16;

        private const int ARGBGreenShift = 8;

        private const int ARGBBlueShift = 0;

        public static Color Blue { get; } = new Color(0xFF0000FF);

        public static Color Green { get; } = new Color(0xFF00FF00);

        public static Color Red { get; } = new Color(0xFFFF0000);

        public static Color Yellow { get; } = new Color(0xFFFFFF00);

        private Color(long value)
        {
            Value = value;
        }

        private static void CheckByte(int value, string name)
        {
            if (unchecked((uint)value) > byte.MaxValue)
            {
                throw new ArgumentException(
                    $"Value of '{value}' is not valid for '{name}'. '{name}' should be greater than or equal to {byte.MinValue} and less than or equal to {byte.MaxValue}.");
            }
        }

        public static Color FromArgb(int alpha, int red, int green, int blue)
        {
            CheckByte(alpha, nameof(alpha));
            CheckByte(red, nameof(red));
            CheckByte(green, nameof(green));
            CheckByte(blue, nameof(blue));

            return FromArgb(
                (uint)alpha << ARGBAlphaShift | (uint)red << ARGBRedShift | (uint)green << ARGBGreenShift | (uint)blue << ARGBBlueShift
            );
        }

        public static Color FromArgb(int r, int g, int b)
        {
            return FromArgb(255, r, g, b);
        }

        public static Color FromArgb(float a, float r, float g, float b)
        {
            return FromArgb((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public static Color FromArgb(float r, float g, float b)
        {
            return FromArgb(255, (int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public static Color FromArgb(int argb)
        {
            return FromArgb(unchecked((uint)argb));
        }

        private static Color FromArgb(uint argb) => new Color(argb);
    }
}