#region Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.ApplicationServices;
using System.IO;
#endregion

namespace TimeStamp
{
    [Transaction(TransactionMode.Manual)]
    class ModelTimeStamp : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument UIdoc = commandData.Application.ActiveUIDocument;
            Application app = commandData.Application.Application;
            Document doc = UIdoc.Document;

            using (Transaction tx = new Transaction(doc))
            {
                try
                {
                    //Create a list of category
                    CategorySet categories = CreateCategoryList(doc, app);

                    //Retrive the coresponding list of elements
                    IList<Element> ElementsList = GetElementList(doc, categories);

                    //Load the interface
                    PrepareModelInterface properties = new PrepareModelInterface(doc, ElementsList);

                    if (properties.ShowDialog() == true)
                    {
                        tx.Start("Model TimeStamp");

                        //Create Shared parameters if necessary
                        AddSharedParameters(app, doc, categories);

                        //Apply these values to every elements
                        ApplyValuesOnElements(properties);

                        tx.Commit();

                        // Return Success
                        return Result.Succeeded;
                    }
                    else
                    {
                        return Autodesk.Revit.UI.Result.Cancelled;
                    }
                }

                catch (Autodesk.Revit.Exceptions.OperationCanceledException exceptionCanceled)
                {
                    message = exceptionCanceled.Message;
                    if (tx.HasStarted() == true)
                    {
                        tx.RollBack();
                    }
                    return Autodesk.Revit.UI.Result.Cancelled;
                }
                catch (ErrorMessageException errorEx)
                {
                    // checked exception need to show in error messagebox
                    message = errorEx.Message;
                    if (tx.HasStarted() == true)
                    {
                        tx.RollBack();
                    }
                    return Autodesk.Revit.UI.Result.Failed;
                }
                catch (Exception ex)
                {
                    // unchecked exception cause command failed
                    message = ex.Message;
                    //Trace.WriteLine(ex.ToString());
                    if (tx.HasStarted() == true)
                    {
                        tx.RollBack();
                    }
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }
        }

        private IList<Element> GetElementList(Document doc, CategorySet categories)
        {
            //Retrive all model elements
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<ElementFilter> categoryFilters = new List<ElementFilter>();

            foreach (Category category in categories)
            {
                categoryFilters.Add(new ElementCategoryFilter(category.Id));
            }

            ElementFilter filter = new LogicalOrFilter(categoryFilters);

            return collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

        }

        private void ApplyValuesOnElements(PrepareModelInterface properties)
        {
            //Add the value to all element
            if (properties.ElementList.Count > 0)
            {
                foreach (Element e in properties.ElementList)
                {
                    WriteOnParam("BIM42_Date", e, properties.Modeldate);
                    WriteOnParam("BIM42_Version", e, properties.ModelIndice);
                    WriteOnParam("BIM42_File", e, properties.ModelName);
                    WriteOnParam("BIM42_Discipline", e, properties.ModelLot);
                }
            }
        }

        private void WriteOnParam(string paramId, Element e, string value)
        {
            IList<Parameter> parameters = e.GetParameters(paramId);
            if (parameters.Count != 0)
            {
                Parameter p = parameters.FirstOrDefault();
                if (!p.IsReadOnly)
                {
                    p.Set(value);
                }
            }
        }

        private void AddSharedParameters(Application app, Document doc, CategorySet myCategorySet)
        {
            //Save the previous shared param file path
            string previousSharedParam = app.SharedParametersFilename;

            //Extract shared param to a txt file
            string tempPath = System.IO.Path.GetTempPath();
            string SPPath = Path.Combine(tempPath, "FileProperties.txt");

            if (!File.Exists(SPPath))
            {
                //extract the familly
                List<string> files = new List<string>();
                files.Add("FileProperties.txt");
                Tools.ExtractEmbeddedResource(tempPath, "TimeStamp.Resources", files);
            }

            //set the shared param file
            app.SharedParametersFilename = SPPath;

            //Retrive shared parameters
            DefinitionFile myDefinitionFile = app.OpenSharedParameterFile();

            DefinitionGroup definitionGroup = myDefinitionFile.Groups.get_Item("FileProperties");

            foreach (Definition paramDef in definitionGroup.Definitions)
            {
                //Create an instance of InstanceBinding
                InstanceBinding instanceBinding = app.Create.NewInstanceBinding(myCategorySet);

                // Get the BingdingMap of current document.
                BindingMap bindingMap = doc.ParameterBindings;

                if (!bindingMap.Contains(paramDef))
                {
                    bindingMap.Insert(paramDef, instanceBinding, BuiltInParameterGroup.PG_IDENTITY_DATA);
                }
            }

            //Reset to the previous shared parameters text file
            app.SharedParametersFilename = previousSharedParam;
            File.Delete(SPPath);

        }

        private CategorySet CreateCategoryList(Document doc, Application app)
        {
            CategorySet myCategorySet = app.Create.NewCategorySet();
            Categories categories = doc.Settings.Categories;

            foreach (Category c in categories)
            {
                if (c.AllowsBoundParameters && c.CategoryType == CategoryType.Model)
                {
                    myCategorySet.Insert(c);
                }
            }

            return myCategorySet;
        }


    }
}


