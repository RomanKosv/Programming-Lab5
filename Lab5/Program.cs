

using System;
using System.Collections;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Formats.Asn1;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

Random random = new Random(DateTime.Now.Nanosecond);

const string helpMessage = """
Введите одну из следующих команд:
help для вывода инструкции
create-matrix для создания двумерного массива
create-arr для создания зубчатого массива
create-str для создания строки
remove для удаления столбцов из двумерного массива
add для добавления массива в начало зубчатого массива
transform для переворота всех слов и сортировки слов в предложении
print-matrix для печати матрицы
print-arr для пучати зубчатого массива
print-str для печати строки
stop для остановки программы
""";

const string createArrHelp = """
Введите одну из следующих команд:
help для вывода инструкции
random для создания массива случайных чисел
input для создания массива вводимых чисел
""";

const string createStrHelp = """
Введите одну из следующих команд:
help для вывода инструкции
test для использования тестовой
input для ввода строки
""";

const string testStr = "Asdemwl ekrqklmk ermfkmkq!lr,el leml..me";

bool isStoped = false;

int[,]? matrix = null;
int[][]? array = null;
string? str = null;

void DisplayMatrix()
{
    if (matrix == null)
    {
        Console.WriteLine("Отсутствует");
        return;
    }
    for (int y = 0; y < matrix.GetLength(1); y++)
    {
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            Console.Write($"{matrix[x, y]}\t");
        }
        Console.WriteLine();
    }
}

