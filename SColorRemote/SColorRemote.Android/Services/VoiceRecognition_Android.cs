
using Android.App;
using Android.Content;
using Android.Speech;
using SColorRemote.Droid;
using SColorRemote.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VoiceRecognition_Android))]
namespace SColorRemote.Droid
{
    public class VoiceRecognition_Android : IVoiceRecognition
    {
        public static AutoResetEvent autoEvent = new AutoResetEvent(false);
        public static string SpeechText;
        public static readonly int VOICE = 10;

        public async Task<string> LaunchRecognition()
        {
            var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Parle-moi ?");
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
            voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

            SpeechText = "";
            autoEvent.Reset();
            ((Activity)Forms.Context).StartActivityForResult(voiceIntent, VOICE);
            await Task.Run(() => { autoEvent.WaitOne(new TimeSpan(0, 2, 0)); });
            return SpeechText;
        }
    }
}