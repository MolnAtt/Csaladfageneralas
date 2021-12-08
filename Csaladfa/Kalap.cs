using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Csaladfa
{
    class Kalap<T>
    {
        Random r = new Random();
        List<T> lista;

        public Kalap(List<T> lista) => this.lista = lista;
        public Kalap(): this(new List<T>()){}
                
        public T Peek() => lista[r.Next(lista.Count)];
        public T Pop() => Pop(r.Next(lista.Count));

        private T Pop(int i)
        {
            T result = lista[i];
            lista.RemoveAt(i);
            return result;
        }

        private T Pop(T result)
        {
            lista.Remove(result);
            return result;
        }

        public int Count { get => lista.Count;}

        public void Push(T elem) => lista.Add(elem);
        public void Push(List<T> lista) => this.lista.AddRange(lista);
    }
}
