using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EE595PcrController
{
    class ScheduleSerializer
    {
        public String FileName;
        public String LastFilePath;
        public PCRSchedule LoadedSchedule;

        public event EventHandler OnNewScheduleLoaded;

        public ScheduleSerializer()
        {
        }

        public void SaveSchedule(PCRSchedule schedule)
        {
            if (this.FileName == String.Empty)
            {
                SaveScheduleAs(schedule);
            }
            else
            {
                String scheduleToSave = JsonConvert.SerializeObject(schedule);
                File.WriteAllText(this.FileName, scheduleToSave);
            }
        }

        public void SaveScheduleAs(PCRSchedule schedule)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "PcrExperiment"; // Default file name
            dlg.DefaultExt = ".json"; // Default file extension
            dlg.Filter = "Text documents (.json)|*.json"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                this.FileName = dlg.FileName;
                this.LastFilePath = Path.GetFileName(this.FileName);
                String scheduleToSave = JsonConvert.SerializeObject(schedule);
                File.WriteAllText(this.FileName, scheduleToSave);
            }
            //Else user probably hit cancel
        }

        public void LoadSchedule()
        {
            Microsoft.Win32.FileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "json file (*.json)|*.json";
            if(this.FileName != String.Empty)
            {
                dlg.InitialDirectory = this.FileName;
            }
            Nullable<bool> result = dlg.ShowDialog();

            // Process load file dialog box results
            if (result == true)
            {
                this.FileName = dlg.FileName;
                this.LastFilePath = Path.GetFileName(this.FileName);
                //Load document
                String json = File.ReadAllText(this.FileName);
                this.LoadedSchedule = JsonConvert.DeserializeObject<PCRSchedule>(json);
                OnNewScheduleLoaded(this, EventArgs.Empty);
                this.LoadedSchedule = null;
            }
            //Else user probably hit cancel
        }
    }
}