while (!isStoped)
{
    Console.WriteLine("Введите команду или help для вывода инструкции:");
    switch (Console.ReadLine())
    {
        case "help":
            Console.WriteLine(helpMessage);
            break;
        case "create-matrix":
            Console.Write("Введите ширину массива:");
            bool stopInp = false;
            int width = 0;
            while (!stopInp)
            {
                if (int.TryParse(Console.ReadLine(), out width))
                {
                    if ((width >= 0) && (width <= Array.MaxLength))
                    {
                        stopInp = true;
                    }
                    else
                    {
                        Console.Write($"""
                        Ширина массива дожна быть целым числом от 0 до {Array.MaxLength}.
                        Введте ширину массива:
                        """);
                    }
                }
                else
                {
                    Console.Write("""
                    Ширина массива дожна быть целым числом.
                    Введте ширину массива:
                    """);
                }
            }
            int height = 0;
            stopInp = false;
            Console.Write("Введте высоту массива:");
            while (!stopInp)
            {
                if (int.TryParse(Console.ReadLine(), out height))
                {

                    if (
                        (height >= 0)
                        && (
                            ((width == 0) && (height <= Array.MaxLength))
                            || ((width != 0) && (height <= Array.MaxLength / width))
                        )
                    )
                    {
                        stopInp = true;
                    }
                    else
                    {
                        Console.Write($"""
                        Высота массива дожна быть целым неотрицательным числом рузмер массива не должен превышать {Array.MaxLength}.
                        Введте высоту массива:
                        """);
                    }
                }
                else
                {
                    Console.Write("""
                    Высота массива дожна быть целым числом.
                    Введте высоту массива:
                    """);
                }
            }
            matrix = new int[width, height];
            Console.WriteLine("Введите режим создания массива иди help:");
            ChainInput(
                out string method,
                Console.ReadLine,
                [
                    (
                        (command) => command != "help",
                        createArrHelp+"\nВведите режим создания массива иди help:"
                    ),
                    (
                        (command) => command == "random" || command == "input",
                        "Нет такой команды."
                    )
                ]
            );
            switch (method)
            {
                case "input":
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            Console.Write($"Введите элемент {x + 1}x{y + 1}:");
                            stopInp = false;
                            while (!stopInp)
                            {
                                if (int.TryParse(Console.ReadLine(), out matrix[x, y]))
                                {
                                    stopInp = true;
                                }
                                else
                                {
                                    Console.Write(
                                    """
                                    Элемент массива должен быть целым числом.
                                    Введите элемент:
                                    """);
                                }
                            }
                        }
                    }
                    break;
                case "random":
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            matrix[x, y] = random.Next(-100, 100);
                        }
                    }
                    break;
            }
            if (width * height == 0)
            {
                Console.WriteLine($"Создан пусой массив {width}x{height}");
            }
            else
            {
                Console.WriteLine($"Создан массив {width}x{height}:");
                DisplayMatrix();
            }
            break;
        case "create-arr":
            Console.Write("Введите длину массива:");
            int lenght = 0;
            ChainInput(
                out string strLen,
                Console.ReadLine,
                [
                    (
                        (strLen) => int.TryParse(strLen,out lenght),
                        """
                        Длина массива должна быть целым числом.
                        Введите ее заново:
                        """
                    ),
                    (
                        (strLen) => (lenght>=0) && (lenght <Array.MaxLength),
                        $"""
                        Длина массива должна бытьь числом от 0 до {Array.MaxLength}
                        Введите ее заново:
                        """
                    )
                ]
            );
            Console.WriteLine("Введите режим создания массива иди help:");
            ChainInput(
                out method,
                Console.ReadLine,
                [
                    (
                        (command) => command != "help",
                        createArrHelp+"\nВведите режим создания массива иди help:"
                    ),
                    (
                        (command) => command == "random" || command == "input",
                        "Нет такой команды."
                    )
                ]
            );
            switch (method)
            {
                case "random":
                    array = new int[lenght][];
                    for (int i = 0; i < lenght; i++)
                    {
                        array[i] = RandomLine();
                    }
                    break;
                case "input":
                    array = new int[lenght][];
                    for (int i = 0; i < lenght; i++)
                    {
                        array[i] = InputLine();
                    }
                    break;
            }
            if (array.Length == 0)
            {
                Console.WriteLine("Создан пусой массив");
            }
            else
            {
                Console.WriteLine("Создан массив:");
                DisplayArray();
            }
            break;
        case "create-str":
        restart_create_str:
            Console.WriteLine("Введите режим создания строки или help:");
            ChainInput(
                out method,
                Console.ReadLine,
                [
                    (
                        (str) => str!="help",
                        createStrHelp+"\nВведите режим создания строки или help:"
                    ),
                    (
                        (str) => str == "input" || str == "test",
                        "Нет такой команды."
                    )
                ]
            );
            switch (method)
            {
                case "input":
                    Console.Write("Введите строку:");
                    str = Console.ReadLine();
                    break;
                case "test":
                    str = testStr;
                    break;
            }
            Console.WriteLine("Создана строка:");
            Console.WriteLine(str);
            break;
        case "remove":
            if (matrix != null && matrix.GetLength(0) == 0)
            {
                Console.WriteLine("Ширина столбца должна быть более 0");
            }
            else if (matrix != null)
            {
                int k1 = 0;
                bool restart;
                Console.WriteLine("Введите номер первого столбца");
                ChainInput(
                    out string strK1,
                    Console.ReadLine,
                    [
                        (
                            (str) => int.TryParse(str, out k1) && (k1 > 0) && (k1 <= matrix.GetLength(0)),
                            $"Номер стлбца должен быть целым числом от 1 до {matrix.GetLength(0)}."
                        )
                    ]
                );
                Console.WriteLine("Введите номер последнего столбца");
                int k2 = 0;
                ChainInput(
                    out string strK2,
                    Console.ReadLine,
                    [
                        (
                            (str) => int.TryParse(str, out k2) && (k2 >= k1) && (k2 <= matrix.GetLength(0)),
                            $"Номер стлбца должен быть целым числом от {k1} до {matrix.GetLength(0)}."
                        )
                    ]
                );
                k1--;
                k2--;
                int[,] new_matrix = new int[matrix.GetLength(0) - k2 + k1 - 1, matrix.GetLength(1)];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    for (int i = 0; i < k1; i++) new_matrix[i, j] = matrix[i, j];
                    for (int i = k2 + 1; i < matrix.GetLength(0); i++) new_matrix[i - k2 + k1 - 1, j] = matrix[i, j];
                }
                matrix = new_matrix;
                if (matrix.Length == 0)
                {
                    Console.WriteLine("Измененный массив пуст.");
                }
                else
                {
                    Console.WriteLine("Измененный массив:");
                    DisplayMatrix();
                }
            }
            else
            {
                Console.WriteLine("Массив должен быть инициализирован.");
            }
            break;
        case "add":
            if (array != null)
            {
                int len = 0;
                Console.Write("Введите длину:");
                ChainInput(
                    out string inpt,
                    Console.ReadLine,
                    [
                        (
                            (str) => int.TryParse(str, out len),
                            """
                            Длина должна бать числом
                            Введите ее заново:
                            """
                        ),
                        (
                            (str) => (0<len) && (len < Array.MaxLength),
                            $"""
                            Длина доллжна быть числом от 0 до {Array.MaxLength}
                            """
                        )
                    ]
                );
                int[] newRow = new int[len];
                string line = "";
                ChainInput(
                    out line,
                    Console.ReadLine,
                    [
                        (
                            (str) => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length==len,
                            $"""
                            Строка должна содержать {len} чисел
                            Введите строку заново:
                            """
                        ),
                        (
                            (str) => !line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(
                                (str, i) => int.TryParse(str,out newRow[i])
                            ).Contains(false),
                            """
                            Строка должна состоять из целых чисел
                            Введите строку заново:
                            """
                        )
                    ]
                );
                int[][] newArray = new int[array.Length + 1][];
                newArray[0] = newRow;
                for (int i = 0; i < array.Length; i++)
                {
                    newArray[i + 1] = array[i];
                }
                array = newArray;
                Console.WriteLine("Измененный массив:");
                DisplayArray();
            }
            else Console.WriteLine("Массив должен быть инициализирован");
            break;
        case "transform":
            if (str != null)
            {
                Regex regex = new Regex(@"(.*?(?:\.|!|\?))");
                var sentences = regex.Matches(str);
                List<string> strSent = new List<string>(MyMap<string, Match>(sentences, (a, i) => a.ToString()));
                for (int i = 0; i < strSent.Count(); i++)
                {
                    IEnumerable<string> words = Sort(
                        MyMap<string, Match>(
                            new Regex(@"\w+").Matches(strSent[i]),
                            (s, i) => JoinCh(Sort<char>(s.ToString(), (a, b) => a <= b))
                        ),
                        (a, b) => a.Length <= b.Length
                    );
                    var enumer = words.GetEnumerator();
                    strSent[i] = Regex.Replace(strSent[i], @"\w+", (match) =>
                    {
                        enumer.MoveNext();
                        return enumer.Current;
                    });
                }

                str = Join(
                    strSent
                );
                Console.WriteLine("Измененная строка:");
                Console.WriteLine(str);
            }
            else Console.WriteLine("Строка должна быть инициализирована");
            break;
        case "stop":
            isStoped = true;
            break;
        case "print-str":
            Console.WriteLine("Текущая строка:");
            if (str == null) Console.WriteLine("Отсутствует");
            else Console.WriteLine(str);
            break;
        case "print-arr":
            if (array!=null && array.Length == 0) Console.WriteLine("Текущий массив пуст");
            else
            {
                Console.WriteLine("Текущий массив:");
                DisplayArray();
            }
            break;
        case "print-matrix":
            if (matrix == null || matrix.Length != 0)
            {
                Console.WriteLine($"Текущая матрица:");
                DisplayMatrix();
            }
            else
            {
                Console.WriteLine($"Текущая матрица пуста");
            }
            break;
        default:
            Console.WriteLine("Нет такой команды");
            break;
    }
}

