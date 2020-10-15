using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace CenteredCursor
{
    /// <summary>
    /// Keeps the cursor somewhat centered when chaning caret position
    /// </summary>
    internal sealed class ViewportAdornment1
    {
        /// <summary>
        /// Text view to add the adornment on.
        /// </summary>
        private readonly IWpfTextView view;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewportAdornment1"/> class.
        /// </summary>
        /// <param name="view">The <see cref="IWpfTextView"/> in which the cursor should be kept centered.</param>
        public ViewportAdornment1(IWpfTextView view)
        {
            this.view = view ?? throw new ArgumentNullException("view");
            this.view.Caret.PositionChanged += Caret_PositionChanged;
        }

        private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            if (System.Windows.Input.Mouse.LeftButton.HasFlag(System.Windows.Input.MouseButtonState.Pressed))
            {
                return;
            }

            var fromLine = view.Caret.ContainingTextViewLine.Start.GetContainingLine().LineNumber - 10;
            var toLine = view.Caret.ContainingTextViewLine.End.GetContainingLine().LineNumber + 10;
            var lines = view.VisualSnapshot.Lines;

            var startLine = lines.FirstOrDefault(line => line.LineNumber == fromLine - 1);
            var endLine = lines.FirstOrDefault(line => line.LineNumber == toLine - 1);

            if (startLine == null || endLine == null)
            {
                return;
            }

            var startPosition = startLine.Start;
            var endPosition = endLine.Start;

            var span = new SnapshotSpan(view.TextSnapshot, Span.FromBounds(startPosition, endPosition));
            view.ViewScroller.EnsureSpanVisible(span, EnsureSpanVisibleOptions.AlwaysCenter);
        }
    }
}
