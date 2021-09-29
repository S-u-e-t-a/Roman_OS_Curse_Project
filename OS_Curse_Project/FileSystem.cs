﻿using iText.IO.Font;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace OS_Curse_Project
{
    internal static class FileSystem
    {
        public static void test(byte[] bitmap)
        {
            // Must have write permissions to the path folder
            var writer = new PdfWriter("demo.pdf");
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
            document.Close();
        }


        public static void importPdf()
        {
        }
    }
}