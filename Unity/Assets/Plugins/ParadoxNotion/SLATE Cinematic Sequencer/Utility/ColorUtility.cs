using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Slate
{

    public static class ColorUtility
    {

        ///A greyscale color
        public static Color Grey(float value) {
            return new Color(value, value, value);
        }

        ///The color, with alpha
        public static Color WithAlpha(this Color color, float alpha) {
            color.a = alpha;
            return color;
        }
    }
}