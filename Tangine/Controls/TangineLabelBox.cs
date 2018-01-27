﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Tangine.Controls
{
    [DesignerCategory("Code")]
    public class TangineLabelBox : Control
    {
        private Rectangle _titleRect;

        [Browsable(false)]
        public TextBox Box { get; }

        [DefaultValue(false)]
        public bool IsReadOnly
        {
            get => Box.ReadOnly;
            set => Box.ReadOnly = value;
        }

        [DefaultValue(null)]
        public override string Text
        {
            get => Box.Text;
            set => Box.Text = value;
        }

        private int _textPaddingWidth = 0;
        [DefaultValue(10)]
        public int TextPaddingWidth
        {
            get => _textPaddingWidth;
            set
            {
                _textPaddingWidth = value;
                Title = Title;
            }
        }

        private string _title;
        [DefaultValue(null)]
        public string Title
        {
            get => _title;
            set
            {
                _title = value;

                Size titleSize = TextRenderer.MeasureText(Title, Font);
                titleSize.Width += TextPaddingWidth;
                titleSize.Height = Height;
                _titleRect = new Rectangle(new Point(0, -1), titleSize);

                Box.Size = new Size(Width - titleSize.Width - 7, Height);
                Invalidate();
            }
        }

        [DefaultValue(true)]
        public new bool TabStop
        {
            get => base.TabStop;
            set
            {
                Box.TabStop = value;
                base.TabStop = value;
            }
        }

        public TangineLabelBox()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            Size = new Size(200, 20);
            DoubleBuffered = true;

            Box = new TextBox
            {
                Dock = DockStyle.Right,
                TextAlign = HorizontalAlignment.Center
            };
            Box.KeyDown += Box_KeyDown;
            Box.TextChanged += Box_TextChanged;

            Controls.Add(Box);
        }

        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }
        private void Box_TextChanged(object sender, EventArgs e)
        {
            OnTextChanged(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Box.Focus();
        }
        protected override void Select(bool directed, bool forward)
        {
            base.Select(directed, forward);
            Box.Select();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (!string.IsNullOrWhiteSpace(Title))
            {
                TextRenderer.DrawText(e.Graphics, Title, Font, _titleRect, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                using (var lineColor = new Pen(Color.FromArgb(243, 63, 63)))
                {
                    e.Graphics.DrawLine(lineColor, _titleRect.Right, 0, _titleRect.Right, Height);
                }
            }
            base.OnPaint(e);
        }
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, 20, specified);
            if (Box != null)
            {
                Title = Title;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                Box.KeyDown -= Box_KeyDown;
                Box.TextChanged -= Box_TextChanged;
                Box.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}