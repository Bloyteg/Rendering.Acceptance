using System.Drawing;

namespace Rendering.Acceptance.ImageComparison
{
    public interface IImageComparer
    {
        bool Compare(Image firstImage, Image secondImage, byte threshold, out Image diffImage);
    }
}