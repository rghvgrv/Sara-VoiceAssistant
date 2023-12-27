using System;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Sara = new SpeechSynthesizer();
        SpeechRecognitionEngine startListening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice(); //Use Headphone for clear voice 
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startListening.SetInputToDefaultAudioDevice();
            startListening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startListening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startListening_SpeechRecognized);
        }
        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            if(speech == "Hello")
            {
                Sara.SpeakAsync("Hi , I am Sara");
            }
            if (speech == "How are you")
            {
                Sara.SpeakAsync("I am fine");
            }
            if (speech == "What is the Time")
            {
                Sara.SpeakAsync(DateTime.Now.ToString("h mm ss t"));
            }
            if(speech =="Open Command Prompt")
            {
                Sara.SpeakAsync("Opening command prompt");
                Process.Start("cmd");
            }
            if(speech == "Stop talking")
            {
                Sara.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1,2);
                if(ranNum == 1)
                {
                    Sara.SpeakAsync("Yes sir");
                }
                if(ranNum == 2)
                {
                    Sara.SpeakAsync("I am sorry i will be quiet");
                }
            }
            if(speech == "Stop Listening")
            {
                Sara.SpeakAsync("If you need me just ask");
                _recognizer.RecognizeAsyncCancel();
                startListening.RecognizeAsync(RecognizeMode.Multiple);
            }
            if(speech == "Show commands")
            {
                string[] commands = (File.ReadAllLines(@"DefaultCommands.txt"));
                LstCommands.Items.Clear();
                LstCommands.SelectionMode = SelectionMode.None;
                LstCommands.Visible = true;
                foreach(string command in commands)
                {
                    LstCommands.Items.Add(command);
                }
            }
            if(speech == "Hide commands")
            {
                LstCommands.Visible = false;
            }
        }
        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;

        }
        private void startListening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            if(speech == "Wake up")
            {
                startListening.RecognizeAsyncCancel();
                Sara.SpeakAsync("Yes I am here");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }
        private void tmrSpeaking_Tick(object sender, EventArgs e)
        {
            if(RecTimeOut == 10)
            {
                _recognizer.RecognizeAsyncCancel();
            }
            else if(RecTimeOut == 11)
            {
                tmrSpeaking.Stop();
                startListening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut = 0;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void LstCommands_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
