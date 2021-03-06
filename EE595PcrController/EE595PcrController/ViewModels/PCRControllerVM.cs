﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using WPFTools;

namespace EE595PcrController
{
    class PCRControllerVM : INotifyPropertyChanged
    {
        public PCRSchedule Schedule;
        public ScheduleSerializer Serializer;
        public List<String> ComPortDevices;
        public Boolean ComPortSelected;
        public String ConnectButtonText;
        public String ConnectStatusText;
        private RelayCommand _savePcrSchedule;
        private RelayCommand _savePcrScheduleAs;
        private RelayCommand _loadPcrSchedule;
        private RelayCommand _startStopExperiment;
        private RelayCommand _connectToComPort;

        public PCRDevice Device;
        public ExperimentStatus Experiment;

        public PCRControllerVM()
        {
            Schedule = new PCRSchedule();
            Serializer = new ScheduleSerializer();
            Serializer.OnNewScheduleLoaded += Serializer_OnNewScheduleLoaded;
            _savePcrSchedule = new RelayCommand(param => Serializer.SaveSchedule(Schedule));
            _savePcrScheduleAs = new RelayCommand(param => Serializer.SaveScheduleAs(Schedule));
            _loadPcrSchedule = new RelayCommand(param => Serializer.LoadSchedule());
            _startStopExperiment = new RelayCommand(param => Device.StartStopExperiment(Experiment.ExperimentRunning));
            _connectToComPort = new RelayCommand(param => Device.ConnectToDevice());

            Device = new PCRDevice();
            Device.OnDeviceConnected += Device_OnDeviceConnected;
            Device.OnDeviceDisconnected += Device_OnDeviceDisconnected;
            Device.OnExperimentStarted += Device_OnExperimentStarted;
            Device.OnExperimentTerminated += Device_OnExperimentTerminated;
            Experiment = new ExperimentStatus();
            ComPortDevices = SerialPort.GetPortNames().ToList<String>();
            ComPortSelected = false;
            ConnectButtonText = "Connect";
            ConnectionStatus = Device.ConnectionMessage;
        }

        private void Device_OnExperimentTerminated(object sender, EventArgs e)
        {
            Experiment.ExperimentRunning = false;
            StartStopMessage = GetStartStopMessage();
        }

        private void Device_OnExperimentStarted(object sender, EventArgs e)
        {
            Experiment.ExperimentRunning = true;
            StartStopMessage = GetStartStopMessage();
        }

        private void Device_OnDeviceDisconnected(object sender, EventArgs e)
        {
            if (sender is PCRDevice)
            {
                ConnectButtonMessage = "Connect";
                ConnectionStatus = Device.ConnectionMessage;
                DeviceOnline = false;
            }
        }

        private void Device_OnDeviceConnected(object sender, EventArgs e)
        {
            if(sender is PCRDevice)
            {
                ConnectButtonMessage = "Disconnect";
                ConnectionStatus = Device.ConnectionMessage;
                DeviceOnline = true;
            }
        }

        private void Serializer_OnNewScheduleLoaded(object sender, EventArgs e)
        {
            if (sender is ScheduleSerializer)
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
        public List<String> ComPortList
        {
            get { return ComPortDevices; }
        }

        public String SelectedDevice
        {
            get { return Device.DeviceName; }
            set
            {
                if(value != Device.DeviceName)
                {
                    Device.DeviceName = value;

                    if(SelectedDevice != String.Empty)
                    {
                        DeviceSelected = true;
                    }

                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean DeviceOnline
        {
            get { return Device.DeviceOnline; }
            set
            {
                if (value != Device.DeviceOnline)
                {
                    Device.DeviceOnline = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean DeviceSelected
        {
            get { return ComPortSelected; }
            set
            {
                if (value != ComPortSelected)
                {
                    ComPortSelected = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Experiment Status Binding Properties
        public String CurrentStepName
        {
            get { return Experiment.CurrentStep; }
            set
            {
                if (value != Experiment.CurrentStep)
                {
                    Experiment.CurrentStep = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 CyclesCompleted
        {
            get { return Experiment.CyclesCompleted; }
            set
            {
                if (value != Experiment.CyclesCompleted)
                {
                    Experiment.CyclesCompleted = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 CyclesRemaining
        {
            get { return Experiment.CyclesRemaining; }
            set
            {
                if (value != Experiment.CyclesRemaining)
                {
                    Experiment.CyclesRemaining = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Int32 ExperimentMinutes => Experiment.PCRMinutes;

        public Int32 ExperimentSeconds => Experiment.PCRSeconds;

        public Int32 CurrentTemperature => Device.OutputTemperature;

        public Int32 TargetTemperature => Device.ReferenceTemperature;

        public String StartStopMessage
        {
            get { return GetStartStopMessage(); }
            set => NotifyPropertyChanged();
        }

        public string GetStartStopMessage()
        {
            if (Experiment.ExperimentRunning)
            {
                return "Stop PCR Experiment";
            }
            else
            {
                return "Start PCR Experiment";
            }
        }
        #endregion

        #region Device Connection Binding Properties
        public String ConnectButtonMessage
        {
            get{ return ConnectButtonText; }

            set
            {
                if (value != ConnectButtonText)
                {
                    ConnectButtonText = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public String ConnectionStatus
        {
            get { return Device.ConnectionMessage; }

            set => NotifyPropertyChanged();
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

        public ICommand ConnectToComPort
        {
            get { return _connectToComPort; }
        }

        public ICommand StartStopExperiment
        {
            get { return _startStopExperiment; }
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
