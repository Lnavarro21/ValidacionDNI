using System.Text.Json;
using ValidacionDNI_Backend.DataAccess;
using ValidacionDNI_Backend.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using System;
using System.IO;
using System.Threading.Tasks;
using iText.IO.Image;
using iText.Layout.Borders;
namespace ValidacionDNI_Backend.BusinessLogic
{
    public class ValidacionDNIHandler
    {
        private ValidacionDNIDAO vgDataAccess;
        private beMySettings vgSettings;
        private readonly IWebHostEnvironment _env;
        public ValidacionDNIHandler(beMySettings peSettings, IWebHostEnvironment env)
        {
            this.vgSettings = peSettings;
            vgDataAccess = new ValidacionDNIDAO(vgSettings.DbConnection);
            _env = env;
        }

        public async Task<MensajeRespuesta> RegistrarDNIAsync(PostulanteDTO postulante)
        {
            try
            {
                
                MensajeRespuesta Respuesta = null;

                Respuesta = await vgDataAccess.RegistrarDNI(postulante);

                return Respuesta;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<TipoDocumentoLista> ListaDocumentoAsync()
        {
            try
            {

                TipoDocumentoLista Respuesta = null;

                Respuesta = await vgDataAccess.ListaDocumento();

                return Respuesta;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ModalidadTipoLista> ModalidadListaAsync()
        {
            try
            {

                ModalidadTipoLista Respuesta = null;

                Respuesta = await vgDataAccess.ModalidadLista();

                return Respuesta;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<SedesLista> SedeListaAsync()
        {
            try
            {

                SedesLista Respuesta = null;

                Respuesta = await vgDataAccess.SedeLista();

                return Respuesta;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<EscuelasLista> EscuelaListaAsync()
        {
            try
            {

                EscuelasLista Respuesta = null;

                Respuesta = await vgDataAccess.EscuelaLista();

                return Respuesta;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<byte[]> GenerarReciboPDFAsync(ReciboRequest data)
        {
            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            // Logo
            var logoUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQcmB6wzZVYIYWcH9QVOF5N6uWk2C8x6ydCrw&s";
            using var client = new HttpClient();
            var imgBytes = await client.GetByteArrayAsync(logoUrl);
            var imgData = iText.IO.Image.ImageDataFactory.Create(imgBytes);
            var logo = new iText.Layout.Element.Image(imgData).SetHorizontalAlignment(HorizontalAlignment.CENTER).ScaleToFit(200, 200);
            document.Add(logo);

            // Título
            document.Add(new Paragraph("\nUNIVERSIDAD DE SAN MARTIN DE PORRES")
                .SetFont(fontBold).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("R.U.C. 20138149022")
                .SetFont(fontNormal).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph("AV. CIRCUNVAL.CL.GF. LOS INCAS NRO 154 LIMA – LIMA – SANTIAGO DE SURCO")
                .SetFont(fontNormal).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph($"Facultad: ---\n\nEscuela: ---\n\nPrograma: ---")
                .SetFont(fontNormal).SetFontSize(9).SetTextAlignment(TextAlignment.CENTER));

            document.Add(new Paragraph("RECIBO")
                .SetFont(fontNormal).SetFontSize(11).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(10));

            document.Add(new Paragraph("CÓDIGO CLIENTE")
                .SetFont(fontNormal).SetTextAlignment(TextAlignment.CENTER));
            document.Add(new Paragraph($"{data.NumeroDocumento}")
                .SetFont(fontNormal).SetTextAlignment(TextAlignment.CENTER));

            var now = DateTime.Now;
            var vencimiento = now.AddMonths(1);

            var fechaHoraTable = new Table(UnitValue.CreatePercentArray(new float[] { 1.5f, 1.5f, 1.5f, 1.5f }))
             .SetWidth(UnitValue.CreatePercentValue(80))
             .SetHorizontalAlignment(HorizontalAlignment.CENTER)
             .SetBorder(Border.NO_BORDER)
             .SetMarginBottom(10);

            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("Fecha:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph($"{now:dd/MM/yyyy}").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("INTERBANK").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("Hora:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph($"{now:HH:mm:ss}").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("CRÉDITO:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("Fecha Ven:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph($"{vencimiento:dd/MM/yyyy}").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("CONTINENTAL:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal)).SetBorder(Border.NO_BORDER));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("BIF:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal)).SetBorder(Border.NO_BORDER));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("SCOTIABANK:").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
            fechaHoraTable.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

            document.Add(fechaHoraTable);


            // Tabla de concepto
            var table = new Table(new float[] { 1, 3, 1 })
                .SetWidth(350);
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            table.SetFontSize(10);
            table.AddHeaderCell(new Cell().Add(new Paragraph("Semestre").SetFont(fontNormal)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Concepto/Descripción").SetFont(fontNormal)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Monto").SetFont(fontNormal)));

            table.AddCell(new Cell().Add(new Paragraph("00000").SetFont(fontNormal)));
            table.AddCell(new Cell().Add(new Paragraph("Examen de Admisión").SetFont(fontNormal)));
            table.AddCell(new Cell().Add(new Paragraph("350.0").SetFont(fontNormal)));

            table.AddCell(new Cell().Add(new Paragraph("").SetFont(fontNormal)));
            table.AddCell(new Cell().Add(new Paragraph("TOTAL").SetFont(fontNormal)));
            table.AddCell(new Cell().Add(new Paragraph("S/ 350.0").SetFont(fontNormal)));
            document.Add(table);

            document.Add(new Paragraph("Local: WEB")
                .SetFont(fontNormal).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).SetMarginTop(10));

            // Código de barras (mock - si usas datos reales, usa Barcode128)
            var barcode = new iText.Barcodes.Barcode128(pdf);
            barcode.SetCodeType(iText.Barcodes.Barcode128.CODE128);
            var barcodeImage = new iText.Layout.Element.Image(barcode.CreateFormXObject(pdf)).SetWidth(150).SetHeight(50).SetHorizontalAlignment(HorizontalAlignment.CENTER);
            document.Add(barcodeImage);

            document.Close();
            return stream.ToArray();
        }

    }
}
