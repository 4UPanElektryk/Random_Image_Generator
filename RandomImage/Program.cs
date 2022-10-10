using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using CoolConsole;
using CoolConsole.MenuItemTemplate;

namespace RandomImage
{
	internal class Program
	{
		static int height, width, seed;
		static void Main(string[] args)
		{
			bool testmode = false;
			int selected = 5;
			Console.WriteLine("Random image Creator V3");
			Console.WriteLine("Press any key to continue");
			if (!testmode)
			{
				Console.ReadKey(true);
				List<MenuItem> menuItems = new List<MenuItem>
				{
					new TextboxMenuItem("Seed of Image",""),
					new TextboxMenuItem("Width",""),
					new TextboxMenuItem("Height",""),
					new MenuItem("Fully Random"),
					new MenuItem("Avraged Random"),
					new MenuItem("Sequential Avraged Random"),
					new MenuItem("Perlin Noise")
				};
				ReturnCode returnCode = Menu.Show(menuItems);
				seed = int.Parse(returnCode.Textboxes[0]._Text);
				width = int.Parse(returnCode.Textboxes[1]._Text);
				height = int.Parse(returnCode.Textboxes[2]._Text);
				selected = returnCode.SelectedMenuItem;
			}
			else
			{
				seed = 2; width = 920; height = 1080;
			}
			if (selected == 3)
			{
				CreateRandomImage();
			}
			if (selected == 4)
			{
				CreateRandomImageWithLines();
			}
			if (selected == 5)
			{
				CreateRandomImageWithScanLines();
			}
            if (selected == 6)
            {
                CreateRandomPerlinNoise();
            }
        }

		static void CreateRandomImageWithLines()
		{
			Random random = new Random(seed);
			Bitmap bitmap = new Bitmap(width, height);
			Color lastcolor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
			for (int x = 0; x < bitmap.Height; x++)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					bitmap.SetPixel(y, x, lastcolor);
					lastcolor = GetRandom(lastcolor, random);
				}
			}
			Image image = bitmap;
			image.Save("image.png", ImageFormat.Png);
		}
		static void CreateRandomImage()
		{
			Random random = new Random(seed);
			Bitmap bitmap = new Bitmap(width, height);
			for (int x = 0; x < bitmap.Height; x++)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					bitmap.SetPixel(y, x, Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
				}
			}
			Image image = bitmap;
			image.Save("image.png", ImageFormat.Png);
		}
		static void CreateRandomImageWithScanLines()
		{
			Random random = new Random(seed);
			Bitmap bitmap = new Bitmap(width, height);
			Color lastcolor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
			#region Initial Color Sampling
			for (int x = 0; x < bitmap.Height; x += 2)
			{
				for (int y = 0; y < bitmap.Width; y += 2)
				{
					bitmap.SetPixel(y, x, lastcolor);
					lastcolor = GetRandom(lastcolor, random);
				}
			}
			#endregion
			#region Horizontal Sampling
			for (int x = 0; x < bitmap.Height; x += 2)
			{
				for (int y = 1; y < bitmap.Width; y += 2)
				{
					Color left = bitmap.GetPixel(y - 1, x);
					Color right = ColorError(random);
					if ((y + 1) < bitmap.Width)
					{
						right = bitmap.GetPixel(y + 1, x);
					}
					bitmap.SetPixel(y, x, AvrageColor(left, right));
					lastcolor = GetRandom(lastcolor, random);
				}
			}
			#endregion
			#region Vertical Sampling
			for (int x = 1; x < bitmap.Height; x += 2)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					Color left = bitmap.GetPixel(y, x - 1);
					Color right = ColorError(random);
					if ((x + 1) < bitmap.Height)
					{
						right = bitmap.GetPixel(y, x + 1);
					}
					bitmap.SetPixel(y, x, AvrageColor(left, right));
					lastcolor = GetRandom(lastcolor, random);
				}
			}
			#endregion
			Image image = bitmap;
			image.Save("image.png", ImageFormat.Png);
		}
		static void CreateRandomPerlinNoise()
		{
            Random random = new Random(seed);
            Bitmap bitmap = new Bitmap(width, height);
			Color lastcolor = Color.Black;
            for (int x = 0; x < bitmap.Height; x++)
            {
                for (int y = 0; y < bitmap.Width; y++)
                {
					lastcolor = GetColorGraySpace(lastcolor, random);
                    bitmap.SetPixel(y, x, lastcolor);
                }
            }
            Image image = bitmap;
            image.Save("image.png", ImageFormat.Png);
        }
		#region Aditional Math
		static Color GetRandom(Color color, Random random)
		{
			int r, g, b;
			r = Avrage(color.R, random.Next(0, 255));
			g = Avrage(color.G, random.Next(0, 255));
			b = Avrage(color.B, random.Next(0, 255));
			color = Color.FromArgb(r,g,b);
			return color;
		}
		static int Avrage(int num1,int num2)
		{
			int sum = num1 + num2;
			sum = sum / 2;
			return sum;
		}
		static Color AvrageColor(Color color1, Color color2)
		{
			int r, g, b;
			r = Avrage(color1.R, color2.R);
			g = Avrage(color1.G, color2.R);
			b = Avrage(color1.B, color2.R);
			return Color.FromArgb(r, g, b);
		}
		static Color ColorError(Random random)
		{
			if (random.Next(0,100) > 49)
			{
				return Color.Pink;
			}
			return Color.Black;
		}
		static Color GetColorGraySpace(Color color, Random random)
		{
            int r, g, b;
			int randome = (int)(random.Next(0, 10) * 25.5);
            r = Avrage(color.R, randome);
            g = Avrage(color.G, randome);
            b = Avrage(color.B, randome);
            color = Color.FromArgb(r, g, b);
            return color;
        }
		#endregion
	}
}
