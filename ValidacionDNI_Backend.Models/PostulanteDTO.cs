using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidacionDNI_Backend.Models
{
    public class PostulanteDTO
    {
        public int IdTipoDocumento { get; set; }
        public string Documento { get; set; }
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

    public class ModalidadTipo
    {
        public int IdModalidad { get; set; }
        public string Modalidad { get; set; }
    }

    public class ModalidadTipoLista
    {
        public List<ModalidadTipo> Lista { get; set; }
    }

    public class Sedes
    {
        public int IdSede { get; set; }
        public string Sede { get; set; }
    }
    public class SedesLista
    {
        public List<Sedes> Lista { get; set; }
    }

    public class Escuelas
    {
        public int IdEscuela { get; set; }
        public string Escuela { get; set; }
    }
    public class EscuelasLista
    {
        public List<Escuelas> Lista { get; set; }
    }
        public class ReciboRequest
        {
            public string NumeroDocumento { get; set; }
            public string Nombres { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string Concepto { get; set; }
        }
}