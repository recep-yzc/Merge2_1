using System.Collections.Generic;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Grid.Serializable;

namespace _Game.Development.Board.Controller
{
    public abstract class BoardGlobalValues
    {
        public static List<GridData> GridDataList { get; set; } = new();
        public static BoardJsonData BoardJsonData { get; set; }

    }
}