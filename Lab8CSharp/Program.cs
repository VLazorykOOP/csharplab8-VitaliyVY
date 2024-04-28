using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MyProgram
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool exit = false;



            while (!exit)
            {
                Console.WriteLine("\nОберіть опцію:");
                Console.WriteLine("1. Обробити IP-адреси");
                Console.WriteLine("2. Обробити послідовність чисел");
                Console.WriteLine("3. Обробити текст");
                Console.WriteLine("4. Вивести розгорнуту інформацію про створені файли");
                Console.WriteLine("5. Вийти\n");

                int choice;
                if (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Будь ласка, введіть число від 1 до 5.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        ProcessIPAddresses();
                        break;
                    case 2:
                        ProcessNumberSequence();
                        break;
                    case 3:
                        ProcessText();
                        break;
                    case 4:
                        Console.WriteLine("Введіть прізвище студента:");
                        string surname = Console.ReadLine();
                        ProcessStudentData(surname);
                        Console.WriteLine("Дії успішно завершено.");
                        break;
                    case 5:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Виберіть від 1 до 5.");
                        break;
                }
            }
        }

        static void ProcessIPAddresses()
        {
            // Зчитуємо вміст вихідного файлу
            string inputFilePath = "input.txt";
            string outputFilePath1 = "output.txt";
            string outputFilePath2 = "output2.txt";

            string[] lines = File.ReadAllLines(inputFilePath);

            Console.WriteLine("Вміст тексту з вихідного файлу:");

            foreach (string line in lines)
            {
                Console.WriteLine(line);
            }

            Console.WriteLine();


            // Регулярний вираз для знаходження IP-адрес

        // string ipPattern = @"(((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))";


            string ipPattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";
           // (((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))\.){3}((\d{1,2})|(1\d{2})|(2[0-4]\d)|(25[0-5]))

            int ipCount = 0;

            // Запит користувача на введення тексту для заміни IP-адрес
            Console.Write("Введіть текст, на який бажаєте замінити IP-адреси: ");
            string replacementText = Console.ReadLine();

            // Відкриваємо файл для запису заміненого вмісту з пунктуацією
            using (StreamWriter writer = new StreamWriter(outputFilePath1))
            {
                // Пошук і підрахунок IP-адрес у кожному рядку
                foreach (string line in lines)
                {
                    MatchCollection matches = Regex.Matches(line, ipPattern);
                    ipCount += matches.Count;

                    // Заміна IP-адрес за вказаним текстом користувача
                    string modifiedLine = Regex.Replace(line, ipPattern, replacementText);

                    // Запис рядка заміненого вмісту з пунктуацією в новий файл
                    writer.WriteLine(modifiedLine);
                }
            }

            // Відкриваємо файл для запису вмісту без пунктуації та цифр
            using (StreamWriter writer = new StreamWriter(outputFilePath2))
            {
                // Пошук і підрахунок IP-адрес у кожному рядку
                foreach (string line in lines)
                {
                    // Видаляємо всі знаки пунктуації та цифри з рядка
                    string cleanLine = Regex.Replace(line, @"[\p{P}\d]", "");

                    // Запис рядка без пунктуації та цифр в новий файл
                    writer.WriteLine(cleanLine);
                }
            }

            Console.WriteLine($"Кількість знайдених IP-адрес: {ipCount}");
            Console.WriteLine($"Модифікований вміст збережено у файлі '{outputFilePath1}'.");
            Console.WriteLine($"Текст без пунктуації та цифр збережено у файлі '{outputFilePath2}'.");
        }

        static void ProcessNumberSequence()
        {
            // Введення кількості чисел у послідовності
            Console.Write("Введіть кількість чисел у послідовності: ");
            int n = int.Parse(Console.ReadLine());

            // Створення масиву для зберігання послідовності чисел
            int[] sequence = new int[n];

            // Введення чисел у послідовність
            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введіть число {i + 1}: ");
                sequence[i] = int.Parse(Console.ReadLine());
            }

            // Введення назви файлу
            Console.Write("Введіть назву файлу: ");
            string fileName = Console.ReadLine();

            // Запис парних чисел у файл
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                foreach (int number in sequence)
                {
                    if (number % 2 == 0)
                    {
                        writer.Write(number);
                    }
                }
            }

            // Виведення вмісту файлу на екран
            Console.WriteLine("Вміст файлу:");
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    int number = reader.ReadInt32();
                    Console.WriteLine(number);
                }
            }
        }

        static void ProcessText()
        {
            // Зчитуємо вміст першого текстового файлу
            string firstFilePath = "first_text.txt";
            string[] firstSentences = File.ReadAllLines(firstFilePath);

            // Зчитуємо вміст другого текстового файлу
            string secondFilePath = "second_text.txt";
            string[] secondWords = File.ReadAllText(secondFilePath)
                                       .Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

            // Вилучаємо всі слова, які містяться у другому тексті, з кожного речення першого тексту
            var filteredSentences = firstSentences.Select(sentence => string.Join(" ", sentence.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                                                                               .Where(word => !secondWords.Contains(word))));

            // Записуємо результат у новий файл
            string outputFilePath = "result.txt";
            File.WriteAllLines(outputFilePath, filteredSentences);

            Console.WriteLine("Результат записано у файл 'result.txt'.");
        }
        static void ProcessStudentData(string surname)
        {
            string firstName1 = $"{surname}1";
            string firstName2 = $"{surname}2";

            // Шлях до папки temp
            string tempPath = @"D:\temp";

            // Перевірка чи існує папка temp, якщо ні - створення
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            // Папка прізвище_студента1
            string folderPath1 = Path.Combine(tempPath, firstName1);
            Directory.CreateDirectory(folderPath1);

            // Файл t1.txt зі змістом
            string filePath1 = Path.Combine(folderPath1, "t1.txt");
            using (StreamWriter writer = File.CreateText(filePath1))
            {
                writer.WriteLine("<Шевченко Степан Іванович, 2001> року народження, місце проживання <м. Суми>");
            }

            Console.WriteLine($"Створено файл: {filePath1}");

            // Папка прізвище_студента2
            string folderPath2 = Path.Combine(tempPath, firstName2);
            Directory.CreateDirectory(folderPath2);

            // Файл t2.txt зі змістом
            string filePath2 = Path.Combine(folderPath2, "t2.txt");
            using (StreamWriter writer = File.CreateText(filePath2))
            {
                writer.WriteLine("<Комар Сергій Федорович, 2000 > року народження, місце проживання <м. Київ>");
            }

            Console.WriteLine($"Створено файл: {filePath2}");

            // Створення файлу t3.txt у папці прізвище_студента2
            string filePath3 = Path.Combine(folderPath2, "t3.txt");
            string t1Content = File.ReadAllText(filePath1);
            string t2Content = File.ReadAllText(filePath2);
            using (StreamWriter writer = File.CreateText(filePath3))
            {
                writer.WriteLine(t1Content);
                writer.WriteLine(t2Content);
            }

            Console.WriteLine($"Створено файл: {filePath3}");

            // Перенесення файлу t2.txt у папку прізвище_студента2
            string destinationFilePath2 = Path.Combine(folderPath2, "t2.txt");
            File.Move(filePath2, destinationFilePath2);

            Console.WriteLine($"Переміщено файл: {destinationFilePath2}");

            // Копіювання файлу t1.txt у папку прізвище_студента2
            string destinationFilePath1 = Path.Combine(folderPath2, "t1.txt");
            File.Copy(filePath1, destinationFilePath1);

            Console.WriteLine($"Скопійовано файл: {destinationFilePath1}");

            // Перевірка чи існує папка <прізвище_студента>2, перш ніж перейменовувати
            if (Directory.Exists(folderPath2))
            {
                // Перейменування папки <прізвище_студента>2 у ALL
                string allPath = Path.Combine(tempPath, "ALL");
                Directory.Move(folderPath2, allPath);
                Console.WriteLine($"Перейменовано папку: {folderPath2} -> {allPath}");
            }
            else
            {
                Console.WriteLine($"Папки {folderPath2} не існує.");
            }


            // Вилучення папки прізвище_студента1
            Directory.Delete(folderPath1, true);
            Console.WriteLine($"Вилучено папку: {folderPath1}");

            // Виведення повної інформації про файли у папці ALL, якщо вона існує
            string allPathInfo = Path.Combine(tempPath, "ALL");
            if (Directory.Exists(allPathInfo))
            {
                Console.WriteLine("Повна інформація про файли у папці ALL:");
                string[] allFiles = Directory.GetFiles(allPathInfo);
                foreach (string file in allFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);
                    Console.WriteLine($"Шлях: {fileInfo.FullName}");
                    Console.WriteLine($"Розмір: {fileInfo.Length} байт");
                    Console.WriteLine($"Дата створення: {fileInfo.CreationTime}");
                    Console.WriteLine($"Дата останньої зміни: {fileInfo.LastWriteTime}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine($"Папки {allPathInfo} не існує.");
            }
        }
    }
}



