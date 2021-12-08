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
        private static StreamWriter gy = new StreamWriter("g.txt");
        private static StreamWriter graf = new StreamWriter("graf.txt");
        private static StreamWriter grafhosszu = new StreamWriter("grafhosszu.txt");

        const int mingyerek = 1;
        const int maxgyerek = 5;


        static Random g = new Random();
        static Kalap<string> veznevek = new Kalap<string>(Beolvas("veznev.txt"));
        static Kalap<string> fiunevek = new Kalap<string>(Beolvas("sokfiunev.txt"));
        static Kalap<string> lanynevek = new Kalap<string>(Beolvas("soklanynev.txt"));
        private static List<string> Beolvas(string path) => File.ReadAllLines(path).ToList();

        int id;
        Nem nem;
        public string Veznev { get; }
        public string Kernev { get; }


        List<Csaladtag> gyerekei;

        private static List<Csaladtag> lista = new List<Csaladtag>();
        public Csaladtag(Nem nem, string veznev, string kernev)
        {
            this.nem = nem;
            this.Veznev = veznev;
            this.Kernev = kernev;
            gyerekei = new List<Csaladtag>();
            lista.Add(this);
            this.id = lista.Count;
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
        


        public static List<Csaladtag> operator +(Csaladtag egyik, Csaladtag masik)
        {
            int gyerekszam = g.Next(mingyerek, maxgyerek);

            (Csaladtag apa, Csaladtag anya) = egyik.nem == Nem.Férfi ? (egyik, masik) : (masik, egyik);


            for (int i = 0; i < gyerekszam && VanMégNév(); i++)
                Console.WriteLine(new Csaladtag(apa, anya));
            foreach (Csaladtag gyerek in apa.gyerekei)
            {
                gy.WriteLine($"{apa.id}\t{gyerek.id}");
                gy.WriteLine($"{anya.id}\t{gyerek.id}");

                graf.WriteLine($"{apa.id} -> {gyerek.id};");
                graf.WriteLine($"{anya.id} -> {gyerek.id};");

                grafhosszu.WriteLine($"\"{apa}\" -> \"{gyerek}\";");
                grafhosszu.WriteLine($"\"{ anya}\" -> \"{gyerek}\";");
            }



            return apa.gyerekei;
        }

        private static bool VanMégNév() => 0 < veznevek.Count && 0 < fiunevek.Count && 0 < lanynevek.Count;

        public List<Csaladtag> Párt_keres()
        {
            Csaladtag pár = new Csaladtag(MásikNem(this.nem));
            Console.WriteLine($"{this} párt talált magának: {pár}, gyerekeik pedig:");
            return this + pár;
        }
        public static void Családfagenerálás_Vezetéknevek_kifogyásáig()
        {
            while (VanMégNév())
            {
                Kalap<Csaladtag> párkeresők = new Kalap<Csaladtag>();
                párkeresők.Push(new Csaladtag());
                while (0 < veznevek.Count)
                    párkeresők.Push(párkeresők.Pop().Párt_keres());
            }

            Univerzum_kiírása();
            

        }

        private static void Univerzum_kiírása()
        {
            using (StreamWriter u = new StreamWriter("u.txt"))
            {
                u.WriteLine($"id\tveznev\tkernev\tnem");
                foreach (Csaladtag ember in lista)
                    u.WriteLine($"{ember.id}\t{ember.Veznev}\t{ember.Kernev}\t{ember.nem}");
            }
        }
    }
}
