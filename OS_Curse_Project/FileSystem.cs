using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace OS_Curse_Project
{
    /// <summary>
    ///     Класс для работы с файлами
    /// </summary>
    internal static class FileSystem
    {
        /// <summary>
        ///     Экспортирует данные в формат PDF
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="bitmap">График </param>
        /// <param name="results">Результаты анализа алгоритмов</param>
        public static void exportPdf(string path, byte[] bitmap, Dictionary<string, Dictionary<int, List<List<char>>>> results)
        {
            var writer = new PdfWriter(path);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);
            var FONT_FILENAME = "../../resources/Times_New_Roman.ttf";
            var font = PdfFontFactory.CreateFont(FONT_FILENAME, PdfEncodings.IDENTITY_H);
            var header = new Paragraph("Анализ методов замещения страниц").SetFont(font)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(20);
            document.SetFont(font);
            var image = new Image(ImageDataFactory.Create(bitmap)).SetTextAlignment(TextAlignment.CENTER);
            var widthscaler = (document.GetPageEffectiveArea(PageSize.A4).GetWidth() - document.GetLeftMargin() - document.GetRightMargin()) / image.GetImageWidth();
            var heighscaler = (document.GetPageEffectiveArea(PageSize.A4).GetHeight() - document.GetTopMargin() - document.GetBottomMargin()) / image.GetImageHeight();
            float scaler;
            if (widthscaler < heighscaler)
            {
                scaler = widthscaler;
            }
            else
            {
                scaler = heighscaler;
            }

            //Trace.WriteLine(scaler);
            //image.Scale(0.2f, 0.2f);
            image.Scale(scaler, scaler);
            document.Add(header);
            document.Add(image);
            foreach (var alg in results)
            {
                foreach (var result in alg.Value)
                {
                    var countOfCells = result.Key + 2;
                    var table = new Table(UnitValue.CreatePercentArray(countOfCells)).UseAllAvailableWidth();
                    document.Add(new Paragraph($"Алгоритм — {alg.Key}, количество страничных блоков — {result.Key}"));
                    table.AddHeaderCell("Прерывание");
                    table.AddHeaderCell("Добавленный блок");
                    for (var i = 1; i < countOfCells - 1; i++)
                    {
                        table.AddHeaderCell($"С{i}");
                    }

                    foreach (var chars in result.Value)
                    {
                        var cells = new List<Cell>();
                        for (var i = 0; i < countOfCells; i++)
                        {
                            cells.Add(new Cell());
                        }

                        for (var i = 0; i < chars.Count; i++)
                        {
                            cells[i].Add(new Paragraph(chars[i].ToString()));
                        }

                        foreach (var cell in cells)
                        {
                            table.AddCell(cell);
                        }
                    }

                    document.Add(table);
                }
            }

            document.Close();
        }


        /// <summary>
        ///     Сохраняет данные в файл
        /// </summary>
        /// <param name="path"> Путь к файлу</param>
        /// <param name="text">Данные для записи</param>
        public static void SaveToFile(string path, string text)
        {
            var fileWriter = new StreamWriter(path);
            fileWriter.WriteLine(text);
            fileWriter.Close();
        }


        /// <summary>
        ///     Читает данные из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns> Полученные из файла данные</returns>
        public static List<string> ReadFromFile(string path)
        {
            string[] stringSeparators = {"\r", "\n", "\t"};
            var lines = File.ReadAllText(path)
                .Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries) // разбиваем строку по указанным символам
                .ToList(); // преобразуем результат в список

            return lines;
        }
    }
}