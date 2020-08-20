using System;

namespace SumCalculator
{
    public class TriangleManager
    {
        private readonly int[,] _triangle;
        private readonly int[,] _resultTriangle;
        private readonly int _length;

        public TriangleManager(string inputData)
        {
            this._triangle = this.ParseTriangle(inputData);
            this._length = this._triangle.GetLength(0);
            this._resultTriangle = new int[_length, _length];
        }

        public Result Calculate()
        {
            var result = new Result
            {
                MaxSum = this.GetMaxSum(),
                Path = this.GetPath()
            };

            return result;
        }

        private int GetMaxSum()
        {
            if(this._length == 1)
            {
                return this._triangle[0, 0];
            }

            bool isBottomEven = (_triangle[0, 0] % 2 == 0) == (_length % 2 != 0);

            for (int i = _length - 2; i >= 0; i--)
            {
                for (int j = 0; j <= i; j++)
                {
                    int parent = _resultTriangle[i, j] = _triangle[i, j];
                    if (isBottomEven == (parent % 2 != 0))
                    {
                        int firstChild;
                        int secondChild;
                        if (i == _length - 2)
                        {
                            firstChild = _resultTriangle[i + 1, j] = _triangle[i + 1, j];
                            secondChild = _resultTriangle[i + 1, j + 1] = _triangle[i + 1, j + 1];

                            if (isBottomEven == (firstChild % 2 != 0))
                            {
                                firstChild = _resultTriangle[i + 1, j] = -1;
                            }

                            if (isBottomEven == (secondChild % 2 != 0))
                            {
                                secondChild = _resultTriangle[i + 1, j + 1] = -1;
                            }
                        }
                        else
                        {
                            firstChild = _resultTriangle[i + 1, j];
                            secondChild = _resultTriangle[i + 1, j + 1];
                        }

                        if (firstChild == -1 && secondChild == -1)
                        {
                            _resultTriangle[i, j] = -1;
                        }
                        else if (firstChild == -1)
                        {
                            _resultTriangle[i, j] += secondChild;
                        }
                        else if (secondChild == -1)
                        {
                            _resultTriangle[i, j] += firstChild;
                        }
                        else if (firstChild > secondChild)
                        {
                            _resultTriangle[i, j] += firstChild;
                        }
                        else
                        {
                            _resultTriangle[i, j] += secondChild;
                        }
                    }
                    else
                    {
                        _resultTriangle[i, j] = -1;
                    }
                }

                isBottomEven = !isBottomEven;
            }

            return _resultTriangle[0, 0];
        }

        private int[] GetPath()
        {
            var path = new int[_length];
            path[0] = _triangle[0, 0];

            int j = 0;
            for (int i = 1; i < _length; i++)
            {
                var firstChild = _resultTriangle[i, j];
                var secondChild = _resultTriangle[i, j + 1];

                if (firstChild == -1)
                {
                    path[i] = _triangle[i, j + 1];
                    j++;
                }
                else if (secondChild == -1)
                {
                    path[i] = _triangle[i, j];
                }
                else if (firstChild > secondChild)
                {
                    path[i] = _triangle[i, j];
                }
                else
                {
                    path[i] = _triangle[i, j + 1];
                    j++;
                }
            }

            return path;
        }

        private int[,] ParseTriangle(string inputData)
        {
            string[] rows = inputData.Split('\n');
            int[,] triangle = new int[rows.Length, rows.Length];
            string[] columns = null;
            for (int i = 0; i < rows.Length; i++)
            {
                columns = rows[i].Trim().Split(' ');
                for (int j = 0; j < columns.Length; j++)
                {
                    if(!int.TryParse(columns[j], out int number))
                    {
                        throw new ArgumentException("Unknown value");
                    }

                    triangle[i, j] = number;
                }
            }

            if(columns.Length != rows.Length)
            {
                throw new ArgumentException("Wrong size");
            }

            return triangle;
        }
    }
}
