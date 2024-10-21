using Sexshop_TutsiPop.Models;
using Npgsql;
using System.Data.SqlClient;
using System.Data;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NpgsqlTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Mvc;

namespace Sexshop_TutsiPop.Datos
{
    public class DbUsuarios
    {
        private static string CadenaSQL = "Host=ep-billowing-butterfly-a5gnrjz7.us-east-2.aws.neon.tech;Database=TutsiPop2;Username=TutsiPop2_owner;Password=mUkEx9lKaf5c;SslMode=Require";

        public static bool Registrar(Usuarios usuario)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "insert into Usuarios (nombre,email,contrasenna,token,confirmado," +
                        "restablecer)";
                    query += " values (@nombre,@email,@contrasenna,@token,@confirmado,@restablecer)";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@nombre", usuario.nombre);
                    cmd.Parameters.AddWithValue("@email", usuario.email);
                    cmd.Parameters.AddWithValue("@contrasenna", usuario.contrasenna);
                    cmd.Parameters.AddWithValue("@token", usuario.token);
                    cmd.Parameters.AddWithValue("@confirmado", NpgsqlDbType.Boolean).Value = usuario.confirmado;
                    cmd.Parameters.AddWithValue("@restablecer", usuario.restablecer);


                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filasafectadas = cmd.ExecuteNonQuery();
                    if (filasafectadas > 0) respuesta = true;
                }
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static Usuarios Validar(string email, string contrasenna)
        {
            Usuarios usuario = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "SELECT nombre, restablecer, confirmado FROM Usuarios";
                    query += " WHERE email = @email and contrasenna = @contrasenna";



                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@contrasenna", contrasenna);

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuarios()
                            {
                                nombre = reader["nombre"].ToString(),
                                restablecer = (bool)reader["restablecer"],
                                confirmado = (bool)reader["confirmado"]
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return usuario;
        }

        [HttpPost]
        public static Usuarios Obtener(string email)
        {
            Usuarios usuario = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = "select nombre,contrasenna,restablecer,confirmado,token from Usuarios";
                    query += " where email=@email";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = email;
                    cmd.CommandType = CommandType.Text;


                    conexion.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario = new Usuarios()
                            {
                                nombre = dr["nombre"].ToString(),
                                contrasenna = dr["contrasenna"].ToString(),
                                restablecer = (bool)dr["restablecer"],
                                confirmado = (bool)dr["confirmado"],
                                token = dr["token"].ToString()
                            };
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return usuario;
        }

        public static bool RestablecerActualizar(bool restablecer, string contrasenna, string token)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE Usuarios SET " +
                        "restablecer = @restablecer, " +
                        "contrasenna = @contrasenna " +
                        "WHERE token = @token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@restablecer", restablecer);
                    cmd.Parameters.AddWithValue("@contrasenna", contrasenna);
                    cmd.Parameters.AddWithValue("@token", NpgsqlTypes.NpgsqlDbType.Varchar).Value = token;

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filasafectadas = cmd.ExecuteNonQuery();
                    if (filasafectadas > 0) respuesta = true;
                }
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool Confirmar(string token)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(CadenaSQL))
                {
                    string query = @"UPDATE Usuarios SET " +
                        "confirmado = true " +
                        "WHERE token = @token";

                    NpgsqlCommand cmd = new NpgsqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@token", token);

                    cmd.CommandType = CommandType.Text;

                    conexion.Open();

                    int filasafectadas = cmd.ExecuteNonQuery();
                    if (filasafectadas > 0) respuesta = true;
                }
                return respuesta;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
