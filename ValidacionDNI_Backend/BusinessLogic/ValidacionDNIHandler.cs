using System.Text.Json;
using ValidacionDNI_Backend.DataAccess;
using ValidacionDNI_Backend.Models;
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
    }
}
