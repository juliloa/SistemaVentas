using Sexshop_TutsiPop.Models;
using Npgsql;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NpgsqlTypes;

namespace Sexshop_TutsiPop.Datos
{
    public class Dbusuarios
    {
        private readonly string _connectionString;

        public Dbusuarios(IConfiguration configuration)
        {
            // Asigna la cadena de conexión a la variable desde el archivo appsettings.json
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool Registrar(usuarios usuario)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
                {
                    string query = "INSERT INTO usuarios (nombre, email, contrasenna, token, confirmado, restablecer) " +
                                   "VALUES (@nombre, @email, @contrasenna, @token, @confirmado, @restablecer)";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@nombre", usuario.nombre);
                        cmd.Parameters.AddWithValue("@email", usuario.email);
                        cmd.Parameters.AddWithValue("@contrasenna", usuario.contrasenna);
                        cmd.Parameters.AddWithValue("@token", usuario.token);
                        cmd.Parameters.AddWithValue("@confirmado", NpgsqlDbType.Boolean).Value = usuario.confirmado;
                        cmd.Parameters.AddWithValue("@restablecer", usuario.restablecer);

                        conexion.Open();
                        int filasafectadas = cmd.ExecuteNonQuery();
                        if (filasafectadas > 0) respuesta = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return respuesta;
        }

        public usuarios Validar(string email, string contrasenna)
        {
            usuarios usuario = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
                {
                    string query = "SELECT nombre, restablecer, confirmado, rol FROM usuarios WHERE email = @email AND contrasenna = @contrasenna";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@contrasenna", contrasenna);

                        conexion.Open();
                        using (NpgsqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuario = new usuarios()
                                {
                                    nombre = reader["nombre"].ToString(),
                                    restablecer = (bool)reader["restablecer"],
                                    confirmado = (bool)reader["confirmado"],
                                    rol = reader["rol"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la validación: " + ex.Message);
                throw;
            }
            return usuario;
        }

        [HttpPost]
        public usuarios Obtener(string email)
        {
            usuarios usuario = null;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
                {
                    string query = "SELECT nombre, contrasenna, restablecer, confirmado, token FROM usuarios WHERE email = @email";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@email", email);

                        conexion.Open();
                        using (NpgsqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                usuario = new usuarios()
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error en la obtención: " + ex.Message);
                throw;
            }
            return usuario;
        }

        public bool RestablecerActualizar(bool restablecer, string contrasenna, string token)
        {
            bool respuesta = false;

            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
                {
                    string query = "UPDATE usuarios SET restablecer = @restablecer, contrasenna = @contrasenna WHERE token = @token";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@restablecer", restablecer);
                        cmd.Parameters.AddWithValue("@contrasenna", contrasenna);
                        cmd.Parameters.AddWithValue("@token", token);

                        conexion.Open();
                        int filasafectadas = cmd.ExecuteNonQuery();
                        if (filasafectadas > 0) respuesta = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return respuesta;
        }

        public bool Confirmar(string token)
        {
            bool respuesta = false;
            try
            {
                using (NpgsqlConnection conexion = new NpgsqlConnection(_connectionString))
                {
                    string query = "UPDATE usuarios SET confirmado = true WHERE token = @token";

                    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conexion))
                    {
                        cmd.Parameters.AddWithValue("@token", token);

                        conexion.Open();
                        int filasafectadas = cmd.ExecuteNonQuery();
                        if (filasafectadas > 0) respuesta = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return respuesta;
        }
    }
}
