﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Traicy.GUI.Logic
{
    /// <summary>
    /// Provides methods for using the webcam and converting media.
    /// </summary>
    public class WebcamHelper
    {
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        /// <summary>
        /// Parses given webcam bitmap to ImageSource 
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        public void TakePicture(Bitmap picture)
        {
            string filename = $"{DateTime.Now:dd_MM_yy hh_mm_ss}.png";
            //TODO: create directory if not exists
            picture.Save(@"../../images/" + filename, ImageFormat.Png);
            //TODO: Image an Python übergeben oder so lassen, dass Python die Bilder über Ordnerstruktur einliest?
        }
    }
}
