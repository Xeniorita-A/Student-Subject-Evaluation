using Student_Subject_Evaluation.Core;
using System;

namespace Student_Subject_Evaluation.MVVM.View_Model
{
    internal class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand AccountViewCommand { get; set; }
        public RelayCommand CurriculumViewCommand { get; set; }
        public RelayCommand EvaluationViewCommand { get; set; }
        public RelayCommand StudentViewCommand { get; set; }
        public RelayCommand ActivityLogViewCommand { get; set; }

        public HomeViewModel HomeVm { get; set; }
        public AccountViewModel AccountVm { get; set; }
        public CurriculumViewModel CurriculumVm { get; set; }
        public EvaluationViewModel EvaluationVm { get; set; }
        public StudentViewModel StudentVm { get; set; }
        public ActivityLogViewModel ActivityLogVm { get; set; }

        private object _currentView;

        public Object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }

        }
        public MainViewModel()
        {
            HomeVm = new HomeViewModel();
            AccountVm = new AccountViewModel();
            CurriculumVm = new CurriculumViewModel();
            EvaluationVm = new EvaluationViewModel();
            StudentVm = new StudentViewModel();
            ActivityLogVm = new ActivityLogViewModel();
            CurrentView = HomeVm;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVm;
            });

            AccountViewCommand = new RelayCommand(o =>
            {
                CurrentView = AccountVm;
            });

            CurriculumViewCommand = new RelayCommand(o =>
            {
                CurrentView = CurriculumVm;
            });

            EvaluationViewCommand = new RelayCommand(o =>
            {
                CurrentView = EvaluationVm;
            });

            StudentViewCommand = new RelayCommand(o =>
            {
                CurrentView = StudentVm;
            });
            ActivityLogViewCommand = new RelayCommand(o =>
            {
                CurrentView = ActivityLogVm;
            });
        }
    }
}
