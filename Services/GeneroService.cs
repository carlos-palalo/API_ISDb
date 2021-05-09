﻿using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class GeneroService : IGeneroService
    {
        private proyectoContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GeneroService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Genero> GetAll()
        {
            return _context.Genero.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Genero GetGenero(int id)
        {
            return _context.Genero.Find(id);
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <param name="genero"></param>
        /// <returns></returns>
        public Boolean PutGenero(Genero genero)
        {
            var v_genero = _context.Genero.SingleOrDefault(a => a.IdGenero == genero.IdGenero);
            if (v_genero != null)
            {
                _context.Entry(v_genero).CurrentValues.SetValues(genero);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
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
        /// 
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