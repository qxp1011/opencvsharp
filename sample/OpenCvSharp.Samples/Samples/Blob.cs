﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenCvSharp;
using OpenCvSharp.Blob; // for blob

namespace OpenCvSharpSamples
{
    /// <summary>
    /// c# implementation of cvblob/test/test.cpp.
    /// cvblob : http://code.google.com/p/cvblob/
    /// </summary>
    internal class Blob
    {
        public Blob()
        {
            using (IplImage imgSrc = new IplImage(Const.ImageShapes, LoadMode.Color))
            using (IplImage imgBinary = new IplImage(imgSrc.Size, BitDepth.U8, 1))
            using (IplImage imgLabel = new IplImage(imgSrc.Size, BitDepth.F32, 1))
            using (IplImage imgRender = new IplImage(imgSrc.Size, BitDepth.U8, 3))
            using (IplImage imgContour = new IplImage(imgSrc.Size, BitDepth.U8, 3))
            using (IplImage imgPolygon = new IplImage(imgSrc.Size, BitDepth.U8, 3))
            {
                Cv.CvtColor(imgSrc, imgBinary, ColorConversion.BgrToGray);
                Cv.Threshold(imgBinary, imgBinary, 100, 255, ThresholdType.Binary);

                CvBlobs blobs = new CvBlobs();
                blobs.Label(imgBinary);

                foreach (KeyValuePair<int, CvBlob> item in blobs)
                {
                    CvBlob b = item.Value;
                    Console.WriteLine("{0} | Centroid:{1} Area:{2}", item.Key, b.Centroid, b.Area);

                    CvContourChainCode cc = b.Contour;
                    cc.Render(imgContour);

                    CvContourPolygon polygon = cc.ConvertToPolygon();
                    foreach (CvPoint p in polygon)
                    {
                        imgPolygon.Circle(p, 1, CvColor.Red, -1);
                    }
                }

                blobs.RenderBlobs(imgSrc, imgRender);

                using (new CvWindow("render", imgRender))
                using (new CvWindow("contour", imgContour))
                using (new CvWindow("polygon vertices", imgPolygon))
                {
                    Cv.WaitKey(0);
                }
            }
        }
    }
}
