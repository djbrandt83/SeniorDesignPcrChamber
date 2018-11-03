using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFTools;

namespace EE595PcrController
{
    class PCRControllerVM : INotifyPropertyChanged
    {
        public PCRSchedule Schedule;
        private RelayCommand _addScheduleStep;

        public PCRControllerVM()
        {
            Schedule = new PCRSchedule();
        }

        #region Binding Properties
        public Int32 InitializationTemp
        {
            get { return Schedule.Initialization.Temperature; }
            set
            {
                if (value != Schedule.Initialization.Temperature)
                {
                    Schedule.Initialization.Temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 InitializationDuration
        {
            get { return Schedule.Initialization.Duration; }
            set
            {
                if (value != Schedule.Initialization.Duration)
                {
                    Schedule.Initialization.Duration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 DenaturationTemp
        {
            get { return Schedule.Denaturation.Temperature; }
            set
            {
                if (value != Schedule.Denaturation.Temperature)
                {
                    Schedule.Denaturation.Temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 DenaturationDuration
        {
            get { return Schedule.Denaturation.Duration; }
            set
            {
                if (value != Schedule.Denaturation.Duration)
                {
                    Schedule.Denaturation.Duration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 AnnealingTemp
        {
            get { return Schedule.Annealing.Temperature; }
            set
            {
                if (value != Schedule.Annealing.Temperature)
                {
                    Schedule.Annealing.Temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 AnnealingDuration
        {
            get { return Schedule.Annealing.Duration; }
            set
            {
                if (value != Schedule.Annealing.Duration)
                {
                    Schedule.Annealing.Duration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 ElongationTemp
        {
            get { return Schedule.Elongation.Temperature; }
            set
            {
                if (value != Schedule.Elongation.Temperature)
                {
                    Schedule.Elongation.Temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 ElongationDuration
        {
            get { return Schedule.Elongation.Duration; }
            set
            {
                if (value != Schedule.Elongation.Duration)
                {
                    Schedule.Elongation.Duration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 FinalHoldTemp
        {
            get { return Schedule.FinalHold.Temperature; }
            set
            {
                if (value != Schedule.FinalHold.Temperature)
                {
                    Schedule.FinalHold.Temperature = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 FinalHoldDuration
        {
            get { return Schedule.FinalHold.Duration; }
            set
            {
                if (value != Schedule.FinalHold.Duration)
                {
                    Schedule.FinalHold.Duration = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 PcrIterations
        {
            get { return Schedule.NumberOfIterations; }
            set
            {
                if (value != Schedule.NumberOfIterations)
                {
                    Schedule.NumberOfIterations = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Commands
        public ICommand AddScheduleStep
        {
            get { return _addScheduleStep; }
        }
        #endregion

        #region INotifyPropertyChangedInterface
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
