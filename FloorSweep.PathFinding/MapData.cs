
using System.Drawing;
using System.Drawing.Imaging;
using FloorSweep.Math;

namespace FloorSweep.PathFinding
{
    public class MapData
    {
        private MapData(Mat originalImage, Mat binaryMap, int scaling = 1)
        {
            OriginalImage = originalImage;
            Scaling = scaling;
            LoadMap(binaryMap, scaling);
        }

        public static MapData FromImage(string path, int scaling = 1)
        {
            var img = new Bitmap(Image.FromFile(path));
            var mat = Mat.ImageToGrayScale(img);
            return new MapData(mat, mat.BinaryTresh(126), scaling);
        }

        public static MapData Empty(int scaling = 1)
        {
            var original = Mat.Ones(600, 600);
            return new MapData(original, original, scaling);
        }

        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }


        private void LoadMap(Mat image, int scaling)
        {
            var map = image;
            if (scaling != 1)
            {
                map = SimplifyMap.DoSimplifyMap(map, scaling);
            }
            Map = Mat.Zeros(BorderThickness, map.Cols);
            Map.VConcat(map);
            Map.VConcat(Mat.Zeros(BorderThickness, map.Cols));
            var m = Mat.Zeros(Map.Rows, BorderThickness);
            m.HConcat(Map);
            m.HConcat(Mat.Zeros(Map.Rows, BorderThickness));
            Map = m;
        }

        public Mat Map { get; private set; }
        public Mat OriginalImage { get; private set; }
        public int Scaling { get; }
        public int BorderThickness { get; } = 5;
    }
}
