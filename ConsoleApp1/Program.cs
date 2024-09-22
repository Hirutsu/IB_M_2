const string MagicWord = "MAGICWORD";
const string HashFileName = "hash.dat";

string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
string hashFilePath = Path.Combine(Path.GetDirectoryName(executablePath), HashFileName);

if (!File.Exists(hashFilePath))
{
    // Первый запуск: вычисляем хэш и сохраняем его в файл
    string hash = CalculateHash(executablePath);
    File.WriteAllText(hashFilePath, $"{MagicWord}\n{hash}");
    Console.WriteLine("The hash amount is calculated and stored.");
}
else
{
    // Последующие запуски: проверяем хэш
    string[] lines = File.ReadAllLines(hashFilePath);
    if (lines.Length != 2 || lines[0] != MagicWord)
    {
        Console.WriteLine("The hash sum file is corrupted or does not contain the magic word.");
        return;
    }

    string storedHash = lines[1];
    string currentHash = CalculateHash(executablePath);

    if (storedHash == currentHash)
    {
        Console.WriteLine("The integrity of the executable file has been confirmed.");
    }
    else
    {
        Console.WriteLine("A change has been detected in the executable file!");
    }
}

static string CalculateHash(string filePath)
{
    using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
    {
        ushort hash = 0;
        byte[] buffer = new byte[2];

        while (fs.Read(buffer, 0, buffer.Length) > 0)
        {
            ushort segment = BitConverter.ToUInt16(buffer, 0);
            hash ^= segment;
        }

        // Если файл нечетный, дополняем нулями
        if (fs.Position % 2 != 0)
        {
            byte lastByte = (byte)fs.ReadByte();
            hash ^= lastByte;
        }

        return hash.ToString("X4"); // Возвращаем хэш в шестнадцатеричном формате
    }
}