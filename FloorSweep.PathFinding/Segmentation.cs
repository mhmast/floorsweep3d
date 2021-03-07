using FloorSweep.Math;
using System;

namespace FloorSweep.PathFinding
{
    internal static class Segmentation
    {
        public static OpenCvSharp.Mat Bitwise_NOT(this OpenCvSharp.Mat original)
        {
            var ret = new OpenCvSharp.Mat(rows: original.Rows, original.Cols, OpenCvSharp.MatType.CV_64FC1);
            OpenCvSharp.Cv2.BitwiseNot(original, ret);
            return ret;
        }

        public static MapData DoSegmentation(string path, double thr1, double thr2, double thr3)
        {
            var mapData = new MapData();
            var img = OpenCvSharp.Cv2.ImRead(path, OpenCvSharp.ImreadModes.Grayscale);//.CvtColor(OpenCvSharp.ColorConversionCodes.RGB2GRAY);
            mapData.Image = Mat.FromCV(img);
            var se = OpenCvSharp.Cv2.GetStructuringElement(OpenCvSharp.MorphShapes.Ellipse, new OpenCvSharp.Size(9, 9));
            var im_robot = img.Threshold(255 * thr2, 255, OpenCvSharp.ThresholdTypes.Binary);
            var im_target = img.Threshold(255 * thr1, 255, OpenCvSharp.ThresholdTypes.Binary);
            im_target = im_target.Erode(se);
            var im_obst = img.Threshold(255 * thr3, 255, OpenCvSharp.ThresholdTypes.Binary);
            im_obst.MorphologyEx(OpenCvSharp.MorphTypes.Close, se);


            im_obst = (im_obst.Bitwise_NOT() - im_target.Bitwise_NOT()).ToMat().Bitwise_NOT();
            im_target = im_target.Bitwise_NOT();


            var centroids = new OpenCvSharp.Mat();
            var stats = new OpenCvSharp.Mat();
            var label = new OpenCvSharp.Mat();
            var num = OpenCvSharp.Cv2.ConnectedComponentsWithStats(im_robot, label, stats, centroids, OpenCvSharp.PixelConnectivity.Connectivity4);
            if (num > 0)
            {
                mapData.Start = new Point((int)centroids.At<double>(1,0), (int)centroids.At<double>(1, 1));
            }
            else
            {
                Console.WriteLine("Nie wykryto robota na obrazie");
                return mapData;
            }
            centroids = new OpenCvSharp.Mat();
            num = OpenCvSharp.Cv2.ConnectedComponentsWithStats(im_target, label, stats, centroids, OpenCvSharp.PixelConnectivity.Connectivity4);

            if (num > 0)
            {
                mapData.Target = new Point((int)centroids.At<double>(1, 0), (int)centroids.At<double>(1, 1));
            }
            else
            {
                Console.WriteLine("Nie wykryto celu ruchu na obrazie");
                return mapData;
            }

            mapData.Map = Mat.FromCV(im_obst);
            return mapData;
        }
    }
}
