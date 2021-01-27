using OpenCvSharp;
using System;

namespace FloorSweep.PathFinding
{
    internal static class Segmentation
    {
        public static Mat Bitwise_NOT(this Mat original)
        {
            var ret = new Mat(original.Rows, original.Cols, original.Type());
            Cv2.BitwiseNot(original, ret);
            return ret;
        }

        public static MapData DoSegmentation(string path, double thr1, double thr2, double thr3)
        {
            var mapData = new MapData();
            var img = OpenCvSharp.Cv2.ImRead(path).CvtColor(OpenCvSharp.ColorConversionCodes.RGB2GRAY);
            var se = OpenCvSharp.Cv2.GetStructuringElement(OpenCvSharp.MorphShapes.Ellipse, new OpenCvSharp.Size(4, 4));
            var im_robot = img.Threshold(thr2, 255, OpenCvSharp.ThresholdTypes.Binary);
            var im_target = img.Threshold(thr1, 255, OpenCvSharp.ThresholdTypes.Binary);
            im_target = im_target.Erode(se);
            var im_obst = img.Threshold(thr3, 255, ThresholdTypes.Binary);
            im_obst.MorphologyEx(MorphTypes.Close, se);


            im_obst = (im_obst.Bitwise_NOT() - im_target.Bitwise_NOT()).ToMat().Bitwise_NOT();
            im_target = im_target.Bitwise_NOT();


            var centroids = new Mat();
            var stats = new Mat();
            var label = new Mat();
            var num = Cv2.ConnectedComponentsWithStats(im_robot, label, stats, centroids, PixelConnectivity.Connectivity4);
            if (num > 0)
            {
                mapData.Start = centroids;
            }
            else
            {
                Console.WriteLine("Nie wykryto robota na obrazie");
                return mapData;
            }
            num = Cv2.ConnectedComponentsWithStats(im_target, label, stats, centroids, PixelConnectivity.Connectivity4);

            if (num > 0)
            {
                mapData.Target = centroids;
            }
            else
            {
                Console.WriteLine("Nie wykryto celu ruchu na obrazie");
                return mapData;
            }

            mapData.Map = im_obst;
            return mapData;
        }
    }
}
