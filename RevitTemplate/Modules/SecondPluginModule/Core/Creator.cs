using Core.Enums;
using Core.Results;
using Modules.SecondPluginModule.ViewModel;

namespace Modules.SecondPluginModule.Core
{
    public class Creator
    {
        private SecondPluginViewModel _ownViewModel;

        public Creator(SecondPluginViewModel ownViewModel)
        {
            _ownViewModel = ownViewModel;
        }

        public RevitContextResult Delete()
        {
            return new RevitContextResult(ResultStatus.Complete,
                "Элемент удален из модели");
        }
    }
}
