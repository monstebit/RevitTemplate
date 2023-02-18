using Core.Enums;

namespace Core.Results;

public class RevitContextResult
{
    public ResultStatus OperationStatus;
    public string OperationStatusMessage;
    public string OperationMessage;
    public RevitContextResult(ResultStatus operationStatus, string operationMessage)
    {
        OperationStatus = operationStatus;
        OperationMessage = operationMessage;
        switch (OperationStatus)
        {
            case ResultStatus.Complete:
                OperationStatusMessage = "Успешно!!";
                break;
            case ResultStatus.Error:
                OperationStatusMessage = "Ошибка!!";
                break;
        }
    }
}
