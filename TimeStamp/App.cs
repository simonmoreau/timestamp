#region Namespaces
using System;
using System.Reflection;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.IO;
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

                string helpPath = Path.Combine(Path.GetDirectoryName(dllpath), "TimeStampHelp.html");
                ContextualHelp help = new ContextualHelp(ContextualHelpType.ChmFile, helpPath);

                //Create the panel for the TimeStamp
                RibbonPanel TimeStampPanel = a.CreateRibbonPanel("Time Stamper");

                //Add FSL Button
                PushButtonData timeStampButton = new PushButtonData("TimeStampButton", "Stamp\r\nModel", dllpath, "TimeStamp.ModelTimeStamp");
                timeStampButton.ToolTip = "Add Date and File information on every Revit model object.";
                timeStampButton.LargeImage = Tools.GetEmbeddedImage("TimeStamp.Resources.TimeStamp_Large.png");
                timeStampButton.Image = Tools.GetEmbeddedImage("TimeStamp.Resources.TimeStamp_Small.png");
                timeStampButton.SetContextualHelp(help);
                TimeStampPanel.AddItem(timeStampButton);

                return Result.Succeeded;
            }
            catch
            {
                // Return Failure
                return Result.Failed;
            }
            
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            return Result.Succeeded;
        }
    }
}
