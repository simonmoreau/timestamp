#region Namespaces
using System;
using System.Reflection;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
#endregion

namespace TimeStamp
{
    class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication a)
        {
            try
            {
                Assembly exe = Assembly.GetExecutingAssembly();
                string dllpath = exe.Location;

                //Create contextual help
                string helpPath = "https://witty-river-01a861010.2.azurestaticapps.net/TimeStamp/TimeStampHelp.html";
                ContextualHelp help = new ContextualHelp(ContextualHelpType.Url, helpPath);

                //Create the panel for the TimeStamp
                RibbonPanel TimeStampPanel = a.CreateRibbonPanel("Time Stamper");

                //Add FSL Button
                PushButtonData timeStampButton = new PushButtonData("TimeStampButton", "Stamp\r\nModel", dllpath, "TimeStamp.ModelTimeStamp");
                timeStampButton.ToolTip = "Add Date and File information on every Revit model object.";

                timeStampButton.LargeImage = RetriveImage("TimeStamp.Resources.TimeStamp_Large.png");
                timeStampButton.Image = RetriveImage("TimeStamp.Resources.TimeStamp_Small.png");

                timeStampButton.SetContextualHelp(help);
                TimeStampPanel.AddItem(timeStampButton);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                // Return Failure
                return Result.Failed;
            }
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }

        private static ImageSource RetriveImage(string imagePath)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(imagePath);

            switch (imagePath.Substring(imagePath.Length - 3))
            {
                case "jpg":
                    var jpgDecoder = new System.Windows.Media.Imaging.JpegBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return jpgDecoder.Frames[0];
                case "bmp":
                    var bmpDecoder = new System.Windows.Media.Imaging.BmpBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return bmpDecoder.Frames[0];
                case "png":
                    var pngDecoder = new System.Windows.Media.Imaging.PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return pngDecoder.Frames[0];
                case "ico":
                    var icoDecoder = new System.Windows.Media.Imaging.IconBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    return icoDecoder.Frames[0];
                default:
                    return null;
            }
        }
    }
}
