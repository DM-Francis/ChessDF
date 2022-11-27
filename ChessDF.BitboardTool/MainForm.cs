using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessDF.BitboardTool
{
    public partial class MainForm : Form
    {
        private readonly string _files = "abcdefgh";
        private Color _activeColor = System.Drawing.Color.Gray;
        private readonly Dictionary<Color, Bitboard> _bitboards = new();

        public MainForm()
        {
            InitializeComponent();
            InitializeTableLayoutPanel();
            PickColorButton.BackColor = _activeColor;
        }

        private void InitializeTableLayoutPanel()
        {
            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel.RowCount; j++)
                {                    
                    var button = new Button
                    {
                        Dock = DockStyle.Fill,
                        Text = $"{_files[i]}{8 - j}"
                    };
                    button.Click += new EventHandler(OnButtonClick);
                    tableLayoutPanel.Controls.Add(button, i, j);
                }
            }
        }

        private void OnButtonClick(object? sender, EventArgs e)
        {
            if (sender is not Button button)
                throw new InvalidOperationException($"{nameof(OnButtonClick)} called from non-button control.");

            if (button.BackColor == _activeColor)
            {
                ResetButtonColor(button);
            }
            else
            {
                button.BackColor = _activeColor;
            }

            RefreshBitboards();
        }

        private void PickColorButton_Click(object sender, EventArgs e)
        {
            var colorDialog = new ColorDialog
            {
                Color = _activeColor
            };

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                _activeColor = colorDialog.Color;
                if (sender is Button button)
                    button.BackColor = colorDialog.Color;

                RefreshBitboards();
            }
        }

        private void UpdateListViewFromBitboards()
        {
            listView.Items.Clear();

            foreach((Color color, Bitboard bitboard) in _bitboards)
            {
                var newItem = new ListViewItem
                {
                    BackColor = color,
                    UseItemStyleForSubItems = false
                };

                newItem.SubItems.Add(bitboard.ToString());
                listView.Items.Add(newItem);
            }
        }

        private void RefreshBitboards()
        {
            var allButtons = tableLayoutPanel.Controls.Cast<Button>().ToList();
            var currentColors = allButtons.Select(b => b.BackColor).Where(c => c != SystemColors.Control);

            _bitboards.Clear();
            foreach (Color color in currentColors)
            {
                var coloredButtons = allButtons.Where(b => b.BackColor == color);

                Bitboard newBitboard = 0;
                foreach(Button button in coloredButtons)
                {
                    int index = GetIndexForButton(button);
                    newBitboard |= (ulong)1 << index;
                }

                _bitboards[color] = newBitboard;
            }

            UpdateListViewFromBitboards();
        }

        private int GetIndexForButton(Button button)
        {
            TableLayoutPanelCellPosition position = tableLayoutPanel.GetPositionFromControl(button);
            return ConvertToBBIndex(position.Column, position.Row);
        }

        private static int ConvertToBBIndex(int column, int row) => (7 - row) * 8 + column;

        private void ClearAllButton_Click(object sender, EventArgs e)
        {
            foreach(var button in tableLayoutPanel.Controls.Cast<Button>())
            {
                ResetButtonColor(button);
            }

            RefreshBitboards();
        }

        private static void ResetButtonColor(Button button)
        {
            button.BackColor = default;
            button.UseVisualStyleBackColor = true;
        }

        private void CopySelectedBitboardsButton_Click(object sender, EventArgs e)
        {
            var stringBuilder = new StringBuilder();
            var selectedItems = listView.SelectedItems.Cast<ListViewItem>().ToList();

            for (int i = 0; i < selectedItems.Count; i++)
            {
                ListViewItem listitem = selectedItems[i];

                if (i > 0)
                    stringBuilder.AppendLine();

                stringBuilder.Append(listitem.SubItems[1].Text);
            }

            if (stringBuilder.Length > 0)
                Clipboard.SetText(stringBuilder.ToString());
        }

        private void LoadBitboardButton_Click(object sender, EventArgs e)
        {
            string bitboardText = BitboardTextBox.Text;

            if (bitboardText.StartsWith("0x"))
                bitboardText = bitboardText[2..];

            bitboardText = bitboardText.Replace("_", "");

            if (ulong.TryParse(bitboardText, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong bits))
            {
                _bitboards[_activeColor] = new Bitboard(bits);
                RefreshGridFromBitboards();
                RefreshBitboards();
            }
        }

        private void RefreshGridFromBitboards()
        {
            foreach (var button in tableLayoutPanel.Controls.Cast<Button>())
            {
                ResetButtonColor(button);
            }

            foreach ((Color color, Bitboard bitboard) in _bitboards)
            {
                var indexes = bitboard.Serialize();

                foreach(int index in indexes)
                {
                    (int col, int row) = ConvertIndexToPosition(index);

                    if (tableLayoutPanel.GetControlFromPosition(col, row) is Button button)
                    {
                        button.BackColor = color;
                    }
                }
            }
        }

        private static (int col, int row) ConvertIndexToPosition(int index)
        {
            int col = index % 8;
            int row = 7 - index / 8;

            return (col, row);
        }
    }
}
