using Core.Variables;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System.Net;

namespace MOXA_IO.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        #region Variables
        public IDialogService m_dialogService;
        #endregion Variables

        #region Constructors
        public HomeViewModel(IDialogService dialogService)
        {
            m_dialogService = dialogService;

         
        }
        #endregion Constructors

        #region Methods
       
        #endregion Methods




        #region Properties
        private string _title = "Home";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        #endregion Properties
    }
}
