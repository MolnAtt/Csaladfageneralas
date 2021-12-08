using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Csaladfa
{
    
    enum Nem { Férfi, Nő }
    class Csaladtag
    {
        const int mingyerek = 1;
        const int maxgyerek = 10;


        static Random g = new Random();
        static Kalap<string> veznevek = new Kalap<string>(Beolvas("veznev.txt"));
        static Kalap<string> fiunevek = new Kalap<string>(Beolvas("sokfiunev.txt"));
        static Kalap<string> lanynevek = new Kalap<string>(Beolvas("soklanynev.txt"));
        private static List<string> Beolvas(string path) => File.ReadAllLines(path).ToList();

        Nem nem;
        public string Veznev { get; }
        public string Kernev { get; }


        List<Csaladtag> gyerekei;


        public Csaladtag(Nem nem, string veznev, string kernev)
        {
            this.nem = nem;
            this.Veznev = veznev;
            this.Kernev = kernev;
            gyerekei = new List<Csaladtag>();
        }
        public override string ToString() => $"{Veznev} {Kernev}";
        public Csaladtag(Nem nem, string veznev) : this(nem, veznev, Kernevek(nem).Pop()) { }
        public Csaladtag(Nem nem) : this(nem, veznevek.Pop(), Kernevek(nem).Pop()) { }
        public Csaladtag() : this(RandomNem()) { }
        public Csaladtag(string veznev):this(RandomNem(), veznev){}
        public Csaladtag(Csaladtag apa, Csaladtag anya) : this(RandomNem(), apa.Veznev) 
        {
            apa.gyerekei.Add(this);
            anya.gyerekei.Add(this);
        }

        private static Nem RandomNem() => (Nem)g.Next(2);
        private static Nem MásikNem(Nem nem) => (Nem)(1 - nem);
        private static Kalap<string> Kernevek(Nem nem) => nem == Nem.Férfi ? fiunevek: lanynevek;
        


        public static List<Csaladtag> operator +(Csaladtag apa, Csaladtag anya)
        {
            int gyerekszam = g.Next(mingyerek, maxgyerek);
            for (int i = 0; i < gyerekszam; i++)
                Console.WriteLine(new Csaladtag(apa, anya));

            return apa.gyerekei;
        }

        public bool Párt_keres()
        {
            if (veznevek.Count > 0)
            {
                Csaladtag pár = new Csaladtag(MásikNem(this.nem));
                Console.WriteLine($"{this} párt talált magának: {pár}, gyerekeik pedig:");
                List<Csaladtag> gyerekek = this + pár;
                return true;
            }
            return false;
        }
        public void Családfagenerálás_Vezetéknevek_kifogyásáig()
        {
            Kalap<Csaladtag> párkeresők = new Kalap<Csaladtag>();
            párkeresők.Push(this);
            while (0 < veznevek.Count)
            {
                Csaladtag párkereső = párkeresők.Pop();
                párkeresők.Push(párkereső + new Csaladtag(párkereső.nem));
            }
        }

    }
}
