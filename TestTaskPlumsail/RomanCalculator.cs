using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace TestTaskPlumsail
{
    public class RomanCalculator
    {
        private Dictionary<char, int> _romanMap = new Dictionary<char, int>()
        { {'I', 1}, {'V', 5}, {'X', 10}, {'L', 50}, {'C', 100}, {'D', 500}, {'M', 1000} };

        public string Evaluate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentException("input is null or empty");
            }

            input = input.Replace(" ", "");
            input = ReplaceRomanToInt(input);

            var intResult = (int)new DataTable().Compute(input.ToString(), string.Empty);

            return ConvertIntToRomanian(intResult);
        }

        private string ReplaceRomanToInt(string romanInput)
        {
            var intInput = new StringBuilder();
            var romanianNumber = new StringBuilder();

            for (int i = 0; i < romanInput.Length; i++)
            {
                if (_romanMap.ContainsKey(romanInput[i]))
                {
                    romanianNumber.Append(romanInput[i]);
                }
                else if ("+-*()".Any(c => c == romanInput[i]))
                {
                    if (romanianNumber.Length > 0)
                    {
                        intInput.Append(ConvertRomanianToInt(romanianNumber.ToString()).ToString());
                    }
                    romanianNumber.Clear();
                    intInput.Append(romanInput[i]);
                }
                else
                {
                    throw new ArgumentException("invalid input");
                }
            }
            if (romanianNumber.Length > 0)
            {
                intInput.Append(ConvertRomanianToInt(romanianNumber.ToString()).ToString());
            }

            return intInput.ToString();
        }

        private int ConvertRomanianToInt(string number)
        {
            if (!new Regex("^M{0,4}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$").IsMatch(number))
            {
                throw new ArgumentException("invalid romanian number");
            }

            int result = 0;
            for (int i = 0; i < number.Length; i++)
            {
                if (i != (number.Length - 1) && _romanMap[number[i + 1]] > _romanMap[number[i]])
                {
                    result -= _romanMap[number[i]];
                }
                else
                {
                    result += _romanMap[number[i]];
                }
            }

            return result;
        }
        private string ConvertIntToRomanian(int number)
        {
            if (number <= 0 || number >= 4000)
            {
                throw new ArgumentOutOfRangeException();
            }    

            var romanIntMap = new List<(string R, int N)>()
            {
                ("M", 1000), ("CM", 900), ("D", 500), ("CD", 400),
                ("C", 100), ("XC", 90), ("L", 50), ("XL", 40),
                ("X", 10), ("IX", 9), ("V", 5), ("IV", 4),
                ("I", 1)
            };

            var result = new StringBuilder();
            while (number > 0)
            {
                var index = romanIntMap.FindIndex(pair => pair.N <= number);
                number -= romanIntMap[index].N;
                result.Append(romanIntMap[index].R);
            }

            return result.ToString();
        }
    }
}