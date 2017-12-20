using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using ArktinMonitor.Helpers;

namespace ArktinMonitor.ServiceApp.Services
{
    public static class TextToSpeechHelper
    {
        public static void Speak(string text, string languageCode)
        {
            // Initialize a new instance of the SpeechSynthesizer.
            var synth = new SpeechSynthesizer();
            var voices = synth.GetInstalledVoices();
            var voice = voices.FirstOrDefault(v => v.VoiceInfo.Culture.Name == languageCode);
            if (voice != null) synth.SelectVoice(voice.VoiceInfo.Name);
            // Configure the audio output. 
            synth.SetOutputToDefaultAudioDevice();
            LocalLogger.Log($"Speaking: {text}");
            // Speak a string.
            synth.Speak(text);
        }
    }
}
