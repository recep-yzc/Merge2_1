using System.Collections.Generic;
using _Game.Development.Level;

namespace _Game.Development.Board
{
    public abstract class BoardGlobalValues
    {
        public static List<GridData> GridDataList { get; set; } = new();
    }
}