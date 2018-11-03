using System;
using System.Diagnostics;

namespace EE595PcrController
{
    class ExperimentStatus
    {
        private static readonly String Initalization = "Initialization";
        private static readonly String Denaturization = "Denaturization";
        private static readonly String Annealing = "Annealing";
        private static readonly String Elongation = "Elongation";
        private static readonly String FinalHold = "Final Hold";

        private Stopwatch experimentStopwatch;

        public Boolean ExperimentRunning;
        public String CurrentStep;
        public Int32 CyclesCompleted;
        public Int32 CyclesRemaining;

        public Int32 PCRMinutes => (Int32)this.experimentStopwatch.Elapsed.TotalMinutes;
        public Int32 PCRSeconds => (Int32)this.experimentStopwatch.Elapsed.Seconds;

        public ExperimentStatus()
        {
            ExperimentRunning = false;
            CurrentStep = Initalization;
            CyclesCompleted = 0;
            experimentStopwatch = new Stopwatch();
        }
    }
}
