using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using CoolConsole;
using CoolConsole.MenuItems;
using CoolConsole.Aditonal;
using System.Security.Policy;

namespace RandomImage
{
	internal class Program
	{
		static int height, width, seed;
		static void Main(string[] args)
		{
			bool testmode = false;
			int selected = 5;
			Console.WriteLine("Random image Creator V4");
			Console.WriteLine("Press any key to continue");
			if (!testmode)
			{
				Console.ReadKey(true);
				List<MenuItem> menuItems = new List<MenuItem>
				{
					new NumboxMenuItem("Seed of Image",0),
					new NumboxMenuItem("Width",512),
					new NumboxMenuItem("Height",512),
					new MenuItem("Fully Random"),
					new MenuItem("Avraged Random"),
					new MenuItem("Sequential Avraged Random"),
					new MenuItem("Perlin Noise")
				};
				ReturnCode returnCode = Menu.Show(menuItems);
				seed = returnCode.Numboxes[0];
				width = returnCode.Numboxes[1];
				height = returnCode.Numboxes[2];
				selected = returnCode.SelectedMenuItem;
			}
			else
			{
				seed = 2137; width = 1920; height = 1080;
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
			int goal = bitmap.Height * bitmap.Width;
			Color lastcolor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
			for (int x = 0; x < bitmap.Height; x++)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					bitmap.SetPixel(y, x, lastcolor);
					lastcolor = ColorCalculations.GetRandom(lastcolor, random);
					int progress = (x * bitmap.Width) + y;
					ProgressReporter(progress, goal, "Generating...");
				}
			}
			Image image = bitmap;
			image.Save("image.png", ImageFormat.Png);
		}
		static void CreateRandomImage()
		{
			Random random = new Random(seed);
			Bitmap bitmap = new Bitmap(width, height);
			int goal = bitmap.Height * bitmap.Width;
			for (int x = 0; x < bitmap.Height; x++)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					bitmap.SetPixel(y, x, Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
					int progress = (x * bitmap.Width) + y;
					ProgressReporter(progress, goal, "Generating...");
				}
			}
			Image image = bitmap;
			image.Save("image.png", ImageFormat.Png);
		}
		static void CreateRandomImageWithScanLines()
		{
			Random random = new Random(seed);
			Bitmap bitmap = new Bitmap(width, height);
			int goal = bitmap.Height * bitmap.Width;
			Color lastcolor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
			#region Initial Color Sampling
			for (int x = 0; x < bitmap.Height; x += 2)
			{
				for (int y = 0; y < bitmap.Width; y += 2)
				{
					bitmap.SetPixel(y, x, lastcolor);
					lastcolor = ColorCalculations.GetRandom(lastcolor, random);
					int progress = (x * bitmap.Width) + y;
					ProgressReporter(progress, goal, "Initial Color Sampling...");
				}
			}
			#endregion
			lastproc = -1;
			#region Horizontal Sampling
			for (int x = 0; x < bitmap.Height; x += 2)
			{
				for (int y = 1; y < bitmap.Width; y += 2)
				{
					Color left = bitmap.GetPixel(y - 1, x);
					Color right = ColorCalculations.ColorError(random);
					if ((y + 1) < bitmap.Width)
					{
						right = bitmap.GetPixel(y + 1, x);
					}
					bitmap.SetPixel(y, x, ColorCalculations.AvrageColor(left, right));
					lastcolor = ColorCalculations.GetRandom(lastcolor, random);
					int progress = (x * bitmap.Width) + y;
					ProgressReporter(progress, goal, "Horizontal Sampling...");
				}
			}
			#endregion
			lastproc = -1;
			#region Vertical Sampling
			for (int x = 1; x < bitmap.Height; x += 2)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					Color left = bitmap.GetPixel(y, x - 1);
					Color right = ColorCalculations.ColorError(random);
					if ((x + 1) < bitmap.Height)
					{
						right = bitmap.GetPixel(y, x + 1);
					}
					bitmap.SetPixel(y, x, ColorCalculations.AvrageColor(left, right));
					lastcolor = ColorCalculations.GetRandom(lastcolor, random);
					int progress = (x * bitmap.Width) + y;
					ProgressReporter(progress, goal, "Vertical Sampling...");
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
			int goal = bitmap.Height * bitmap.Width;
			Color lastcolor = Color.Black;
			for (int x = 0; x < bitmap.Height; x++)
			{
				for (int y = 0; y < bitmap.Width; y++)
				{
					lastcolor = ColorCalculations.GetColorGraySpace(lastcolor, random);
					bitmap.SetPixel(y, x, lastcolor);
					int progress = (x * bitmap.Width) + y;
					ProgressReporter(progress, goal, "Generating...");
				}
			}
			Image image = bitmap;
			image.Save("image.png", ImageFormat.Png);
		}
		static int lastproc = -1;
		static void ProgressReporter(int value, int max, string msg)
		{
			int proc = (value * 100) / max;
			if (lastproc == -1)
			{
				Console.Clear();
			}
			if (proc > lastproc)
			{
				Console.CursorLeft = 0;
				Console.CursorTop = 0;
				Console.WriteLine(msg);
				ProgressBar.Show(value, max, 30,true);
				lastproc = proc;
			}
		}
	}
}
