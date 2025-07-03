using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using DL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BL
{
    public class Restaurante
    {

        private readonly RestaurantedbcoreContext _context;


        public Restaurante(DL.RestaurantedbcoreContext context)
        {

            _context = context;
        }

        public ML.Result Add(ML.Restaurante restaurante)
        {
            ML.Result result = new ML.Result();

            var img = new SqlParameter("@Imagen", System.Data.SqlDbType.VarBinary);
            if (restaurante.Imagen != null)
            {
                img.Value = restaurante.Imagen;
            }
            else
            {
                img.Value = DBNull.Value;
            }

            try
            {
                int filasAfectadas = _context.Database.ExecuteSqlRaw($"RestauranteAdd '{restaurante.Nombre}','{restaurante.Slogan}', '{restaurante.Descripcion}',@Imagen", img);

                if (filasAfectadas > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Ocurrio un problema al insertar los datos";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result Update(int IdRestaurante, ML.Restaurante restaurante)
        {

            ML.Result result = new ML.Result();

            try
            {

                var img = new SqlParameter("@Imagen", System.Data.SqlDbType.VarBinary);
                if (restaurante.Imagen != null)
                {
                    img.Value = restaurante.Imagen;
                }
                else { img.Value = DBNull.Value; }

                int filasAfectadas = _context.Database.ExecuteSqlRaw($"RestauranteUpdate {IdRestaurante},'{restaurante.Nombre}', '{restaurante.Slogan}', '{restaurante.Descripcion}', @Imagen", img);

                if (filasAfectadas > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error al actualizar campos";
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result Delete(int IdRestaurante)
        {
            ML.Result result = new ML.Result();

            try
            {

                int filasAfectadas = _context.Database.ExecuteSqlRaw($"RestauranteDelete {IdRestaurante}");

                if (filasAfectadas > 0)
                {
                    result.Correct = true;
                }
                else
                {
                    result.Correct = false;
                    result.ErrorMessage = "Error al eliminar registro";
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }

        public ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try {

                var query = _context.Restaurantes.FromSqlRaw("RestauranteGetAll").ToList();

                if (query.Count > 0) {

                    result.Objects = new List<object>();

                    foreach (var item in query) { 
                        
                        ML.Restaurante restauranteTemp = new ML.Restaurante();

                        restauranteTemp.IdRestaurante = item.IdRestaurante; 
                        restauranteTemp.Nombre = item.Nombre;
                        restauranteTemp.Slogan = item.Slogan;
                        restauranteTemp.Descripcion = item.Descripcion;
                        restauranteTemp.Imagen = item.Imagen;   

                        result.Objects.Add(restauranteTemp);
                    }

                    result.Correct = true;
                }

            }
            catch (Exception ex) {
                result.Correct = false;
                result.ErrorMessage= ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public ML.Result GetByid(int IdRestaurante) { 
            ML.Result result = new ML.Result();

            try {

                var query = _context.Restaurantes.FromSqlRaw($"RestauranteGetById {IdRestaurante}").AsEnumerable().SingleOrDefault();

                if (query != null) {

                    ML.Restaurante restauranteTemp = new ML.Restaurante();

                    restauranteTemp.IdRestaurante = query.IdRestaurante;
                    restauranteTemp.Nombre= query.Nombre;
                    restauranteTemp.Slogan= query.Slogan;
                    restauranteTemp.Descripcion= query.Descripcion;
                    restauranteTemp.Imagen= query.Imagen;

                    result.Object = restauranteTemp;
                }

                result.Correct = true;
            
            }
            catch (Exception ex) {
                result.Correct = true;
                result.ErrorMessage= ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
