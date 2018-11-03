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
        public ScheduleSerializer Serializer;
        private RelayCommand _savePcrSchedule;
        private RelayCommand _savePcrScheduleAs;
        private RelayCommand _loadPcrSchedule;

        public PCRDevice Device;

        public PCRControllerVM()
        {
            Schedule = new PCRSchedule();
            Serializer = new ScheduleSerializer();
            Serializer.OnNewScheduleLoaded += Serializer_OnNewScheduleLoaded;
            _savePcrSchedule = new RelayCommand( param => Serializer.SaveSchedule(Schedule) );
            _savePcrScheduleAs = new RelayCommand( param => Serializer.SaveScheduleAs(Schedule) );
            _loadPcrSchedule = new RelayCommand( param => Serializer.LoadSchedule() );

            Device = new PCRDevice();
        }

        private void Serializer_OnNewScheduleLoaded(object sender, EventArgs e)
        {
            if(sender is ScheduleSerializer)
            {
                Serializer = (ScheduleSerializer)sender;
                PCRSchedule schedule = Serializer.LoadedSchedule;
                InitializationTemp = schedule.Initialization.Temperature;
                InitializationDuration = schedule.Initialization.Duration;
                DenaturationTemp = schedule.Denaturation.Temperature;
                DenaturationDuration = schedule.Denaturation.Duration;
                AnnealingTemp = schedule.Annealing.Temperature;
                AnnealingDuration = schedule.Annealing.Duration;
                ElongationTemp = schedule.Elongation.Temperature;
                ElongationDuration = schedule.Elongation.Duration;
                FinalHoldTemp = schedule.FinalHold.Temperature;
                FinalHoldDuration = schedule.FinalHold.Duration;
                PcrIterations = schedule.NumberOfIterations;
            }
            else
            {
                throw new Exception();
            }
        }

        #region Device Connection Binding Properties
        public String ConnectButtonMessage
        {
            get
            {
                if (Device.DeviceOnline)
                {
                    return "Disconnect";
                }
                else
                {
                    return "Connect";
                }
            }
        }

        public String ConnectionStatus
        {
            get
            {
                if (Device.DeviceOnline)
                {
                    return "Device connected";
                }
                else
                {
                    return "Device Offline";
                }
            }
        }
        #endregion

        #region Schedule Binding Properties
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
        public ICommand SavePcrSchedule
        {
            get { return _savePcrSchedule; }
        }

        public ICommand SavePcrScheduleAs
        {
            get { return _savePcrScheduleAs; }
        }

        public ICommand LoadPcrSchedule
        {
            get { return _loadPcrSchedule; }
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
