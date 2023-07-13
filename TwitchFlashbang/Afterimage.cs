using System.Diagnostics;
using System.Drawing.Imaging;

namespace TwitchFlashbang
{
    public partial class Afterimage : Form
    {
        internal float saturation = 1.2f;
        internal float brightness = 1.1f;

        public Afterimage()
        {
            InitializeComponent();

            Debug.WriteLine($"Handle for Afterimage: {Handle:x}");

            WinAPI.CreateMagicWindow(this);
        }

        public void SetImage(Bitmap image)
        {
            pictureBox1.Image = image;
        }

        public Image GetImage()
        {
            return pictureBox1.Image;
        }

        public void SetOpacity(byte opacity)
        {
            BeginInvoke(() =>
            {
                WinAPI.SetLayeredWindowAttributes(Handle, 0, opacity, 0x2);
                WinAPI.UpdateWindow(Handle);
            });
        }

        internal void SetSize(Size s)
        {
            Size = s;
        }

        internal void SetLocation(Point l)
        {
            Location = l;
        }

        internal Bitmap ApplyColorMatrix(Bitmap bmp)
        {
            // Create a new bitmap with the same dimensions as the original
            Bitmap resultBitmap = new(bmp.Width, bmp.Height);

            // Create a graphics object from the result bitmap
            using (Graphics graphics = Graphics.FromImage(resultBitmap))
            {
                // Create an image attributes object for color transformation
                using (ImageAttributes imageAttributes = new())
                {
                    ColorMatrix colorMatrix = new(
                        new float[][]
                        {
                            new float[] { 2f, 0f, 0f, 0f, 0f },             // Red component
                            new float[] { 0f, 2f, 0f, 0f, 0f },             // Green component
                            new float[] { 0f, 0f, 2f, 0f, 0f },             // Blue component
                            new float[] { 0f, 0f, 0f, 0.2f, 0f },           // Alpha component
                            new float[] { -0.1f, -0.1f, -0.1f, 0f, 1f }     // Brightness adjustment
                        });

                    // Set the color matrix to the image attributes
                    imageAttributes.SetColorMatrix(colorMatrix);

                    // Draw the original bitmap onto the result bitmap with the color transformation
                    graphics.DrawImage(bmp, new Rectangle(0, 0, resultBitmap.Width, resultBitmap.Height), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttributes);
                }
            }

            return resultBitmap;
        }
    }
}