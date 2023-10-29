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
            var destRect = new Rectangle(0, 0, width, height); // Create a new rectangle with the new width and height
            var destImage = new Bitmap(width, height); // Create a new bitmap to store the resized image

            // Set the resolution of the new bitmap to the resolution of the original image
            destImage.SetResolution(img.HorizontalResolution, img.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage)) // Create a new graphics object from the new bitmap
            {
                // Set the properties of the graphics object

                // SourceCopy is used to prevent the image from being transparent
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                // Set the quality of the graphics object to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                // Set the interpolation mode of the graphics object to high quality bicubic
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                // Set the smoothing mode of the graphics object to high quality
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                // Set the pixel offset mode of the graphics object to high quality
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using var wrapMode = new System.Drawing.Imaging.ImageAttributes();

                // Set the wrap mode of the graphics object to tile flip XY
                wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);

                // Draw the image onto the new bitmap
                graphics.DrawImage(img, destRect, 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage; // Return the resized image
        }
    }
}