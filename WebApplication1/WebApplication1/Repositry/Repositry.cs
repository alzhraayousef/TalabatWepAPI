using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.repo
{
    public class Repositry<T> : IRepositry<T> where T : class
    {
        Context context;
        public Repositry(Context _context)
        {
            context = _context;
        }
        public List<T> getall()
        {
            return context.Set<T>().ToList();
        }
        public List<T> getall(string s)
        {
            return context.Set<T>().Include(s).ToList();
        }
        public List<T> getall(string s, string s2)
        {
            return context.Set<T>().Include(s).Include(s2).ToList();
        }
        public List<T> getall(string s, string s2 ,string s3)
        {
            return context.Set<T>().Include(s).Include(s2).Include(s3).ToList();
        }

        public T getbyid(int id)
        {

            return context.Find<T>(id);
        }
    


        public void create(T t)
        {
            context.Set<T>().Add(t);
            context.SaveChanges();
        }
        public void update(T tt)
        {
            context.Update<T>(tt);
            context.SaveChanges();
        }
        public void delete(T tt)
        {
            context.Remove<T>(tt);
            context.SaveChanges();
        }

    }
}
