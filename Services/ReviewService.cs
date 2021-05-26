using API_ISDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_ISDb.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ReviewService : IReviewService
    {
        private proyectoContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public ReviewService(proyectoContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ICollection<Review> GetAll()
        {
            return _context.Review.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Review GetReview(int id)
        {
            return _context.Review.Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public List<ListaReview> GetListaReviews(int serie)
        {
            List<ListaReview> data = _context.Review
                .Join(
                    _context.Usuario,
                    review => review.UsuarioIdUsuario,
                    usuario => usuario.IdUsuario,
                    (review, usuario) => new
                    {
                        Usuario = usuario.Username,
                        Titulo = review.Titulo,
                        Descripcion = review.Descripcion,
                        Puntuacion = review.Puntuacion,
                        Fecha = review.Fecha,
                        IdSerie = review.SerieIdSerie
                    }
                ).Where(item => item.IdSerie == serie)
                .Select(item => new ListaReview(item.Usuario, item.Titulo, item.Descripcion, item.Puntuacion, item.Fecha.ToString("d")))
                .ToList();

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public Review PostReview(Review review)
        {
            _context.Review.Add(review);
            _context.SaveChanges();
            return review;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="review"></param>
        /// <returns></returns>
        public Boolean PutReview(Review review)
        {
            var v_review = _context.Review.SingleOrDefault(a => a.IdReview == review.IdReview);
            if (v_review != null)
            {
                _context.Entry(v_review).CurrentValues.SetValues(review);
                _context.Update(v_review);
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
        public Boolean DeleteReview(int id)
        {
            var reviews = _context.Review.SingleOrDefault(a => a.IdReview == id);
            if (reviews != null)
            {
                _context.Review.Remove(reviews);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
