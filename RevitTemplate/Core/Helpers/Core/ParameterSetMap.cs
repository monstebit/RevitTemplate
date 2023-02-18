namespace DigitalServicesTools.Core.Helpers.Core;

public class ParameterSetMap
{
    public ParameterMetadata ParameterFrom;
    public ParameterMetadata ParameterTo;

    public ParameterSetMap(ParameterMetadata parameterFrom, ParameterMetadata parameterTo)
    {
        ParameterFrom = parameterFrom;
        ParameterTo = parameterTo;
    }
}