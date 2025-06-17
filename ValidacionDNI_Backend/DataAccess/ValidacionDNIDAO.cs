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
                    oCmC.Parameters.AddWithValue("@intIdTipoDocumento", postulante.IdTipoDocumento);
                    oCmC.Parameters.AddWithValue("@vchDocumento", postulante.Documento);
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

        public async Task<TipoDocumentoLista> ListaDocumento()
        {
            TipoDocumentoLista result = new TipoDocumentoLista();
            List<TipoDocumento> Lista = new List<TipoDocumento>();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "TipoDocumento_LST";

                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;

                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            var respuesta = new TipoDocumento()
                            {                                
                                IdDocumento = oSqlR["IdDocumento"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdDocumento"]) : 0,
                                Documento = oSqlR["Documento"] != DBNull.Value ? Convert.ToString(oSqlR["Documento"]) : string.Empty,
                                NombreCortoDocumento = oSqlR["NombreCortoDocumento"] != DBNull.Value ? Convert.ToString(oSqlR["NombreCortoDocumento"]) : string.Empty,
                            };
                            Lista.Add(respuesta);
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
            result.Lista = Lista;
            return result;
        }

        public async Task<ModalidadTipoLista> ModalidadLista()
        {
            ModalidadTipoLista result = new ModalidadTipoLista();
            List<ModalidadTipo> Lista = new List<ModalidadTipo>();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "Modalidad_LST";

                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;

                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            var respuesta = new ModalidadTipo()
                            {
                                IdModalidad = oSqlR["IdModalidad"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdModalidad"]) : 0,
                                Modalidad = oSqlR["Modalidad"] != DBNull.Value ? Convert.ToString(oSqlR["Modalidad"]) : string.Empty,
                            };
                            Lista.Add(respuesta);
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
            result.Lista = Lista;
            return result;
        }

        public async Task<SedesLista> SedeLista()
        {
            SedesLista result = new SedesLista();
            List<Sedes> Lista = new List<Sedes>();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "Sede_LST";

                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;

                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            var respuesta = new Sedes()
                            {
                                IdSede = oSqlR["IdSede"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdSede"]) : 0,
                                Sede = oSqlR["NombreSede"] != DBNull.Value ? Convert.ToString(oSqlR["NombreSede"]) : string.Empty,
                            };
                            Lista.Add(respuesta);
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
            result.Lista = Lista;
            return result;
        }

        public async Task<EscuelasLista> EscuelaLista()
        {
            EscuelasLista result = new EscuelasLista();
            List<Escuelas> Lista = new List<Escuelas>();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "Escuela_LST";

                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;

                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            var respuesta = new Escuelas()
                            {
                                IdEscuela = oSqlR["IdEscuela"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdEscuela"]) : 0,
                                Escuela = oSqlR["NombreEscuela"] != DBNull.Value ? Convert.ToString(oSqlR["NombreEscuela"]) : string.Empty,
                            };
                            Lista.Add(respuesta);
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
            result.Lista = Lista;
            return result;
        }

        public async Task<MensajeRespuesta> RegistrarOpcion1(Opcion1DTO opcion)
        {
            var result = new MensajeRespuesta();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "PostulanteSedeEscuela_INS";
                    oCmC.Parameters.AddWithValue("@vchDocumento", opcion.Documento);
                    oCmC.Parameters.AddWithValue("@intIdSede", opcion.IdSede);
                    oCmC.Parameters.AddWithValue("@intIdEscuela", opcion.IdEscuela);
                    oCmC.Parameters.AddWithValue("@intIdModalidad", opcion.IdModalidad);


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
                                IdTipoMensaje = oSqlR["IdTipoMensaje"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdTipoMensaje"]) : 0,
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

        public async Task<MensajeRespuesta> CompletarRegistro(CompletarRegistroDTO postulante)
        {
            var result = new MensajeRespuesta();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "CompletarRegistro";
                    oCmC.Parameters.AddWithValue("@intIdPostulante", postulante.IdPostulante);
                    oCmC.Parameters.AddWithValue("@intIdGenero", postulante.IdGenero);
                    oCmC.Parameters.AddWithValue("@dtmFechaNacimiento", postulante.FechaNacimiento);
                    oCmC.Parameters.AddWithValue("@vchDireccion", postulante.Direccion);
                    oCmC.Parameters.AddWithValue("@vchColegio3", postulante.Colegio3);
                    oCmC.Parameters.AddWithValue("@vchColegio4", postulante.Colegio4);
                    oCmC.Parameters.AddWithValue("@vchColegio5", postulante.Colegio5);
                    oCmC.Parameters.AddWithValue("@intIdModalidad", postulante.IdModalidad);
                    oCmC.Parameters.AddWithValue("@intIdSede", postulante.IdSede);
                    oCmC.Parameters.AddWithValue("@intIdFacultad", postulante.IdFacultad);
                    oCmC.Parameters.AddWithValue("@intIdEscuela", postulante.IdEscuela);


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

        public async Task<DatosPostulante> Postulante(int IdPostulante)
        {
            var result = new DatosPostulante();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "Postulante_SEL";
                    oCmC.Parameters.AddWithValue("@intIdPostulante", IdPostulante);

                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;
                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            result = new DatosPostulante()
                            {
                                IdPostulante = oSqlR["IdPostulante"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdPostulante"]) : 0,                                
                                TipoDocumento = oSqlR["TipoDocumento"] != DBNull.Value ? Convert.ToString(oSqlR["TipoDocumento"]) : string.Empty,
                                Documento = oSqlR["Documento"] != DBNull.Value ? Convert.ToString(oSqlR["Documento"]) : string.Empty,
                                Nombres = oSqlR["Nombres"] != DBNull.Value ? Convert.ToString(oSqlR["Nombres"]) : string.Empty,
                                ApellidoPaterno = oSqlR["ApellidoPaterno"] != DBNull.Value ? Convert.ToString(oSqlR["ApellidoPaterno"]) : string.Empty,
                                ApellidoMaterno = oSqlR["ApellidoMaterno"] != DBNull.Value ? Convert.ToString(oSqlR["ApellidoMaterno"]) : string.Empty,
                                Email = oSqlR["Email"] != DBNull.Value ? Convert.ToString(oSqlR["Email"]) : string.Empty,
                                Celular = oSqlR["Celular"] != DBNull.Value ? Convert.ToString(oSqlR["Celular"]) : string.Empty,
                                Modalidad = oSqlR["Modalidad"] != DBNull.Value ? Convert.ToString(oSqlR["Modalidad"]) : string.Empty,
                                Sede = oSqlR["Sede"] != DBNull.Value ? Convert.ToString(oSqlR["Sede"]) : string.Empty,
                                Escuela = oSqlR["Escuela"] != DBNull.Value ? Convert.ToString(oSqlR["Escuela"]) : string.Empty,
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

        public async Task<LogPostulante> PostulanteLogin(string Documento)
        {
            var result = new LogPostulante();

            try
            {
                using (SqlCommand oCmC = new SqlCommand())
                {
                    oCmC.CommandType = CommandType.StoredProcedure;
                    oCmC.CommandText = "Autenticacion";
                    oCmC.Parameters.AddWithValue("@vchDocumento", Documento);

                    oConn = await vgBDConeccion.AbrirModoLecturaAsync();
                    oTran = await Task.Run<SqlTransaction>(() => oConn.BeginTransaction());
                    oCmC.Connection = oTran.Connection;
                    oCmC.Transaction = oTran;
                    using (SqlDataReader oSqlR = await oCmC.ExecuteReaderAsync())
                    {
                        while (await oSqlR.ReadAsync())
                        {
                            result = new LogPostulante()
                            {
                                IdPostulante = oSqlR["IdPostulante"] != DBNull.Value ? Convert.ToInt32(oSqlR["IdPostulante"]) : 0,
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
