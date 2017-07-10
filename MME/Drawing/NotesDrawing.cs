using System;
using System.IO.Packaging;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.Native;
using MME.Console;
using MME.Data;
using MME.Storage;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace MME.Drawing
{
    public static class NotesDrawing
    {
        public static Canvas OutputCanvas;
        public static Point NoteSize = new Point(50, 25);
        public static string CurrentInstrumentName;

        private static float _currentTime;
        public static float CurrentTime
        {
            get => _currentTime;
            set => _currentTime = Math.Max(0, Math.Min(SongData.CalculateLength(), value));
        }

        private static WriteableBitmap _bitmap;

        public static void Draw()
        {
            if (OutputCanvas == null) return;

            if (_bitmap == null) _bitmap = BitmapFactory.New((int)OutputCanvas.ActualWidth, (int)OutputCanvas.ActualHeight);


            _bitmap.Clear(Colors.White);

            var canvasSize = new Point((int)OutputCanvas.ActualWidth, (int)OutputCanvas.ActualHeight);

            for (var y = 0; y <= canvasSize.Y; y += NoteSize.Y)
            {
                _bitmap.DrawLine(0, y, canvasSize.X, y, Colors.Black);
            }

            for (var x = 0; x <= canvasSize.X; x += NoteSize.X)
            {
                _bitmap.DrawLine(x, 0, x, canvasSize.Y, Colors.Black);
            }


            if (CurrentInstrumentName != null)
            {
                var instrumentNotes = SongData.GetInstrumentNotes(CurrentInstrumentName);

                foreach (var instrumentNote in instrumentNotes)
                {
                    var accentColorBrush = (SolidColorBrush)Application.Current.FindResource("AccentColorBrush");
                    if (accentColorBrush != null)
                        _bitmap.FillRectangle(
                            (int)(instrumentNote.StartBeat * NoteSize.X + 1),
                            (Notes.NoteFreqs.Length - instrumentNote.Index) * NoteSize.Y + 1,
                            (int)(instrumentNote.StartBeat * NoteSize.X + instrumentNote.LengthBeats * NoteSize.X),
                            (Notes.NoteFreqs.Length - instrumentNote.Index + 1) * NoteSize.Y,
                            accentColorBrush.Color
                        );
                }
            }

            var darkAccentColorBrush = (SolidColorBrush) Application.Current.FindResource("AccentColorBrush2");
            if (darkAccentColorBrush != null)
            {
                _bitmap.FillRectangle(
                    (int)(CurrentTime * NoteSize.X - 1),
                    0,
                    (int)(CurrentTime * NoteSize.X + 1),
                    canvasSize.Y,
                    darkAccentColorBrush.Color
                );
            }

            _bitmap.FillRectangle(0, 0, 60, 30, Colors.White);

            Render();
        }

        public static void Render()
        {
            if (OutputCanvas == null) return;

            var image = new Image {Source = _bitmap};

            OutputCanvas.Children.Clear();
            OutputCanvas.Children.Add(image);

            for (var y = 1; y < Math.Min(Notes.NoteNames.Length, OutputCanvas.ActualHeight / NoteSize.Y); y++)
            {
                var text = new TextBlock
                {
                    Text = Notes.NoteNames[Notes.NoteNames.Length - y - 1]
                };

                Canvas.SetLeft(text, 4);
                Canvas.SetTop(text, y * NoteSize.Y + 4);

                OutputCanvas.Children.Add(text);
            }

            var voicesCountText = new TextBlock
            {
                Text = "Voices: " + SongData.GetVoicesCount()
            };
            Canvas.SetLeft(voicesCountText, 5);
            Canvas.SetTop(voicesCountText, 5);

            OutputCanvas.Children.Add(voicesCountText);
        }
    }
}