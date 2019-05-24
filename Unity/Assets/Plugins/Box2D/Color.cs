//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月24日 23:50:24
//------------------------------------------------------------

namespace System.Drawing
{
    public class Color
    {
        public UnityEngine.Color color { get; }

        public byte A { get; }

        public byte R { get; }

        public byte G { get; }

        public byte B { get; }

        public static Color FromArgb(int a, int r, int g, int b)
        {
            return new Color(a, r, g, b);
        }

        public static Color FromArgb(int r, int g, int b)
        {
            return new Color(255, r, g, b);
        }

        private Color(int a, int r, int g, int b)
        {
            this.A = (byte) a;
            this.R = (byte) r;
            this.G = (byte) g;
            this.B = (byte) b;
            this.color = new UnityEngine.Color(R / 255f, G / 255f, B / 255f, A / 255f);
        }
    }
}