using ChessDF.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
                button.BackColor = SystemColors.Control;
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

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
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
                button.BackColor = default;
                button.UseVisualStyleBackColor = true;
            }

            RefreshBitboards();
        }

        private void CopySelectedBitboardsButton_Click(object sender, EventArgs e)
        {
            var stringBuilder = new StringBuilder();
            var selectedItems = listView.SelectedItems.Cast<ListViewItem>().ToList();

            for (int i = 0; i < selectedItems.Count; i++)
            {
                ListViewItem listitem = selectedItems[i];

                if (i > 0) stringBuilder.AppendLine();
                stringBuilder.Append(listitem.SubItems[1].Text);
            }

            Clipboard.SetText(stringBuilder.ToString());
        }
    }
}