int[] RandomLine()
{
    int len = GetLength();
    int[] line = new int[len];
    for (int i = 0; i < len; i++)
    {
        line[i] = random.Next(-100, 100);
    }
    return line;
}

int[] InputLine()
{
    string[] splt = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
    int[] rez = new int[splt.Length];
    for (int i = 0; i < splt.Length; i++)
    {
        if (!int.TryParse(splt[i], out rez[i]))
        {
            Console.Write("""
            Массив должен состоять только из чисел.
            """);
            return InputLine();
        }
    }
    return rez;
}


int GetLength()
{
    Console.Write("Введите длину внутреннего массива:");
    while (true)
    {
        if (int.TryParse(Console.ReadLine(), out int len) && len >= 0 && len <= Array.MaxLength)
        {
            return len;
        }
        else
        {
            Console.WriteLine($"""
                                Длина массива должна быть целым неотрицательным числом не более {Array.MaxLength}
                                Введите ее заново:
                                """);
        }
    }
}

void DisplayArray()
{
    if (array == null)
    {
        Console.WriteLine("Отсутствует");
        return;
    }
    foreach (int[] line in array)
    {
        foreach (int element in line)
        {
            Console.Write($"{element} ");
        }
        Console.WriteLine();
    }
}

void ChainInput<T>(out T rez, Func<T> inputF, IEnumerable<(Predicate<T>, string)> checkChain)
{
    rez = inputF();
    foreach (var (checker, message) in checkChain)
    {
        if (!checker(rez))
        {
            Console.Write(message);
            ChainInput(out rez, inputF, checkChain);
        }
    }
}

IEnumerable<Out> MyMap<Out, In>(
    IEnumerable<In> input,
    Func<In, int, Out> fun
)
{
    int i = 0;
    var enumer = input.GetEnumerator();
    while (enumer.MoveNext())
    {
        yield return fun(enumer.Current, i);
    }
}
IEnumerable<T> Sort<T>(IEnumerable<T> str, Func<T, T, bool> func) where T : IComparable<T>
{
    if (!str.GetEnumerator().MoveNext()) return str;
    T comparer = str.First();
    List<T> s1, s2;
    s1 = new List<T>();
    s2 = new List<T>();
    foreach (T sym in str.Skip(1))
    {
        if (func(sym, comparer)) s1.Add(sym);
        else s2.Add(sym);
    }
    return Sort(s1, func).Append(comparer).Concat(Sort(s2, func));
}

string Join(IEnumerable<string> strs)
{
    return strs.Aggregate("", (a, b) => a + b);
}

string JoinCh(IEnumerable<char> strs)
{
    return Join(MyMap<string, char>(strs, (ch, i) => ch.ToString()));
}