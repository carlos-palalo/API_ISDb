using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_ISDb.Services
{
    /// <summary>
    /// GeneroService
    /// </summary>
    public class GeneroService : IGeneroService
    {
        private readonly proyectoContext _context;

        public GeneroService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtengo colección de Generos
        /// </summary>
        /// <returns></returns>
        public ICollection<Genero> GetAll()
        {
            return _context.Genero.ToArray();
        }

        /// <summary>
        /// Obtengo un género en concreto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Genero GetGenero(int id)
        {
            return _context.Genero.Find(id);
        }

        /// <summary>
        /// Obtengo los géneros relacionados con una serie
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ICollection<Genero> GetGeneros(int id)
        {
            var sg = _context.SerieGenero
                .Where(n => n.SerieIdSerie == id)
                .Select(item => item.GeneroIdGenero).ToList();

            var g = _context.Genero
                .Where(item => sg.Contains(item.IdGenero))
                .ToArray();

            return g;
        }

        /// <summary>
        /// Añado un genero comprobando existencia en Db y lo devuelvo
        /// </summary>
        /// <param name="genero"></param>
        /// <returns></returns>
        public Genero PostGenero(Genero genero)
        {
            Genero gen = Search(genero.Nombre);
            if (gen == null)
            {
                _context.Genero.Add(genero);
                _context.SaveChanges();
            }
            return Search(genero.Nombre);
        }

        /// <summary>
        /// Actualizo los datos de un genero
        /// </summary>
        /// <param name="genero"></param>
        /// <returns></returns>
        public Boolean PutGenero(Genero genero)
        {
            var v_genero = _context.Genero.SingleOrDefault(a => a.IdGenero == genero.IdGenero);
            if (v_genero != null)
            {
                _context.Entry(v_genero).CurrentValues.SetValues(genero);
                _context.Update(v_genero);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Elimino un género
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Boolean DeleteGenero(int id)
        {
            var generos = _context.Genero.SingleOrDefault(a => a.IdGenero == id);
            if (generos != null)
            {
                _context.Genero.Remove(generos);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Busco un género dada una cadena
        /// </summary>
        /// <param name="cad"></param>
        /// <returns></returns>
        public Genero Search(string cad)
        {
            var v_genero = _context.Genero.SingleOrDefault(a => a.Nombre.Contains(cad));
            if (v_genero != null)
            {
                return v_genero;
            }
            return v_genero;
        }
    }
}
