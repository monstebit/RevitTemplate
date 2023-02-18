using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System;
using Core.Helpers;
using Modules.SecondPluginModule.ViewModel;
using Serilog;
using Modules.SecondPluginModule.View;
using System.IO;
using System.Collections.ObjectModel;


namespace Modules.SecondPluginModule.ExternalCommands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class SecondPluginCommand : IExternalCommand
    {
        private RevitContextHelper _revitTaskHelper;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var module = SecondPluginModule1.GetInstance();
            if (module.IsRunning)
            {
                var msg = "Экземпляр программы уже открыт!";
                var msgTitle = "Ошибка!";

                TaskDialog.Show(msgTitle, msg);

                return Result.Cancelled;
            }

            module.RunControls(commandData);
            _revitTaskHelper = new RevitContextHelper(module.Application);
            InitializeLogger(module.Document);

            try
            {
                var mainViewModel = new SecondPluginViewModel(_revitTaskHelper);
                var mainView = new SecondPluginView(mainViewModel);
                mainView.Show();

                return Result.Succeeded;
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Something wrong");
                TaskDialog.Show($"Ошибка!", exception.Message);
                module.CloseControls();

                return Result.Failed;
            }
            finally
            {
                Log.Information("AmethystRed log closing...");
                Log.CloseAndFlush();
            }
        }

        private static void InitializeLogger(Document document)
        {
            var assemblyPath = Path.GetDirectoryName(typeof(SecondPluginModule1).Assembly.Location);
            var logsPath = Path.Combine(assemblyPath, "Logs");
            var projectName = $"{document.Title}.log";
            var logPath = Path.Combine(logsPath, projectName);

            if (File.Exists(logPath))
            {
                File.Delete(logPath);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(logPath,
                    rollingInterval: RollingInterval.Minute,
                    rollOnFileSizeLimit: true)
                .CreateLogger();

            Log.Information("AmethystRed log running...");
        }
    }
}



//            // Get the application and document
//            UIApplication uiApp = commandData.Application;
//            Document doc = uiApp.ActiveUIDocument.Document;

//            // Get the selected elements
//            ICollection<ElementId> selectedIds = uiApp.ActiveUIDocument.Selection.GetElementIds();

//            // Create a new ElementSet
//            ElementSet selectedElements = new ElementSet();

//            // Add the selected elements to the ElementSet
//            foreach (ElementId id in selectedIds)
//            {
//                Element element = doc.GetElement(id);
//                selectedElements.Insert(element);
//            }

//            try
//            {
//                // Get all the selected elements
//                // Show the form to select the parameter and new value
//                using (ParameterChangerForm form = new ParameterChangerForm(selectedElements))
//                {
//                    if (form.ShowDialog() == DialogResult.OK)
//                    {
//                        using (Transaction trans = new Transaction(doc))
//                        {
//                            trans.Start("Change Parameter");

//                            // Get the selected parameter and new value from the form
//                            string paramName = form.SelectedParameter;
//                            string newValue = form.NewValue;

//                            // Loop through each selected element
//                            foreach (Element elem in selectedElements)
//                            {
//                                // Get the parameter by name
//                                Parameter param = elem.LookupParameter(paramName);

//                                // Check if the parameter is not read-only
//                                if (param != null && !param.IsReadOnly)
//                                {
//                                    // Change the parameter value
//                                    switch (param.StorageType)
//                                    {
//                                        case StorageType.Double:
//                                            param.SetValueString(newValue);
//                                            break;
//                                        case StorageType.Integer:
//                                            int intValue = int.Parse(newValue);
//                                            param.Set(intValue);
//                                            break;
//                                        case StorageType.String:
//                                            param.Set(newValue);
//                                            break;
//                                    }
//                                }
//                            }

//                            trans.Commit();
//                        }
//                    }
//                }

//                return Result.Succeeded;
//            }
//            catch (Exception ex)
//            {
//                message = ex.Message;
//                return Result.Failed;
//            }
//        }
//    }
//}

//public class ParameterChangerForm : System.Windows.Forms.Form
//{
//    private System.Windows.Forms.ComboBox cbParameters;
//    private System.Windows.Forms.TextBox tbNewValue;
//    private System.Windows.Forms.Button btnOK;
//    private System.Windows.Forms.Button btnCancel;

