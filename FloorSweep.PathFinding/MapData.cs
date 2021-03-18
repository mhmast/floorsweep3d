using Point = FloorSweep.Math.Point;
using System.Drawing;
using System.Drawing.Imaging;
using FloorSweep.Math;

namespace FloorSweep.PathFinding
{
    public class MapData
    {
        private MapData(Mat originalImage,Mat binaryMap, Point start, Point target, int scaling = 1)
        {
            Start = start;
            Target = target;
            OriginalImage = originalImage;
            Scaling = scaling;
            LoadMap(binaryMap, start, target, scaling);
        }

        public static MapData FromImage(string path, Point start, Point target, int scaling = 1)
        {
            var img = new Bitmap(Image.FromFile(path));
            var mat = Mat.ImageToGrayScale(img);
            return new MapData(mat,mat.BinaryTresh(126), start, target, scaling);
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


        private void LoadMap(Mat image, Point start, Point target, int scaling)
        {
            var map = image;
            if (scaling != 1)
            {
                map = SimplifyMap.DoSimplifyMap(map, scaling);
            }
            Map = Mat.Zeros(5, map.Cols);
            Map.VConcat(map);
            Map.VConcat(Mat.Zeros(6, map.Cols));
            var m = Mat.Zeros(Map.Rows, 5);
            m.HConcat(Map);
            m.HConcat(Mat.Zeros(Map.Rows, 6));
            Map = m;
            Start = start / scaling + 5;
            Target = target / scaling + 5;
        }

        public Mat Map { get; set; }
        public Point Start { get; set; }
        public Point Target { get; set; }

        public Mat OriginalImage { get; set; }
        public int Scaling { get; }
    }
}
