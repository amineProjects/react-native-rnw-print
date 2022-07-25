using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

namespace ReactNativeRnwPrint
{
    [ReactModule("RnwPrint")]
    internal sealed class ReactNativePrintModule
    {
        // See https://microsoft.github.io/react-native-windows/docs/native-modules for details on writing native modules
        private Font printFont;
        private StreamReader streamToPrint;

        // The PrintPage event is raised for each page to be printed.
        private void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            String line = null;

            // Calculate the number of lines per page.
            linesPerPage = ev.MarginBounds.Height /
               printFont.GetHeight(ev.Graphics);

            // Iterate over the file, printing each line.
            while (count < linesPerPage &&
               ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black,
                   leftMargin, yPos, new StringFormat());
                count++;
            }

            // If more lines exist, print another page.
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }

        [ReactMethod("print")]
        public void Printing(string filePath)
        {
            try
            {
                streamToPrint = new StreamReader($@"{filePath}");
                try
                {
                    printFont = new Font("Arial", 14);
                    PrintDocument pd = new PrintDocument();
                    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    // Print the document.
                    pd.Print();
                }
                finally
                {
                    streamToPrint.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
