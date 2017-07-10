using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Win32;
using MME.Console;
using MME.Data;
using MME.Drawing;
using MME.Project;
using MME.Sound;
using MME.Storage;
using SplashScreen = MME.Exceptions.SplashScreen;

namespace MME
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private const int SwHide = 0;
        private const int SwShow = 5;

        private static readonly IntPtr _consoleWindow = GetConsoleWindow();

        public static ListBox InstrumentsList;
        public static Button StartButton;
        public static Button EndButton;

        public static float NoteSnap = 1/8f;

        private static Window _win;

        public static bool UnsavedChanges = true;

        private bool _isConsoleOpen;

        public static MainWindow MainWindowAsStatic;

        public MainWindow()
        {
            Logger.Console(LogLevel.Info, "Initialising...");
            InitializeComponent();

            MainWindowAsStatic = this;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private void _toggleConsole()
        {
            if (_isConsoleOpen)
            {
                _isConsoleOpen = false;
                ShowWindow(_consoleWindow, SwHide);
            }
            else
            {
                _isConsoleOpen = true;
                ShowWindow(_consoleWindow, SwShow);
            }
        }

        private void MenuFileQuit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = true;
            Piano.PlayAll();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = false;
            Piano.KillAllInstruments();
        }

        private void MenuFileOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var dialogue = new OpenFileDialog
            {
                Filter = "MiniMusic Files (*.mmx)|*.mmx"
            };

            var result = dialogue.ShowDialog();

            if (result != true) return;

            var filename = dialogue.FileName;
            ProjectFile.LoadProject(filename);

            Piano.PlayResolution = 4;
            Piano.Bpm = SongData.Bpm;
        }

        public void MenuFileSaveAs_OnClick(object sender, RoutedEventArgs e)
        {
            var dialogue = new SaveFileDialog
            {
                FileName = "My MiniMusic Project.mmx",
                Filter = "MiniMusic Files (*.mmx)|*.mmx"
            };

            var result = dialogue.ShowDialog();

            if (result != true) return;
            
            var filename = dialogue.FileName;
            ProjectFile.SaveProject(filename);
        }

        private void MenuFileSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProjectFile.CurrentFileLocation == null)
                MenuFileSaveAs_OnClick(sender, e);
            else
                ProjectFile.SaveProject(ProjectFile.CurrentFileLocation);
        }

        public static void ChangeTitle(string newTitle)
        {
            _win.Title = newTitle;
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            var addInstrumentWindow = new AddInstrumentWindow {ShowInTaskbar = false};
            var response = addInstrumentWindow.Open();

            if (response.InstrumentName == null) return;
            SongData.AddInstrument(response.InstrumentName, response.InstrumentFunction);
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentSelectedInstrument = InstrumentListBox.SelectedItem;
            if (currentSelectedInstrument == null) return;

            var instrumentName = currentSelectedInstrument.ToString();
            SongData.RemoveInstrument(instrumentName);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (!UnsavedChanges) return;

            var mbResult = MessageBox.Show("Are you sure you want to exit? You have unsaved changes!", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (mbResult == MessageBoxResult.No)
                e.Cancel = true;
            else Application.Current.Shutdown();
        }

        private void ToggleConsoleWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _toggleConsole();
            System.Console.WriteLine("Toggled console window visibility");
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            InstrumentsList = InstrumentListBox;
            StartButton = PlayButton;
            EndButton = StopButton;
            _win = this;

            Commands.SaveCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            Commands.SaveAsCommand.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            Commands.OpenCommand.InputGestures.Add(new KeyGesture(Key.O, ModifierKeys.Control));

            Logger.Console(LogLevel.Info, "Initialising logger");
            Logger.Init(LoggerOutput);

            Logger.Console(LogLevel.Info, "Initialising sequencer...");

            NotesDrawing.OutputCanvas = Sequencer;
            NotesDrawing.Draw();

            ShowWindow(_consoleWindow, SwHide);

            Logger.Console(LogLevel.Info, "Ready!");
            Logger.Info("MiniMusic Editor is ready to rock and roll");
        }

        private void InstrumentListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NotesDrawing.CurrentInstrumentName = InstrumentListBox.SelectedItem.ToString();
            NotesDrawing.Draw();

            Logger.Debug("Changed instrument to " + InstrumentListBox.SelectedItem);
        }

        private bool _isSequencerMouseDown;
        private int _sequencerMouseMovement;
        private Point _previousMousePos = new Point(0, 0);

        private NoteAndInstrument _noteToChange;
        private WaveGenerator _noteToChangeVoice;
        private bool _changeStartOfNote;

        private void Sequencer_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isSequencerMouseDown = true;

            _sequencerMouseMovement = 0;
            _previousMousePos = e.MouseDevice.GetPosition(Sequencer);
            Sequencer.CaptureMouse();

            var mousePos = e.MouseDevice.GetPosition(Sequencer);

            var timeAtCursor = mousePos.X / NotesDrawing.NoteSize.X;

            // Set cursor if over the edge of a note
            foreach (var note in SongData.EveryNote())
            {
                if (Math.Abs(note.StartBeat - timeAtCursor) < 0.1 &&
                    Notes.NoteFreqs.Length - (int) (mousePos.Y / NotesDrawing.NoteSize.Y) == note.Index)
                {
                    _noteToChange = note;
                    _changeStartOfNote = true;

                    //_noteToChangeVoice = SongData.PlayInstrumentNote(note.InstrumentName, note);
                }

                if (Math.Abs(note.EndBeat - timeAtCursor) < 0.1 &&
                    Notes.NoteFreqs.Length - (int)(mousePos.Y / NotesDrawing.NoteSize.Y) == note.Index)
                {
                    _noteToChange = note;
                    _changeStartOfNote = false;

                    //_noteToChangeVoice = SongData.PlayInstrumentNote(note.InstrumentName, note);
                }
            }

            _noteToChange?.UpdateOriginal();
        }

        private void Sequencer_OnMouseMove(object sender, MouseEventArgs e)
        {
            var mousePos = e.MouseDevice.GetPosition(Sequencer);

            var movementX = _previousMousePos.X - mousePos.X;
            var movementY = _previousMousePos.Y - mousePos.Y;
            _sequencerMouseMovement += (int)Math.Sqrt(movementX * movementX + movementY * movementY);

            _previousMousePos = mousePos;

            var timeAtCursor = mousePos.X / NotesDrawing.NoteSize.X;

            // Set cursor if over the edge of a note
            var mouseIsOnEdgeOfNote = false;
            foreach (var note in SongData.EveryNote())
            {
                if (Math.Abs(note.StartBeat - timeAtCursor) < 0.1 &&
                    Notes.NoteFreqs.Length - (int)(mousePos.Y / NotesDrawing.NoteSize.Y) == note.Index) mouseIsOnEdgeOfNote = true;
                if (Math.Abs(note.EndBeat - timeAtCursor) < 0.1 &&
                    Notes.NoteFreqs.Length - (int)(mousePos.Y / NotesDrawing.NoteSize.Y) == note.Index) mouseIsOnEdgeOfNote = true;
            }

            e.MouseDevice.SetCursor(mouseIsOnEdgeOfNote || _noteToChange != null ? Cursors.SizeWE : Cursors.Arrow);

            if (!_isSequencerMouseDown) return;

            if (_noteToChange != null)
            {
                //SongData.StopInstrumentNote(_noteToChangeVoice);

                if (_changeStartOfNote)
                {
                    _noteToChange.StartBeat = (float) Math.Round(timeAtCursor / NoteSnap) * NoteSnap;

                    var newIndex = (int)(Notes.NoteFreqs.Length - mousePos.Y / NotesDrawing.NoteSize.Y);
                    _noteToChange.Index = newIndex;
                }
                else
                {
                    _noteToChange.EndBeat = (float) Math.Round(timeAtCursor / NoteSnap) * NoteSnap;
                }
            }
            else
            {
                NotesDrawing.CurrentTime = (float)timeAtCursor;
            }

            
            NotesDrawing.Draw();
        }

        private void Sequencer_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isSequencerMouseDown = false;
            Sequencer.ReleaseMouseCapture();

            var mousePos = e.MouseDevice.GetPosition(Sequencer);

            if (_sequencerMouseMovement < 10 && NotesDrawing.CurrentInstrumentName != null)
            {
                // a click?
                if (e.ChangedButton == MouseButton.Right)
                {
                    var timeAtCursor = e.MouseDevice.GetPosition(Sequencer).X / NotesDrawing.NoteSize.X;

                    foreach (var note in SongData.EveryNote())
                    {
                        if (timeAtCursor > note.StartBeat && timeAtCursor < note.EndBeat)
                        {
                            SongData.RemoveNote(note);
                        }
                    }
                } else if (e.ChangedButton == MouseButton.Left)
                {
                    var noteIndex = Notes.NoteFreqs.Length - (int)(mousePos.Y / NotesDrawing.NoteSize.Y);
                    var noteStartTime = Math.Round(mousePos.X / NotesDrawing.NoteSize.X / NoteSnap) * NoteSnap;

                    SongData.AddNote(NotesDrawing.CurrentInstrumentName, noteIndex, 1, (float)noteStartTime, 1);
                }

                NotesDrawing.Draw();
            }

            if (_noteToChange != null)
            {
                //SongData.StopInstrumentNote(_noteToChangeVoice);
            }
            else
            {
                NotesDrawing.CurrentTime = (float) (mousePos.X / NotesDrawing.NoteSize.X);
            }

            _noteToChange = null;
        }

        private void Sequencer_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            NotesDrawing.Draw();
        }
    }
}