﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
 
namespace NumSharp.Core
{
	public static partial class np
	{
        public static NDArray array(Array array, Type dtype = null, int ndim = 1)
        {
            dtype = (dtype == null) ? array.GetType().GetElementType() : dtype;
            
			var nd = new NDArray(dtype);

            if ((array.Rank == 1) && ( !array.GetType().GetElementType().IsArray ))
			{
                nd.Storage = new NDStorage(dtype);
                nd.Storage.Allocate(dtype, new Shape(new int[] { array.Length }),1);

                nd.Storage.SetData(array); 
            }
            else 
            {
                throw new Exception("Method is not implemeneted for multidimensional arrays or jagged arrays.");
            }
            
            return nd;
        }

        public static NDArray array(System.Drawing.Bitmap image)
        {
            var imageArray = new NDArray(typeof(Byte));

            var bmpd = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, image.PixelFormat);
            var dataSize = bmpd.Stride * bmpd.Height;

            var bytes = new byte[dataSize];
            System.Runtime.InteropServices.Marshal.Copy(bmpd.Scan0, bytes, 0, dataSize);
            image.UnlockBits(bmpd);

            imageArray.Storage.Allocate(typeof(byte),new Shape(bmpd.Height, bmpd.Width, System.Drawing.Image.GetPixelFormatSize(image.PixelFormat) / 8),1);
            imageArray.Storage.SetData(bytes);
            
            return imageArray;
        }

        public static NDArray array<T>(T[][] data)
        {
            var nd = new NDArray(typeof(T),data.Length,data[0].Length);
            
            for (int row = 0; row < data.Length; row++)
            {
                for (int col = 0; col < data[row].Length; col++)
                {
                    nd[row,col] = data[row][col];
                }
            }

            return nd;
        }

        public static NDArray array<T>(params T[] data)
        {
            var nd = new NDArray(typeof(T), data.Length);

            nd.Storage.SetData<T>(data);

            return nd;
        }
    }
}
