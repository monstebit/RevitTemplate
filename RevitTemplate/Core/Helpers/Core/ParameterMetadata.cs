using Autodesk.Revit.DB;

namespace DigitalServicesTools.Core.Helpers.Core;

public class ParameterMetadata
{
    public Category Category { get; }
    public ScheduleFieldType ScheduleFieldType { get; }
    public ParameterUsingType ParameterUsingType { get; }
    public Autodesk.Revit.DB.ElementId Id { get; }
    public string ParameterName { get; }

    public ParameterMetadata(Category category, ScheduleFieldType scheduleFieldType, Autodesk.Revit.DB.ElementId elementId, string parameterName)
    {
        Category = category;
        ScheduleFieldType = scheduleFieldType;
        Id = elementId;
        ParameterName = parameterName;
        if (elementId.IntegerValue < 0) ParameterUsingType = ParameterUsingType.System;
        if (elementId.IntegerValue > 0) ParameterUsingType = ParameterUsingType.NoSystem;
    }
}
