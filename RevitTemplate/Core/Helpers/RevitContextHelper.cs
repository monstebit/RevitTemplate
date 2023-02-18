using System;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Core.Enums;
using Core.Results;

namespace Core.Helpers
{
    public class RevitContextHelper
    {
        private RevitContextRunnerEventHandler _handler;
        private ExternalEvent _externalEvent;
        private bool _isRunning;
        public RevitContextHelper(UIApplication uapp)
        {
            _handler = new RevitContextRunnerEventHandler();
            _handler.SetUApplication(uapp);
            _handler.TaskDone += OnEventCompleted;
            _externalEvent = ExternalEvent.Create(_handler);
        }
        public void RunTask(Func<RevitContextResult> task)
        {
            _isRunning = true;
            _handler.SetTask(task);
            _externalEvent.Raise();
        }
        private void OnEventCompleted(RevitContextResult result)
        {
            MessageBox.Show(result.OperationMessage, result.OperationStatusMessage);
            _isRunning = false;
        }
        public bool IsTaskRunning()
        {
            return _isRunning;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class RevitContextRunnerEventHandler : IExternalEventHandler
    {
        public delegate void TaskDoneHandler(RevitContextResult result);
        public event TaskDoneHandler TaskDone; 
        
        private Func<RevitContextResult> _func;
        private UIApplication _uiApplication;
        private Exception _exception;
        public void SetTask(Func<RevitContextResult> function)
        {
            _func = function;
        }
        public void SetUApplication(UIApplication uapp)
        {
            _uiApplication = uapp;
        }
        public void Execute(UIApplication app)
        {
            RevitContextResult result;
            try
            {
                result = _func.Invoke();
            }
            catch (Exception exception)
            {
                _exception = exception;
                result = new RevitContextResult(ResultStatus.Error, _exception.Message);
            }
            TaskDone?.Invoke(result);
        }
        public string GetName()
        {
            return "RevitContextRunner";
        }
    }
}



