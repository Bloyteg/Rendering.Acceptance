using System.Drawing;
using System.Drawing.Imaging;

namespace Rendering.Acceptance.ImageComparison
{
    public class BitmapComparer : IImageComparer
    {
        public bool Compare(Image firstImage, Image secondImage, byte threshold, out Image diffImage)
        {
            if (firstImage.Width != secondImage.Width || firstImage.Height != secondImage.Height)
            {
                diffImage = null;
                return false;
            }

            bool areSame = true;


            using (var firstBitmap = new Bitmap(firstImage))
            using (var secondBitmap = new Bitmap(secondImage))
            {
                var diffBitmap = firstBitmap.Clone(new Rectangle(0, 0, firstImage.Width, firstImage.Height), PixelFormat.Format24bppRgb);

                unsafe
                {
                    var firstData = firstBitmap.LockBits(new Rectangle(0, 0, firstImage.Width, firstImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    var secondData = secondBitmap.LockBits(new Rectangle(0, 0, secondImage.Width, secondImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                    var diffData = diffBitmap.LockBits(new Rectangle(0, 0, secondImage.Width, secondImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                    var firstPtr = (Pixel*)firstData.Scan0.ToPointer();
                    var secondPtr = (Pixel*)secondData.Scan0.ToPointer();
                    var diffPtr = (Pixel.Difference*)diffData.Scan0.ToPointer();

                    for (int line = 0; line < firstImage.Height; ++line)
                    {
                        for (int column = 0; column < firstImage.Width; ++column)
                        {
                            Pixel firstPixel = *(firstPtr + line * firstImage.Width + column);
                            Pixel secondPixel = *(secondPtr + line * firstImage.Width + column);

                            *(diffPtr + line * firstImage.Width + column) = firstPixel - secondPixel;

                            if (!firstPixel.Equals(secondPixel))
                            {
                                var pixelDifference = firstPixel - secondPixel;

                                if (pixelDifference.RDifference > threshold 
                                    || pixelDifference.GDifference > threshold
                                    || pixelDifference.BDifference > threshold)
                                {
                                    areSame = false;
                                }
                            }
                        }
                    }

                    diffBitmap.UnlockBits(diffData);
                    secondBitmap.UnlockBits(secondData);
                    firstBitmap.UnlockBits(firstData);
                }

                diffImage = diffBitmap;
            }

            return areSame;
        }
    }
}
