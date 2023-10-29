using System.Drawing.Drawing2D;

namespace NotSoBraveBrowser.lib
{
    /**
     * ImageUtil is a class that provides methods to work with images.
     */
    public static class ImageUtil
    {
        /**
         * ResizeImage is a method that resizes the given image to the given width and height
         * in very high quality.
         * It returns the resized image.
         */
        public static Image ResizeImage(Image img, int width, int height)
        {
            Bitmap newImage = new(width, height); // Create a new bitmap with the new width and height

            using (Graphics graphics = Graphics.FromImage(newImage)) // Create a new graphics object from the new image
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality; // Set the smoothing mode to high quality
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic; // Set the interpolation mode to high quality bicubic
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality; // Set the pixel offset mode to high quality
                graphics.DrawImage(img, new Rectangle(0, 0, width, height)); // Draw the image onto the new image
            }

            return newImage; // Return the new image
        }
    }
}