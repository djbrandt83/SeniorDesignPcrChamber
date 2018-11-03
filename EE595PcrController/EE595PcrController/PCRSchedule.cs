﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EE595PcrController
{
    class PCRSchedule
    {
        public ScheduleStep Initialization;
        public ScheduleStep Denaturation;
        public ScheduleStep Annealing;
        public ScheduleStep Elongation;
        public ScheduleStep FinalHold;

        public Int32 NumberOfIterations;

        public PCRSchedule()
        {
            Initialization = new ScheduleStep();
            Denaturation = new ScheduleStep();
            Annealing = new ScheduleStep();
            Elongation = new ScheduleStep();
            FinalHold = new ScheduleStep();
        }
    }
}
