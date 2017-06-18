using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace YouTown.PlainTextUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var center = new Location(0,0,0);
            var desert = new Desert(Identifier.DontCare, center);
            var board = new BoardForPlay(new Dictionary<Location, IHex>
            {
                {center, desert }
            });

            RenderBoardForPlay(board);
        }

        private class AxisMaxima
        {
            public AxisMaxima(IBoardForPlay board)
            {
                X = board.HexesByLocation.Keys.Max(l => l.X);
                Y = board.HexesByLocation.Keys.Max(l => l.Y);
                Z = board.HexesByLocation.Keys.Max(l => l.Z);
                MinusX = Math.Abs(board.HexesByLocation.Keys.Min(l => l.X));
                MinusY = Math.Abs(board.HexesByLocation.Keys.Min(l => l.Y));
                MinusZ = Math.Abs(board.HexesByLocation.Keys.Min(l => l.Z));
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int MinusX { get; set; }
            public int MinusY { get; set; }
            public int MinusZ { get; set; }
            public int CenterX { get; set; }
            public int CenterY { get; set; }

        }

        private void RenderBoardForPlay(BoardForPlay board)
        {
            const int edgeSize = 2; // 2 diagonal uses 5 hori
            const int horizontalSize = 5;
            var stringBuilder = new StringBuilder();
            /*
            //   ╱ ╲
            // ╱     ╲
            //
            */
            var maxima = new AxisMaxima(board);
            var maxZ = maxima.X + maxima.MinusX + 1;
//            var width = (maxX * (horizontalSize + edgeSize)) + edgeSize;
//            var center = 
        }
    }
}
