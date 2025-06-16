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

            var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            document.Add(new Paragraph("UNIVERSIDAD DE SAN MARTIN DE PORRES").SetFont(font).SetFontSize(14));
            document.Add(new Paragraph("R.U.C. 20138149022").SetFont(fontNormal));
            document.Add(new Paragraph("AV. CIRCUNVAL.CL.GF. LOS INCAS NRO 154 LIMA – LIMA – SANTIAGO DE SURCO").SetFont(fontNormal).SetMarginBottom(15));

            document.Add(new Paragraph("RECIBO").SetFont(font).SetFontSize(12).SetTextAlignment(TextAlignment.CENTER).SetMarginBottom(10));

            document.Add(new Paragraph($"NÚMERO DE MATRÍCULA: 0000{data.NumeroDocumento}"));
            document.Add(new Paragraph($"NOMBRES: {data.Nombres}"));
            document.Add(new Paragraph($"APELLIDO PATERNO: {data.ApellidoPaterno}"));
            document.Add(new Paragraph($"APELLIDO MATERNO: {data.ApellidoMaterno}"));
            document.Add(new Paragraph($"CONCEPTO: {data.Concepto}"));
            document.Add(new Paragraph($"MONTO: S/ 350.00"));
            document.Add(new Paragraph($"FECHA DE VENCIMIENTO: {DateTime.Now.AddMonths(1):d/M/yyyy}"));
            document.Add(new Paragraph("FACULTAD: ---"));
            document.Add(new Paragraph("ESCUELA: ---"));
            document.Add(new Paragraph("PROGRAMA: ---"));

            document.Add(new Paragraph("\nLos datos ingresados en el formulario, le deben pertenecer única y exclusivamente al postulante. Si los datos ingresados no pertenecen al postulante, corregirlos, caso contrario ignore este mensaje").SetFont(fontNormal).SetFontSize(9));
            document.Add(new Paragraph("\n¿Estás seguro de generar el recibo con los siguientes datos?").SetFont(fontNormal));

            document.Close();
            return stream.ToArray();
        }
    }
}
