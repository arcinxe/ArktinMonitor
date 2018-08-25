using AudioSwitcher.AudioApi.CoreAudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArktinMonitor.Helpers
{
    public static class VolumeChanger
    {
        private static CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;

        public static int Volume
        {
            get { return (int)defaultPlaybackDevice.Volume; }
            set
            {
                if (value > 100 || value < 0) return;
                defaultPlaybackDevice.Volume = value;
            }
        }
    }
}