//    private ElementSet elems;

//    public ParameterChangerForm(ElementSet elements)
//    {
//        elems = elements;
//        InitializeComponent();
//    }

//    public string SelectedParameter
//    {
//        get { return cbParameters.SelectedItem.ToString(); }
//    }

//    public string NewValue
//    {
//        get { return tbNewValue.Text; }
//    }

//    private void InitializeComponent()
//    {
//        this.cbParameters = new System.Windows.Forms.ComboBox();
//        this.tbNewValue = new System.Windows.Forms.TextBox();
//        this.btnOK = new System.Windows.Forms.Button();
//        this.btnCancel = new System.Windows.Forms.Button();

//        // cbParameters
//        this.cbParameters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//        this.cbParameters.FormattingEnabled = true;


//        ParameterSet parameters2 = new ParameterSet();

//        // Loop through each selected element
//        foreach (Element elem in elems)
//        {
//            // Get all the parameters of the element
//            ParameterSet parameters = elem.Parameters;

//            // Loop through each parameter
//            foreach (Parameter param in parameters)
//            {
//                // Add the parameter to the combo box if it's not read-only
//                if (!param.IsReadOnly)
//                {
//                    parameters2.Insert(param);
//                }
//            }
//        }

//        ParameterSet uniqueParameters = GetUniqueParameterSet(parameters2);
//        StringBuilder parameterStringBuilder = new StringBuilder();

//        foreach (Parameter param2 in uniqueParameters)
//        {
//            var nameee = param2.Definition.Name;
//            cbParameters.Items.Add(param2.Definition.Name);
//        }

//        // Sort the items in the combo box alphabetically
//        cbParameters.Sorted = true;

//        // tbNewValue
//        this.tbNewValue.Location = new System.Drawing.Point(12, 50);
//        this.tbNewValue.Name = "tbNewValue";
//        this.tbNewValue.Size = new System.Drawing.Size(260, 20);

//        // btnOK
//        this.btnOK.Location = new System.Drawing.Point(116, 76);
//        this.btnOK.Name = "btnOK";
//        this.btnOK.Size = new System.Drawing.Size(75, 23);
//        this.btnOK.Text = "OK";
//        this.btnOK.UseVisualStyleBackColor = true;
//        this.btnOK.Click += new System.EventHandler(this.btnOK_Click);

//        // btnCancel
//        this.btnCancel.Location = new System.Drawing.Point(197, 76);
//        this.btnCancel.Name = "btnCancel";
//        this.btnCancel.Size = new System.Drawing.Size(75, 23);
//        this.btnCancel.Text = "Cancel";
//        this.btnCancel.UseVisualStyleBackColor = true;
//        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

//        // ParameterChangerForm
//        this.Controls.Add(this.btnCancel);
//        this.Controls.Add(this.btnOK);
//        this.Controls.Add(this.tbNewValue);
//        this.Controls.Add(this.cbParameters);
//        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
//        this.MaximizeBox = false;
//        this.MinimizeBox = false;
//        this.Name = "ParameterChangerForm";
//        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
//        this.Text = "Change Parameter Value";
//        this.ResumeLayout(false);
//        this.PerformLayout();
//    }

//    private void btnCancel_Click(object sender, EventArgs e)
//    {
//        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
//        this.Close();
//    }

//    private void btnOK_Click(object sender, EventArgs e)
//    {
//        this.DialogResult = System.Windows.Forms.DialogResult.OK;
//        this.Close();
//    }

//    static ParameterSet GetUniqueParameterSet(ParameterSet originalParameterSet)
//    {
//        ParameterSet uniqueParameterSet = new ParameterSet();
//        HashSet<ElementId> uniqueIds = new HashSet<ElementId>();

//        foreach (Parameter parameter in originalParameterSet)
//        {
//            if (!uniqueIds.Contains(parameter.Id))
//            {
//                uniqueIds.Add(parameter.Id);
//                uniqueParameterSet.Insert(parameter);
//            }
//        }

//        return uniqueParameterSet;
//    }
//}

