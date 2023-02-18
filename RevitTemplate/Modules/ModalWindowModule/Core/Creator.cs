using Core.Enums;
using Core.Results;
using Modules.ModalWindowModule.ViewModel;

namespace Modules.ModalWindowModule.Core
{
    public class Creator
    {
        private ModalWindowViewModel _ownViewModel;
        public Creator(ModalWindowViewModel ownViewModel)
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
