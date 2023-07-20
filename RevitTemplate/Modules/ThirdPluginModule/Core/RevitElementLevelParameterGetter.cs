using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.ThirdPluginModule.Core
{
    public static class RevitElementLevelParameterGetter
    {
        public static Parameter GetLevelParameterOfElement(Element element)
        {
            Parameter parameterOfLevel = null;

            if (element is MEPCurve)
            {
                parameterOfLevel = element.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM);
            }

            else if (element is Group)
            {
                parameterOfLevel = element.get_Parameter(BuiltInParameter.GROUP_LEVEL);

            }
            else if (element is CeilingAndFloor)
            {
                parameterOfLevel = element.get_Parameter(BuiltInParameter.LEVEL_PARAM);

            }
            else if (element is Wall)
            {
                parameterOfLevel = element.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT);

            }
            else if (element is FamilyInstance familyInstance)
            {
                Family familyOfElement = familyInstance.Symbol.Family;
                {
                    if (familyOfElement.FamilyPlacementType == FamilyPlacementType.OneLevelBased)
                    {
                        parameterOfLevel = element.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM);
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.OneLevelBasedHosted)
                    {
                        parameterOfLevel = element.get_Parameter(BuiltInParameter.FAMILY_LEVEL_PARAM);
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.TwoLevelsBased)
                    {
                        parameterOfLevel = null;
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.ViewBased)
                    {
                        parameterOfLevel = null;
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.WorkPlaneBased)
                    {
                        parameterOfLevel = element.get_Parameter(BuiltInParameter.INSTANCE_SCHEDULE_ONLY_LEVEL_PARAM);
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.CurveBased)
                    {
                        parameterOfLevel = null;
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.CurveBasedDetail)
                    {
                        parameterOfLevel = null;
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.CurveDrivenStructural)
                    {
                        parameterOfLevel = element.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM);
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.Adaptive)
                    {
                        parameterOfLevel = null;
                    }
                    else if (familyOfElement.FamilyPlacementType == FamilyPlacementType.Invalid)
                    {
                        parameterOfLevel = null;
                    }

                }
            }

            return parameterOfLevel;
        }
    }
}
