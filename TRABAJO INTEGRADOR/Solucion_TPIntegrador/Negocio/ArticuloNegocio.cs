using Dominio;  
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();    
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("select A.Id, A.Codigo, A.Nombre, A.Descripcion, M.Descripcion as Marca, C.Descripcion as Categoria, A.ImagenUrl, A.Precio, A.IdMarca, A.IdCategoria From ARTICULOS A, MARCAS M, CATEGORIAS C Where M.Id = A.IdMarca and C.Id = A.IdCategoria ");
                datos.EjecutarLector();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();    
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];  
                    }

                    aux.Precio = (decimal)datos.Lector["Precio"];   

                    lista.Add(aux);
                }
                return lista;   

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion(); 
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)values(@codigo, @nombre, @desc, @IdMarca, @IdCategoria, @img, @precio)");
                datos.setearParametros("@codigo", nuevo.Codigo);
                datos.setearParametros("@nombre", nuevo.Nombre);
                datos.setearParametros("@desc", nuevo.Descripcion);
                datos.setearParametros("@IdMarca", nuevo.Marca.Id);
                datos.setearParametros("@IdCategoria", nuevo.Categoria.Id);
                datos.setearParametros("@img", nuevo.ImagenUrl);
                datos.setearParametros("@precio", nuevo.Precio);
                datos.EjecutarAccion();

            }
            catch ( Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion(); 
            }


        }

        public void modificar (Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @desc, IdMarca = idMarca, IdCategoria = idCategoria, ImagenUrl = @img, Precio = @precio where Id = @id");
                datos.setearParametros("@codigo", articulo.Codigo);
                datos.setearParametros("@nombre", articulo.Nombre);
                datos.setearParametros("@desc", articulo.Descripcion);
                datos.setearParametros("@IdMarca", articulo.Marca.Id);
                datos.setearParametros("@IdCategoria", articulo.Categoria.Id);
                datos.setearParametros("@img", articulo.ImagenUrl);
                datos.setearParametros("@precio", articulo.Precio);
                datos.setearParametros("@id", articulo.Id); 
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.CerrarConexion(); 
            }
        }

        public void eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("delete from ARTICULOS where Id = @Id");
                datos.setearParametros("@Id", id);
                datos.EjecutarAccion(); 
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }

        public List<Articulo> Filtrar(string Campo, string Criterio, string Filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT A.Id, Codigo, Nombre, A.Descripcion, M.Descripcion as Marca, C.Descripcion as Categoria, ImagenUrl, Precio, A.IdMarca, A.IdCategoria FROM ARTICULOS A, MARCAS M, CATEGORIAS C Where A.IdMarca = M.Id AND A.IdCategoria = C.Id AND "; 

                if(Campo == "Precio")
                {
                    switch (Criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + Filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + Filtro;
                            break;
                        default:
                            consulta += "Precio = " + Filtro;
                            break;
                    }
                }
                else if(Campo == "Nombre" || Campo == "Marca" || Campo == "Categoria" || Campo == "Codigo")
                {
                    //para textos
                    if (Campo == "Marca")
                        Campo = "M.Descripcion";
                    else if (Campo == "Categoria")
                        Campo = "C.Categoria";
                    else
                        Campo = "A. " + Campo; //"A." va a ser solo nombre o codigo

                    switch (Criterio)
                    {
                        case "Comienza con":
                            consulta += Campo + " like '" + Filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += Campo + " like '%" + Filtro + "'";
                            break;
                        default: //contiene
                            consulta += Campo + " like '%" + Filtro + "%'";
                            break;  
                    }                                    
                }

                datos.SetearConsulta(consulta);
                datos.EjecutarLector();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];

                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];

                    aux.Precio = (decimal)datos.Lector["Precio"];
                    lista.Add(aux); 
                }

                    return lista;   
            }
            catch (Exception ex)
            {

                throw ex ;
            }
            finally
            {
                datos.CerrarConexion(); 
            }


        }





    }
}
