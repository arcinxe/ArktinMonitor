using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;

namespace ArktinMonitor.Helpers
{
    public static class TextToSpeechHelper
    {
        private static readonly SpeechSynthesizer Synth = new SpeechSynthesizer();
        private static readonly ReadOnlyCollection<InstalledVoice> Voices = Synth.GetInstalledVoices();
        public static void Speak(string text, string languageCodeOrVoiceName = "en-US")
        {
            // Initialize a new instance of the SpeechSynthesizer.
            var voice = Voices.FirstOrDefault(v => v.VoiceInfo.Name == languageCodeOrVoiceName) ?? Voices.FirstOrDefault(v => v.VoiceInfo.Culture.Name == languageCodeOrVoiceName);

            if (voice != null) Synth.SelectVoice(voice.VoiceInfo.Name);
            // Configure the audio output. 
            Synth.SetOutputToDefaultAudioDevice();
            LocalLogger.Log($"Speaking: {text} using voice: {Synth.Voice.Name}");
            // Speak a string.
            Synth.SpeakAsync(text);
            //try
            //{
            //    TestActions(text);
            //}
            //catch (Exception e)
            //{
            //    LocalLogger.Log(nameof(TestActions), e);
            //}
        }

        public static void VoiceDebug(string data)
        {
            var synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.Rate = 4;
            synth.Speak(data);
        }
      
        public static List<string[]> GetInstalledVoicesList()
        {
            return Voices.Select(v => new[] { v.VoiceInfo.Name, v.VoiceInfo.Culture.Name }).ToList();
        }
    }
}