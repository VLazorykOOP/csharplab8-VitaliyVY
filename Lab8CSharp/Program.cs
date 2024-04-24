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
                        DisplayFileInfo();
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
            string ipPattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";

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
        static void DisplayFileInfo()
        {
            Console.WriteLine("Інформація про створені файли:");

            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                Console.WriteLine($"Назва файлу: {fileInfo.Name}");
                Console.WriteLine($"Розмір файлу: {fileInfo.Length} байт");
                Console.WriteLine($"Дата створення: {fileInfo.CreationTime}");
                Console.WriteLine($"Дата останньої модифікації: {fileInfo.LastWriteTime}");
                Console.WriteLine();
            }
        }
    }
}
