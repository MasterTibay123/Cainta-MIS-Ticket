using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Cainta_MIS_Ticket
{
    public class PdfExporter
    {
        public void ExportDataGridViewToPdf(DataGridView dgvTicketEncoder, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Exported DataGridView";

                // Create a new page
                PdfPage pdfPage = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                // Set fonts
                XFont font = new XFont("Arial", 10);
                XFont wrappedFont = new XFont("Arial", 8); // Smaller font for wrapped text

                // Define starting points, margins, and padding
                int yPoint = 40; // Starting Y position
                int xPoint = 40; // Starting X position
                int rowHeight = 30; // Height of each row (increased for padding)
                int cellPadding = 5; // Padding inside each cell
                int imageSize = 30; // Fixed size for image
                int tableWidth = 0;

                // Determine the first and last columns to hide
                int firstColumnIndex = 0;
                int lastColumnIndex = dgvTicketEncoder.Columns.Count - 1;

                // Calculate the width of the table based on the DataGridView columns, excluding hidden columns
                foreach (DataGridViewColumn column in dgvTicketEncoder.Columns)
                {
                    if (column.Index != firstColumnIndex && column.Index != lastColumnIndex)
                    {
                        tableWidth += column.Width;
                    }
                }

                // Get the page width and set the right margin
                int pageWidth = (int)pdfPage.Width.Point;
                int rightMargin = 40; // Right margin
                double scaleFactor = (tableWidth > (pageWidth - 2 * xPoint))
                    ? (double)(pageWidth - 2 * xPoint) / tableWidth
                    : 1.0;

                // Define colors
                XColor headerColor = XColor.FromArgb(0, 77, 128);
                XBrush headerBrush = new XSolidBrush(headerColor);
                XColor alternatingRowColor = XColor.FromArgb(240, 240, 240);
                XBrush alternatingRowBrush = new XSolidBrush(alternatingRowColor);
                XColor defaultRowColor = XColor.FromArgb(255, 255, 255);
                XBrush defaultRowBrush = new XSolidBrush(defaultRowColor);

                // Center headers
                xPoint = 40; // Reset X position
                foreach (DataGridViewColumn column in dgvTicketEncoder.Columns)
                {
                    if (column.Index == firstColumnIndex || column.Index == lastColumnIndex)
                        continue;

                    int columnWidth = (int)(column.Width * scaleFactor);

                    // Draw header background
                    gfx.DrawRectangle(headerBrush, xPoint, yPoint, columnWidth, rowHeight);

                    // Draw header text centered with padding
                    gfx.DrawString(column.HeaderText, font, XBrushes.White,
                        new XRect(xPoint + cellPadding, yPoint + cellPadding,
                                  columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });

                    xPoint += columnWidth;
                }

                yPoint += rowHeight; // Move to the next row

                // Draw rows with alternating colors
                for (int rowIndex = 0; rowIndex < dgvTicketEncoder.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dgvTicketEncoder.Rows[rowIndex];
                    xPoint = 40; // Reset X position for each row

                    // Determine row color
                    XBrush rowBrush = (rowIndex % 2 == 0) ? defaultRowBrush : alternatingRowBrush;

                    // Draw row background
                    gfx.DrawRectangle(rowBrush, 40, yPoint, tableWidth, rowHeight);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Index == firstColumnIndex || cell.OwningColumn.Index == lastColumnIndex)
                            continue;

                        int columnWidth = (int)(dgvTicketEncoder.Columns[cell.ColumnIndex].Width * scaleFactor);

                        // Check if the cell contains an image or a byte array
                        if (cell.Value is Image img)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (XImage xImg = XImage.FromStream(ms))
                                {
                                    // Ensure image fits within cell width, maintaining aspect ratio
                                    double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                    int imageHeight = (int)(imageSize * aspectRatio);
                                    int imgX = xPoint + (columnWidth - imageSize) / 2;
                                    int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                    gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                }
                            }
                        }
                        else if (cell.Value is byte[] byteArray)
                        {
                            using (MemoryStream ms = new MemoryStream(byteArray))
                            {
                                using (Image byteImage = Image.FromStream(ms))
                                {
                                    using (MemoryStream imageStream = new MemoryStream())
                                    {
                                        byteImage.Save(imageStream, byteImage.RawFormat);
                                        imageStream.Seek(0, SeekOrigin.Begin);
                                        using (XImage xImg = XImage.FromStream(imageStream))
                                        {
                                            // Ensure image fits within cell width, maintaining aspect ratio
                                            double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                            int imageHeight = (int)(imageSize * aspectRatio);
                                            int imgX = xPoint + (columnWidth - imageSize) / 2;
                                            int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                            gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Measure and wrap text if needed
                            string cellText = cell.Value?.ToString() ?? "";
                            XSize textSize = gfx.MeasureString(cellText, wrappedFont);
                            int textX = xPoint + cellPadding;
                            int textY = yPoint + cellPadding;

                            if (textSize.Width > columnWidth - 2 * cellPadding)
                            {
                                // Text needs to be wrapped
                                string[] words = cellText.Split(' ');
                                string currentLine = "";
                                int lineCount = 0;

                                foreach (string word in words)
                                {
                                    string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
                                    textSize = gfx.MeasureString(testLine, wrappedFont);

                                    if (textSize.Width < columnWidth - 2 * cellPadding)
                                    {
                                        currentLine = testLine;
                                    }
                                    else
                                    {
                                        // Draw the current line and move to the next
                                        gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                            new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                            new XStringFormat
                                            {
                                                Alignment = XStringAlignment.Center,
                                                LineAlignment = XLineAlignment.Center
                                            });
                                        currentLine = word;
                                        lineCount++;
                                    }
                                }

                                // Draw the last line
                                gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                            else
                            {
                                // No wrapping needed, draw the text centered with padding
                                gfx.DrawString(cellText, font, XBrushes.Black,
                                    new XRect(textX, textY, columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                        }

                        xPoint += columnWidth;
                    }

                    yPoint += rowHeight; // Move to the next row
                }

                // Save the PDF file
                pdfDoc.Save(filePath);
                MessageBox.Show("PDF file created successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while exporting to PDF: " + ex.Message, "Error");
            }
        }

        public void ExportDataGridViewToPdfTech(DataGridView dgvTicketTech, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Exported DataGridView";

                // Create a new page
                PdfPage pdfPage = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                // Set fonts
                XFont font = new XFont("Arial", 9);
                XFont wrappedFont = new XFont("Arial", 7); // Smaller font for wrapped text

                // Define starting points, margins, and padding
                int yPoint = 40; // Starting Y position
                int xPoint = 40; // Starting X position
                int rowHeight = 30; // Height of each row (increased for padding)
                int cellPadding = 5; // Padding inside each cell
                int imageSize = 30; // Fixed size for image
                int tableWidth = 0;

                // Determine the first and last columns to hide
                int firstColumnIndex = 0;
                int lastColumnIndex = dgvTicketTech.Columns.Count - 1;

                // Calculate the width of the table based on the DataGridView columns, excluding hidden columns
                foreach (DataGridViewColumn column in dgvTicketTech.Columns)
                {
                    if (column.Index != firstColumnIndex && column.Index != lastColumnIndex)
                    {
                        tableWidth += column.Width;
                    }
                }

                // Get the page width and set the right margin
                int pageWidth = (int)pdfPage.Width.Point;
                int rightMargin = 40; // Right margin
                double scaleFactor = (tableWidth > (pageWidth - 2 * xPoint))
                    ? (double)(pageWidth - 2 * xPoint) / tableWidth
                    : 1.0;

                // Define colors
                XColor headerColor = XColor.FromArgb(0, 77, 128);
                XBrush headerBrush = new XSolidBrush(headerColor);
                XColor alternatingRowColor = XColor.FromArgb(240, 240, 240);
                XBrush alternatingRowBrush = new XSolidBrush(alternatingRowColor);
                XColor defaultRowColor = XColor.FromArgb(255, 255, 255);
                XBrush defaultRowBrush = new XSolidBrush(defaultRowColor);

                // Center headers
                xPoint = 40; // Reset X position
                foreach (DataGridViewColumn column in dgvTicketTech.Columns)
                {
                    if (column.Index == firstColumnIndex || column.Index == lastColumnIndex)
                        continue;

                    int columnWidth = (int)(column.Width * scaleFactor);

                    // Draw header background
                    gfx.DrawRectangle(headerBrush, xPoint, yPoint, columnWidth, rowHeight);

                    // Draw header text centered with padding
                    gfx.DrawString(column.HeaderText, font, XBrushes.White,
                        new XRect(xPoint + cellPadding, yPoint + cellPadding,
                                  columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });

                    xPoint += columnWidth;
                }

                yPoint += rowHeight; // Move to the next row

                // Draw rows with alternating colors
                for (int rowIndex = 0; rowIndex < dgvTicketTech.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dgvTicketTech.Rows[rowIndex];
                    xPoint = 40; // Reset X position for each row

                    // Determine row color
                    XBrush rowBrush = (rowIndex % 2 == 0) ? defaultRowBrush : alternatingRowBrush;

                    // Draw row background
                    gfx.DrawRectangle(rowBrush, 40, yPoint, tableWidth, rowHeight);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Index == firstColumnIndex || cell.OwningColumn.Index == lastColumnIndex)
                            continue;

                        int columnWidth = (int)(dgvTicketTech.Columns[cell.ColumnIndex].Width * scaleFactor);

                        // Check if the cell contains an image or a byte array
                        if (cell.Value is Image img)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (XImage xImg = XImage.FromStream(ms))
                                {
                                    // Ensure image fits within cell width, maintaining aspect ratio
                                    double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                    int imageHeight = (int)(imageSize * aspectRatio);
                                    int imgX = xPoint + (columnWidth - imageSize) / 2;
                                    int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                    gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                }
                            }
                        }
                        else if (cell.Value is byte[] byteArray)
                        {
                            using (MemoryStream ms = new MemoryStream(byteArray))
                            {
                                using (Image byteImage = Image.FromStream(ms))
                                {
                                    using (MemoryStream imageStream = new MemoryStream())
                                    {
                                        byteImage.Save(imageStream, byteImage.RawFormat);
                                        imageStream.Seek(0, SeekOrigin.Begin);
                                        using (XImage xImg = XImage.FromStream(imageStream))
                                        {
                                            // Ensure image fits within cell width, maintaining aspect ratio
                                            double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                            int imageHeight = (int)(imageSize * aspectRatio);
                                            int imgX = xPoint + (columnWidth - imageSize) / 2;
                                            int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                            gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Measure and wrap text if needed
                            string cellText = cell.Value?.ToString() ?? "";
                            XSize textSize = gfx.MeasureString(cellText, wrappedFont);
                            int textX = xPoint + cellPadding;
                            int textY = yPoint + cellPadding;

                            if (textSize.Width > columnWidth - 2 * cellPadding)
                            {
                                // Text needs to be wrapped
                                string[] words = cellText.Split(' ');
                                string currentLine = "";
                                int lineCount = 0;

                                foreach (string word in words)
                                {
                                    string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
                                    textSize = gfx.MeasureString(testLine, wrappedFont);

                                    if (textSize.Width < columnWidth - 2 * cellPadding)
                                    {
                                        currentLine = testLine;
                                    }
                                    else
                                    {
                                        // Draw the current line and move to the next
                                        gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                            new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                            new XStringFormat
                                            {
                                                Alignment = XStringAlignment.Center,
                                                LineAlignment = XLineAlignment.Center
                                            });
                                        currentLine = word;
                                        lineCount++;
                                    }
                                }

                                // Draw the last line
                                gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                            else
                            {
                                // No wrapping needed, draw the text centered with padding
                                gfx.DrawString(cellText, font, XBrushes.Black,
                                    new XRect(textX, textY, columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                        }

                        xPoint += columnWidth;
                    }

                    yPoint += rowHeight; // Move to the next row
                }

                // Save the PDF file
                pdfDoc.Save(filePath);
                MessageBox.Show("PDF file created successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while exporting to PDF: " + ex.Message, "Error");
            }
        }

        public void ExportDataGridViewToPdfFilesIn(DataGridView dgvFilesIn, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Exported DataGridView";

                // Create a new page
                PdfPage pdfPage = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                // Set fonts
                XFont font = new XFont("Arial", 10);
                XFont wrappedFont = new XFont("Arial", 8); // Smaller font for wrapped text

                // Define starting points, margins, and padding
                int yPoint = 40; // Starting Y position
                int xPoint = 40; // Starting X position
                int rowHeight = 30; // Height of each row (increased for padding)
                int cellPadding = 5; // Padding inside each cell
                int imageSize = 30; // Fixed size for image
                int tableWidth = 0;

                // Determine the columns to include (excluding the first column)
                foreach (DataGridViewColumn column in dgvFilesIn.Columns)
                {
                    if (column.Index != 0) // Exclude the first column
                    {
                        tableWidth += column.Width;
                    }
                }

                // Get the page width and set the right margin
                int pageWidth = (int)pdfPage.Width.Point;
                int rightMargin = 40; // Right margin
                double scaleFactor = (tableWidth > (pageWidth - 2 * xPoint))
                    ? (double)(pageWidth - 2 * xPoint) / tableWidth
                    : 1.0;

                // Define colors
                XColor headerColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush headerBrush = new XSolidBrush(headerColor);
                XColor headerTextColor = XColor.FromArgb(0, 0, 0); // Black text
                XBrush headerTextBrush = new XSolidBrush(headerTextColor);
                XColor alternatingRowColor = XColor.FromArgb(255, 255, 255); // White
                XBrush alternatingRowBrush = new XSolidBrush(alternatingRowColor);
                XColor defaultRowColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush defaultRowBrush = new XSolidBrush(defaultRowColor);

                // Center headers
                xPoint = 40; // Reset X position
                foreach (DataGridViewColumn column in dgvFilesIn.Columns)
                {
                    if (column.Index == 0) // Skip the first column
                        continue;

                    int columnWidth = (int)(column.Width * scaleFactor);

                    // Draw header background
                    gfx.DrawRectangle(headerBrush, xPoint, yPoint, columnWidth, rowHeight);

                    // Draw header text centered with padding
                    gfx.DrawString(column.HeaderText, font, headerTextBrush,
                        new XRect(xPoint + cellPadding, yPoint + cellPadding,
                                  columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });

                    xPoint += columnWidth;
                }

                yPoint += rowHeight; // Move to the next row

                // Draw rows with alternating colors
                for (int rowIndex = 0; rowIndex < dgvFilesIn.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dgvFilesIn.Rows[rowIndex];
                    xPoint = 40; // Reset X position for each row

                    // Determine row color
                    XBrush rowBrush = (rowIndex % 2 == 0) ? alternatingRowBrush : defaultRowBrush;

                    // Draw row background
                    gfx.DrawRectangle(rowBrush, 40, yPoint, tableWidth, rowHeight);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Index == 0) // Skip the first column
                            continue;

                        int columnWidth = (int)(dgvFilesIn.Columns[cell.ColumnIndex].Width * scaleFactor);

                        // Check if the cell contains an image or a byte array
                        if (cell.Value is Image img)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (XImage xImg = XImage.FromStream(ms))
                                {
                                    // Ensure image fits within cell width, maintaining aspect ratio
                                    double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                    int imageHeight = (int)(imageSize * aspectRatio);
                                    int imgX = xPoint + (columnWidth - imageSize) / 2;
                                    int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                    gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                }
                            }
                        }
                        else if (cell.Value is byte[] byteArray)
                        {
                            using (MemoryStream ms = new MemoryStream(byteArray))
                            {
                                using (Image byteImage = Image.FromStream(ms))
                                {
                                    using (MemoryStream imageStream = new MemoryStream())
                                    {
                                        byteImage.Save(imageStream, byteImage.RawFormat);
                                        imageStream.Seek(0, SeekOrigin.Begin);
                                        using (XImage xImg = XImage.FromStream(imageStream))
                                        {
                                            // Ensure image fits within cell width, maintaining aspect ratio
                                            double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                            int imageHeight = (int)(imageSize * aspectRatio);
                                            int imgX = xPoint + (columnWidth - imageSize) / 2;
                                            int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                            gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Measure and wrap text if needed
                            string cellText = cell.Value?.ToString() ?? "";
                            XSize textSize = gfx.MeasureString(cellText, wrappedFont);
                            int textX = xPoint + cellPadding;
                            int textY = yPoint + cellPadding;

                            if (textSize.Width > columnWidth - 2 * cellPadding)
                            {
                                // Text needs to be wrapped
                                string[] words = cellText.Split(' ');
                                string currentLine = "";
                                int lineCount = 0;

                                foreach (string word in words)
                                {
                                    string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
                                    textSize = gfx.MeasureString(testLine, wrappedFont);

                                    if (textSize.Width < columnWidth - 2 * cellPadding)
                                    {
                                        currentLine = testLine;
                                    }
                                    else
                                    {
                                        // Draw the current line and move to the next
                                        gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                            new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                            new XStringFormat
                                            {
                                                Alignment = XStringAlignment.Center,
                                                LineAlignment = XLineAlignment.Center
                                            });
                                        currentLine = word;
                                        lineCount++;
                                    }
                                }

                                // Draw the last line
                                gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                            else
                            {
                                // No wrapping needed, draw text normally
                                gfx.DrawString(cellText, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY, columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                        }

                        xPoint += columnWidth; // Move to the next column
                    }

                    yPoint += rowHeight; // Move to the next row
                }

                // Save the PDF file
                pdfDoc.Save(filePath);
                MessageBox.Show("PDF file created successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting to PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportDataGridViewToPdfFilesOut(DataGridView dgvFilesOut, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Exported DataGridView";

                // Create a new page
                PdfPage pdfPage = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                // Set fonts
                XFont font = new XFont("Arial", 10);
                XFont wrappedFont = new XFont("Arial", 8); // Smaller font for wrapped text

                // Define starting points, margins, and padding
                int yPoint = 40; // Starting Y position
                int xPoint = 40; // Starting X position
                int rowHeight = 30; // Height of each row (increased for padding)
                int cellPadding = 5; // Padding inside each cell
                int imageSize = 30; // Fixed size for image
                int tableWidth = 0;

                // Determine the columns to include (excluding the first column)
                foreach (DataGridViewColumn column in dgvFilesOut.Columns)
                {
                    if (column.Index != 0) // Exclude the first column
                    {
                        tableWidth += column.Width;
                    }
                }

                // Get the page width and set the right margin
                int pageWidth = (int)pdfPage.Width.Point;
                int rightMargin = 40; // Right margin
                double scaleFactor = (tableWidth > (pageWidth - 2 * xPoint))
                    ? (double)(pageWidth - 2 * xPoint) / tableWidth
                    : 1.0;

                // Define colors
                XColor headerColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush headerBrush = new XSolidBrush(headerColor);
                XColor headerTextColor = XColor.FromArgb(0, 0, 0); // Black text
                XBrush headerTextBrush = new XSolidBrush(headerTextColor);
                XColor alternatingRowColor = XColor.FromArgb(255, 255, 255); // White
                XBrush alternatingRowBrush = new XSolidBrush(alternatingRowColor);
                XColor defaultRowColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush defaultRowBrush = new XSolidBrush(defaultRowColor);

                // Center headers
                xPoint = 40; // Reset X position
                foreach (DataGridViewColumn column in dgvFilesOut.Columns)
                {
                    if (column.Index == 0) // Skip the first column
                        continue;

                    int columnWidth = (int)(column.Width * scaleFactor);

                    // Draw header background
                    gfx.DrawRectangle(headerBrush, xPoint, yPoint, columnWidth, rowHeight);

                    // Draw header text centered with padding
                    gfx.DrawString(column.HeaderText, font, headerTextBrush,
                        new XRect(xPoint + cellPadding, yPoint + cellPadding,
                                  columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });

                    xPoint += columnWidth;
                }

                yPoint += rowHeight; // Move to the next row

                // Draw rows with alternating colors
                for (int rowIndex = 0; rowIndex < dgvFilesOut.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dgvFilesOut.Rows[rowIndex];
                    xPoint = 40; // Reset X position for each row

                    // Determine row color
                    XBrush rowBrush = (rowIndex % 2 == 0) ? alternatingRowBrush : defaultRowBrush;

                    // Draw row background
                    gfx.DrawRectangle(rowBrush, 40, yPoint, tableWidth, rowHeight);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Index == 0) // Skip the first column
                            continue;

                        int columnWidth = (int)(dgvFilesOut.Columns[cell.ColumnIndex].Width * scaleFactor);

                        // Check if the cell contains an image or a byte array
                        if (cell.Value is Image img)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (XImage xImg = XImage.FromStream(ms))
                                {
                                    // Ensure image fits within cell width, maintaining aspect ratio
                                    double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                    int imageHeight = (int)(imageSize * aspectRatio);
                                    int imgX = xPoint + (columnWidth - imageSize) / 2;
                                    int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                    gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                }
                            }
                        }
                        else if (cell.Value is byte[] byteArray)
                        {
                            using (MemoryStream ms = new MemoryStream(byteArray))
                            {
                                using (Image byteImage = Image.FromStream(ms))
                                {
                                    using (MemoryStream imageStream = new MemoryStream())
                                    {
                                        byteImage.Save(imageStream, byteImage.RawFormat);
                                        imageStream.Seek(0, SeekOrigin.Begin);
                                        using (XImage xImg = XImage.FromStream(imageStream))
                                        {
                                            // Ensure image fits within cell width, maintaining aspect ratio
                                            double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                            int imageHeight = (int)(imageSize * aspectRatio);
                                            int imgX = xPoint + (columnWidth - imageSize) / 2;
                                            int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                            gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Measure and wrap text if needed
                            string cellText = cell.Value?.ToString() ?? "";
                            XSize textSize = gfx.MeasureString(cellText, wrappedFont);
                            int textX = xPoint + cellPadding;
                            int textY = yPoint + cellPadding;

                            if (textSize.Width > columnWidth - 2 * cellPadding)
                            {
                                // Text needs to be wrapped
                                string[] words = cellText.Split(' ');
                                string currentLine = "";
                                int lineCount = 0;

                                foreach (string word in words)
                                {
                                    string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
                                    textSize = gfx.MeasureString(testLine, wrappedFont);

                                    if (textSize.Width < columnWidth - 2 * cellPadding)
                                    {
                                        currentLine = testLine;
                                    }
                                    else
                                    {
                                        // Draw the current line and move to the next
                                        gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                            new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                            new XStringFormat
                                            {
                                                Alignment = XStringAlignment.Center,
                                                LineAlignment = XLineAlignment.Center
                                            });
                                        currentLine = word;
                                        lineCount++;
                                    }
                                }

                                // Draw the last line
                                gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                            else
                            {
                                // No wrapping needed, draw text normally
                                gfx.DrawString(cellText, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY, columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                        }

                        xPoint += columnWidth; // Move to the next column
                    }

                    yPoint += rowHeight; // Move to the next row
                }

                // Save the PDF file
                pdfDoc.Save(filePath);
                MessageBox.Show("PDF file created successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting to PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportDataGridViewToPdfVol(DataGridView dgvReportDay, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Exported DataGridView";

                // Create a new page
                PdfPage pdfPage = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                // Set fonts
                XFont font = new XFont("Arial", 10);
                XFont wrappedFont = new XFont("Arial", 8); // Smaller font for wrapped text

                // Define starting points, margins, and padding
                int yPoint = 40; // Starting Y position
                int xPoint = 40; // Starting X position
                int rowHeight = 30; // Height of each row (increased for padding)
                int cellPadding = 5; // Padding inside each cell
                int imageSize = 30; // Fixed size for image
                int tableWidth = 0;

                // Calculate the total width of the columns to include (excluding the first column)
                foreach (DataGridViewColumn column in dgvReportDay.Columns)
                {
                    if (column.Visible && column.Index != 0) // Exclude the first column
                    {
                        tableWidth += column.Width;
                    }
                }

                // Get the page width and set the right margin
                int pageWidth = (int)pdfPage.Width.Point;
                int rightMargin = 40; // Right margin
                double scaleFactor = (tableWidth > (pageWidth - 2 * xPoint))
                    ? (double)(pageWidth - 2 * xPoint) / tableWidth
                    : 1.0;

                // Define colors
                XColor headerColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush headerBrush = new XSolidBrush(headerColor);
                XColor headerTextColor = XColor.FromArgb(0, 0, 0); // Black text
                XBrush headerTextBrush = new XSolidBrush(headerTextColor);
                XColor alternatingRowColor = XColor.FromArgb(255, 255, 255); // White
                XBrush alternatingRowBrush = new XSolidBrush(alternatingRowColor);
                XColor defaultRowColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush defaultRowBrush = new XSolidBrush(defaultRowColor);

                // Center headers
                xPoint = 40; // Reset X position
                foreach (DataGridViewColumn column in dgvReportDay.Columns)
                {
                    if (column.Index == 0 || !column.Visible) // Skip the first column and invisible columns
                        continue;

                    int columnWidth = (int)(column.Width * scaleFactor);

                    // Draw header background
                    gfx.DrawRectangle(headerBrush, xPoint, yPoint, columnWidth, rowHeight);

                    // Draw header text centered with padding
                    gfx.DrawString(column.HeaderText, font, headerTextBrush,
                        new XRect(xPoint + cellPadding, yPoint + cellPadding,
                                  columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });

                    xPoint += columnWidth;
                }

                yPoint += rowHeight; // Move to the next row

                // Draw rows with alternating colors
                for (int rowIndex = 0; rowIndex < dgvReportDay.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dgvReportDay.Rows[rowIndex];
                    xPoint = 40; // Reset X position for each row

                    // Determine row color
                    XBrush rowBrush = (rowIndex % 2 == 0) ? alternatingRowBrush : defaultRowBrush;

                    // Draw row background
                    gfx.DrawRectangle(rowBrush, 40, yPoint, tableWidth, rowHeight);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Index == 0 || !cell.OwningColumn.Visible) // Skip the first column and invisible columns
                            continue;

                        int columnWidth = (int)(cell.OwningColumn.Width * scaleFactor);

                        // Check if the cell contains an image or a byte array
                        if (cell.Value is Image img)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (XImage xImg = XImage.FromStream(ms))
                                {
                                    // Ensure image fits within cell width, maintaining aspect ratio
                                    double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                    int imageHeight = (int)(imageSize * aspectRatio);
                                    int imgX = xPoint + (columnWidth - imageSize) / 2;
                                    int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                    gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                }
                            }
                        }
                        else if (cell.Value is byte[] byteArray)
                        {
                            using (MemoryStream ms = new MemoryStream(byteArray))
                            {
                                using (Image byteImage = Image.FromStream(ms))
                                {
                                    using (MemoryStream imageStream = new MemoryStream())
                                    {
                                        byteImage.Save(imageStream, byteImage.RawFormat);
                                        imageStream.Seek(0, SeekOrigin.Begin);
                                        using (XImage xImg = XImage.FromStream(imageStream))
                                        {
                                            // Ensure image fits within cell width, maintaining aspect ratio
                                            double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                            int imageHeight = (int)(imageSize * aspectRatio);
                                            int imgX = xPoint + (columnWidth - imageSize) / 2;
                                            int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                            gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Measure and wrap text if needed
                            string cellText = cell.Value?.ToString() ?? "";
                            XSize textSize = gfx.MeasureString(cellText, wrappedFont);
                            int textX = xPoint + cellPadding;
                            int textY = yPoint + cellPadding;

                            if (textSize.Width > columnWidth - 2 * cellPadding)
                            {
                                // Text needs to be wrapped
                                string[] words = cellText.Split(' ');
                                string currentLine = "";
                                int lineCount = 0;

                                foreach (string word in words)
                                {
                                    string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
                                    textSize = gfx.MeasureString(testLine, wrappedFont);

                                    if (textSize.Width < columnWidth - 2 * cellPadding)
                                    {
                                        currentLine = testLine;
                                    }
                                    else
                                    {
                                        // Draw the current line and move to the next
                                        gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                            new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                            new XStringFormat
                                            {
                                                Alignment = XStringAlignment.Center,
                                                LineAlignment = XLineAlignment.Center
                                            });
                                        currentLine = word;
                                        lineCount++;
                                    }
                                }

                                // Draw the last line
                                gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                            else
                            {
                                // No wrapping needed, draw text normally
                                gfx.DrawString(cellText, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY, columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                        }

                        xPoint += columnWidth; // Move to the next column
                    }

                    yPoint += rowHeight; // Move to the next row
                }

                // Save the PDF file
                pdfDoc.Save(filePath);
                MessageBox.Show("PDF file created successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting to PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ExportDataGridViewToPdfAudit(DataGridView dgvAudit, string filePath)
        {
            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create a new PDF document
                PdfDocument pdfDoc = new PdfDocument();
                pdfDoc.Info.Title = "Exported DataGridView";

                // Create a new page
                PdfPage pdfPage = pdfDoc.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(pdfPage);

                // Set fonts
                XFont font = new XFont("Arial", 10);
                XFont wrappedFont = new XFont("Arial", 8); // Smaller font for wrapped text

                // Define starting points, margins, and padding
                int yPoint = 40; // Starting Y position
                int xPoint = 40; // Starting X position
                int rowHeight = 30; // Height of each row (increased for padding)
                int cellPadding = 5; // Padding inside each cell
                int imageSize = 30; // Fixed size for image
                int tableWidth = 0;

                // Determine the columns to include (excluding the first column)
                foreach (DataGridViewColumn column in dgvAudit.Columns)
                {
                    if (column.Index != 0) // Exclude the first column
                    {
                        tableWidth += column.Width;
                    }
                }

                // Get the page width and set the right margin
                int pageWidth = (int)pdfPage.Width.Point;
                int rightMargin = 40; // Right margin
                double scaleFactor = (tableWidth > (pageWidth - 2 * xPoint))
                    ? (double)(pageWidth - 2 * xPoint) / tableWidth
                    : 1.0;

                // Define colors
                XColor headerColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush headerBrush = new XSolidBrush(headerColor);
                XColor headerTextColor = XColor.FromArgb(0, 0, 0); // Black text
                XBrush headerTextBrush = new XSolidBrush(headerTextColor);
                XColor alternatingRowColor = XColor.FromArgb(255, 255, 255); // White
                XBrush alternatingRowBrush = new XSolidBrush(alternatingRowColor);
                XColor defaultRowColor = XColor.FromArgb(255, 255, 255); // White background
                XBrush defaultRowBrush = new XSolidBrush(defaultRowColor);

                // Center headers
                xPoint = 40; // Reset X position
                foreach (DataGridViewColumn column in dgvAudit.Columns)
                {
                    if (column.Index == 0) // Skip the first column
                        continue;

                    int columnWidth = (int)(column.Width * scaleFactor);

                    // Draw header background
                    gfx.DrawRectangle(headerBrush, xPoint, yPoint, columnWidth, rowHeight);

                    // Draw header text centered with padding
                    gfx.DrawString(column.HeaderText, font, headerTextBrush,
                        new XRect(xPoint + cellPadding, yPoint + cellPadding,
                                  columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                        new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        });

                    xPoint += columnWidth;
                }

                yPoint += rowHeight; // Move to the next row

                // Draw rows with alternating colors
                for (int rowIndex = 0; rowIndex < dgvAudit.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = dgvAudit.Rows[rowIndex];
                    xPoint = 40; // Reset X position for each row

                    // Determine row color
                    XBrush rowBrush = (rowIndex % 2 == 0) ? alternatingRowBrush : defaultRowBrush;

                    // Draw row background
                    gfx.DrawRectangle(rowBrush, 40, yPoint, tableWidth, rowHeight);

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn.Index == 0) // Skip the first column
                            continue;

                        int columnWidth = (int)(dgvAudit.Columns[cell.ColumnIndex].Width * scaleFactor);

                        // Check if the cell contains an image or a byte array
                        if (cell.Value is Image img)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, img.RawFormat);
                                ms.Seek(0, SeekOrigin.Begin);

                                using (XImage xImg = XImage.FromStream(ms))
                                {
                                    // Ensure image fits within cell width, maintaining aspect ratio
                                    double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                    int imageHeight = (int)(imageSize * aspectRatio);
                                    int imgX = xPoint + (columnWidth - imageSize) / 2;
                                    int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                    gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                }
                            }
                        }
                        else if (cell.Value is byte[] byteArray)
                        {
                            using (MemoryStream ms = new MemoryStream(byteArray))
                            {
                                using (Image byteImage = Image.FromStream(ms))
                                {
                                    using (MemoryStream imageStream = new MemoryStream())
                                    {
                                        byteImage.Save(imageStream, byteImage.RawFormat);
                                        imageStream.Seek(0, SeekOrigin.Begin);
                                        using (XImage xImg = XImage.FromStream(imageStream))
                                        {
                                            // Ensure image fits within cell width, maintaining aspect ratio
                                            double aspectRatio = (double)xImg.PixelHeight / xImg.PixelWidth;
                                            int imageHeight = (int)(imageSize * aspectRatio);
                                            int imgX = xPoint + (columnWidth - imageSize) / 2;
                                            int imgY = yPoint + (rowHeight - imageHeight) / 2;

                                            gfx.DrawImage(xImg, imgX, imgY, imageSize, imageHeight);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Measure and wrap text if needed
                            string cellText = cell.Value?.ToString() ?? "";
                            XSize textSize = gfx.MeasureString(cellText, wrappedFont);
                            int textX = xPoint + cellPadding;
                            int textY = yPoint + cellPadding;

                            if (textSize.Width > columnWidth - 2 * cellPadding)
                            {
                                // Text needs to be wrapped
                                string[] words = cellText.Split(' ');
                                string currentLine = "";
                                int lineCount = 0;

                                foreach (string word in words)
                                {
                                    string testLine = currentLine + (currentLine == "" ? "" : " ") + word;
                                    textSize = gfx.MeasureString(testLine, wrappedFont);

                                    if (textSize.Width < columnWidth - 2 * cellPadding)
                                    {
                                        currentLine = testLine;
                                    }
                                    else
                                    {
                                        // Draw the current line and move to the next
                                        gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                            new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                            new XStringFormat
                                            {
                                                Alignment = XStringAlignment.Center,
                                                LineAlignment = XLineAlignment.Center
                                            });
                                        currentLine = word;
                                        lineCount++;
                                    }
                                }

                                // Draw the last line
                                gfx.DrawString(currentLine, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY + (lineCount * wrappedFont.GetHeight()), columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                            else
                            {
                                // No wrapping needed, draw text normally
                                gfx.DrawString(cellText, wrappedFont, XBrushes.Black,
                                    new XRect(textX, textY, columnWidth - 2 * cellPadding, rowHeight - 2 * cellPadding),
                                    new XStringFormat
                                    {
                                        Alignment = XStringAlignment.Center,
                                        LineAlignment = XLineAlignment.Center
                                    });
                            }
                        }

                        xPoint += columnWidth; // Move to the next column
                    }

                    yPoint += rowHeight; // Move to the next row
                }

                // Save the PDF file
                pdfDoc.Save(filePath);
                MessageBox.Show("PDF file created successfully!", "Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting to PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
