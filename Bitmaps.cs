using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBClient
{
    public static class Bitmaps
    {
        public static float[,] GetBrightness(string imageFile, int height, int width)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(imageFile);
            float[,] br = new float[height, width];
            for (int i = 0; i < bmp.Height; i++)
                for (int j = 0; j < bmp.Width; j++)
                {
                    int ii = i * height / bmp.Height;
                    int jj = j * width / bmp.Width;
                    br[ii, jj] += bmp.GetPixel(j, i).GetBrightness();
                }
            float mianownik = bmp.Height/height * bmp.Width/width;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    br[i, j] /= mianownik;

            return br;
        }
    }
}
