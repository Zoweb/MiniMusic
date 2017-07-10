using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using MME.Console;
using MME.Data;
using MME.Drawing;
using MME.Sound;
using MME.Timers;

namespace MME.Storage
{
    public static class Piano
    {
        private static Timer _playerTimer = new Timer();

        public static float Bpm = 120;
        public static float PlayResolution = 8;

        public static void PlaySoundForTime(string instrumentName, int soundIndex, float amp, float timeMs)
        {
            var voice = SongData.PlayInstrumentNote(instrumentName, soundIndex, amp);
            var timer = new Timer(timeMs);
            timer.Elapsed += sender =>
            {
                SongData.StopInstrumentNote(voice);
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        public static void KillAllInstruments()
        {
            var voices = SongData.GetVoices();
            var currentVoices = new List<WaveGenerator>(voices);
            foreach (var voice in currentVoices)
            {
                if (voice == null) return;
                voice.Stop();
                voices.Remove(voice);
            }

            _playerTimer?.Stop();
        }

        public static void PlayAll()
        {
            NotesDrawing.CurrentTime = 0;
            NotesDrawing.Draw();

            var bpmWaitTime = 60000 / Bpm;
            var timerWaitTime = bpmWaitTime / PlayResolution;
            _playerTimer.Interval = (long) timerWaitTime;

            var songLength = SongData.CalculateLength();
            var noteThreshold = 1 / PlayResolution / 2;

            float currentBeat = 0;
            var allNotes = (from instrumentKeyValuePair in SongData.NoteDb let instrumentName = instrumentKeyValuePair.Key from note in instrumentKeyValuePair.Value select new NoteAndInstrument(instrumentName, note)).ToList();

            void OnPlayerTimerOnElapsed(object sender)
            {
                foreach (var noteAndInstrument in allNotes)
                {
                    var note = (Note) noteAndInstrument;
                    var instrumentName = noteAndInstrument.InstrumentName;

                    if (Math.Abs(note.StartBeat - currentBeat) < noteThreshold)
                    {
                        Logger.Console(LogLevel.Debug, "Playing note");
                        noteAndInstrument.Voice = SongData.PlayInstrumentNote(instrumentName, note);
                    }

                    if (Math.Abs(note.EndBeat - currentBeat) < noteThreshold)
                    {
                       SongData.StopInstrumentNote(noteAndInstrument.Voice);
                    }
                }

                currentBeat += 1 / PlayResolution;
                NotesDrawing.CurrentTime = currentBeat;

                if (currentBeat >= songLength)
                {
                    _playerTimer.Elapsed -= OnPlayerTimerOnElapsed;
                    _playerTimer.Stop();

                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MainWindow.EndButton.IsEnabled = false;
                        MainWindow.StartButton.IsEnabled = true;
                    });

                    NotesDrawing.CurrentTime = 0;
                }

                Application.Current.Dispatcher.Invoke(NotesDrawing.Draw);
            }

            _playerTimer.Elapsed += OnPlayerTimerOnElapsed;
            _playerTimer.Start();
        }
    }
}