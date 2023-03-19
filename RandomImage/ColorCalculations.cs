using System;
using System.Drawing;

namespace RandomImage
{
    public class ColorCalculations
    {
        public static Color GetRandom(Color color, Random random)
        {
            int r, g, b;
            r = Avrage(color.R, random.Next(0, 255));
            g = Avrage(color.G, random.Next(0, 255));
            b = Avrage(color.B, random.Next(0, 255));
            color = Color.FromArgb(r, g, b);
            return color;
        }
        public static int Avrage(int num1, int num2)
        {
            int sum = num1 + num2;
            sum /= 2;
            return sum;
        }
        public static Color AvrageColor(Color color1, Color color2)
        {
            int r, g, b;
            r = Avrage(color1.R, color2.R);
            g = Avrage(color1.G, color2.R);
            b = Avrage(color1.B, color2.R);
            return Color.FromArgb(r, g, b);
        }
        public static Color ColorError(Random random)
        {
            if (random.Next(0, 100) > 49)
            {
                return Color.Pink;
            }
            return Color.Black;
        }
        public static Color GetColorGraySpace(Color color, Random random)
        {
            int r, g, b;
            int steps = 10;
            int randome = (int)(random.Next(0, steps) * (255 / steps));
            r = Avrage(color.R, randome);
            g = Avrage(color.G, randome);
            b = Avrage(color.B, randome);
            color = Color.FromArgb(r, g, b);
            return color;
        }
    }
}
