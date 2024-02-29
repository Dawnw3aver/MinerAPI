using MinerAPI.APIObjects;

namespace MinerAPI.Entities
{
    public class Game
    {
        #region Properties
        public Guid Game_id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Mines_Count { get; set; }
        public bool Completed { get; set; }
        public List<List<char>> Field
        {
            get
            {
                var list = new List<List<char>>();
                for (int i = 0; i < Height; i++)
                {
                    list.Add([]);
                    for (int j = 0; j < Width; j++)
                    {
                        list[i].Add(_field[i][j].GetCellValue(false));
                    }
                }
                return list;
            }
        }
        private List<List<Cell>> _field;
        #endregion

        #region Constructors
        public Game(int width, int height, int mines_count)
        {
            Game_id = Guid.NewGuid();
            Height = height;
            Width = width;
            Mines_Count = mines_count;
            SetupField();
            //PrintField();
        }
        #endregion

        #region Methods
        private void SetupField()
        {
            _field = [];
            for (int i = 0; i < Height; i++)
            {
                _field.Add([]);
                for (int j = 0; j < Width; j++)
                {
                    _field[i].Add(new Cell() { Value = '0', IsHidden = true});
                }
            }
            LandMines();
            MarkField();
        }

        private void LandMines()
        {
            for (int i = 0; i < Mines_Count; i++)
            {
                int randomWidth = Random.Shared.Next(0, Width);
                int randomHeight = Random.Shared.Next(0, Height);
                if (_field[randomWidth][randomHeight].GetCellValue() == 'X') { i--; continue; }
                _field[randomWidth][randomHeight].Value = 'X';
            }
        }

        private void MarkField()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (_field[i][j].GetCellValue() != 'X')
                    {
                        int count = 0;
                        for (int x = i - 1; x < i + 2; x++)
                        {
                            for (int y = j - 1; y < j + 2; y++)
                            {
                                if(x < 0 || y < 0 || x > Height - 1 || y > Width - 1)
                                    continue;
                                if(_field[x][y].GetCellValue() == 'X')
                                    count++;
                            }
                        }
                        _field[i][j].Value = char.Parse(count.ToString());
                    }
                }
            }
        }

        private void PrintField()
        {
            foreach (var row in _field)
            {
                foreach (var item in row)
                {
                    Console.Write(item.GetCellValue() + " ");
                }
                Console.WriteLine();
            }
        }

        public Game GameTurn(GameTurnRequest request)
        {
            Cell cell = _field[request.Row][request.Col];
            switch (cell.GetCellValue())
            {
                case '0':
                    OpenZeroCells();
                    return this;
                case 'X':
                    Completed = true;
                    OpenAllCells();
                    return this;
                default:
                    OpenCell(request.Col, request.Row);
                    return this;
            }
        }

        private bool HasHiddenEmptyCell()
        {
            foreach (var row in _field)
            {
                foreach (var item in row)
                {
                    if (item.IsHidden && item.GetCellValue() != 'X')
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void OpenZeroCells()
        {
            foreach (var row in _field)
            {
                foreach (var item in row)
                {
                    if (item.IsHidden == true && item.GetCellValue() == '0')
                    {
                        item.IsHidden = false;
                    }
                }
            }
        }

        private void OpenAllCells()
        {
            foreach (var row in _field)
            {
                foreach (var item in row)
                {
                    item.IsHidden = false;
                }
            }
        }

        private void OpenCell(int col, int row)
        {
            _field[row][col].IsHidden = false;
            if(!HasHiddenEmptyCell())
            {
                Completed = true;
                OpenAllCells();
                foreach (var row1 in _field)
                {
                    foreach (var item in row1)
                    {
                        if (item.GetCellValue() == 'X')
                            item.Value = 'M';
                    }
                }
            }
        }
        #endregion
    }

    #region NestedClasses
    class Cell()
    {
        public char GetCellValue(bool ignoreHidden = true)
        {
            return IsHidden && !ignoreHidden ? ' ' : Value;
        }
        public char Value { private get; set; }
        public bool IsHidden {  get; set; }
    }
    #endregion
}
