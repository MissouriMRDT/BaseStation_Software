namespace RED.Addons
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Speech.Synthesis;
    using System.Timers;

    public class SpeechCompositionEngine
    {
        private readonly Queue<string> textToSpeechQueue = new Queue<string>();
        private bool CurrentlySpeaking { get; set; }
        public bool SystemInitialized { get; set; }
        private readonly SpeechSynthesizer synth = new SpeechSynthesizer();

        public SpeechCompositionEngine()
        {
            SystemInitialized = false;

            // Defines event handlers for speech sythesis
            synth.SpeakCompleted += SpeakingCompleted;
            synth.SpeakStarted += SpeakingStarted;
            // Selects a particular voice available on the system - Female/British accent
            var voices = synth.GetInstalledVoices().ToList();
            var x = voices[0];
            synth.SelectVoice(x.VoiceInfo.Name);
            // Configure the audio output.
            synth.SetOutputToDefaultAudioDevice();
            synth.Rate = 1;

            // Polling of queue procession
            var listener = new Timer(100);
            listener.Elapsed += Speak;
            listener.Start();
        }

        private void SpeakingStarted(object sender, SpeakStartedEventArgs e)
        {
            CurrentlySpeaking = true;
        }
        private void SpeakingCompleted(object sender, SpeakCompletedEventArgs e)
        {
            CurrentlySpeaking = false;
        }
        private void Speak(object sender, ElapsedEventArgs e)
        {
            if (Properties.Settings.Default.VoiceMute)
            {
                if(CurrentlySpeaking)
                    synth.SpeakAsyncCancelAll();
                if (textToSpeechQueue.Count > 0)
                    textToSpeechQueue.Clear();
            }

            if (textToSpeechQueue.Count <= 0 || CurrentlySpeaking) return;

            synth.SpeakAsync(textToSpeechQueue.Dequeue());
        }

        public void EnqueueMessage(string text)
        {
            if (Properties.Settings.Default.VoiceMute || !SystemInitialized) return;

            if (!textToSpeechQueue.Contains(text))
                textToSpeechQueue.Enqueue(text);
        }
    }
}
