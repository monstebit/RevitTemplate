using Autodesk.Revit.DB;
using DigitalServicesTools.Core.Helpers.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers.Core
{
    public class CategoryParametersContainer
    {
        public Category Category { get; set; }
        public List<ParameterMetadata> ParameterMetadata { get; set; }
        public CategoryParametersContainer(Category category, List<ParameterMetadata> metadata)
        {
            Category = category;
            ParameterMetadata = metadata;
        }
    }
}
