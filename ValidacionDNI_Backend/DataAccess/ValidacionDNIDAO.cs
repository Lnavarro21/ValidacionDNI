using ValidacionDNI_Backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace ValidacionDNI_Backend.DataAccess
{
    public class ValidacionDNIDAO
    {
        private ConexionDAO vgBDConeccion;
        private SqlConnection oConn = new SqlConnection();
        private SqlTransaction oTran = null;
        private readonly IConfiguration _configuration;

        public ValidacionDNIDAO(string peDbConection)
        {
            vgBDConeccion = new ConexionDAO(peDbConection);
        }

        public async Task<MensajeRespuesta> RegistrarDNI(PostulanteDTO postulante)
        {
            var result = new MensajeRespuesta();

            try
            {               
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "Postulante_INS";

                    oCmC.Parameters.AddWithValue("@vchDNI", postulante.DNI);
                    oCmC.Parameters.AddWithValue("@vchApellidoPaterno", postulante.ApellidoPaterno);
                    oCmC.Parameters.AddWithValue("@vchApellidoMaterno", postulante.ApellidoMaterno);
                    oCmC.Parameters.AddWithValue("@vchNombres", postulante.Nombres);
                    oCmC.Parameters.AddWithValue("@vchEmail", postulante.Email);
                    oCmC.Parameters.AddWithValue("@vchCelular", postulante.Celular);


                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;
                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            result = new MensajeRespuesta()
                            {
                                Mensaje = oSqlR["Mensaje"] != DBNull.Value ? Convert.ToString(oSqlR["Mensaje"]) : string.Empty,
                                IdMensaje = oSqlR["IdMensaje"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdMensaje"]) : 0,
                                IdTipoMensaje = oSqlR["TipoMensaje"] != DBNull.Value ? Convert.ToInt32(oSqlR["TipoMensaje"]) : 0,
                            };
                        }
                    }
                    oTran.Commit();
                }
            }
            catch (Exception ex)
            {
                // Contrucción de Salida
                if (oTran != null)
                {
                    await Task.Run(() => oTran.Rollback());
                }
                throw new Exception(ex.Message);
            }
            finally
            {
                if (oTran != null)
                {
                    await oTran.DisposeAsync();
                    await oConn.DisposeAsync();
                    vgBDConeccion.Dispose();
                }
            }
            return result;
        }

    }
}
