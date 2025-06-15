using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidacionDNI_Backend.Models
{
    public class PostulanteDTO
    {
        public string DNI { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Nombres { get; set; }
        public string Email { get; set; }
        public string Celular {  get; set; }
    }

    public class TipoDocumentoLista
    {
        public List<TipoDocumento> Lista { get; set; }
    }
    public class TipoDocumento
    {
        public int IdDocumento { get; set; }
        public string Documento { get; set; }
        public string NombreCortoDocumento { get; set; }
    }

}