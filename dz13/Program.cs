using System.Text.Json;

namespace dz13
{
    [Serializable]
    public class Rational
    {
        public int Top { get; set; }
        public int Bottom { get; set; }

        public Rational() { }

        public Rational(int top, int bottom)
        {
            Top = top;
            Bottom = bottom == 0 ? 1 : bottom;
        }

        public override string ToString() => $"{Top}/{Bottom}";
    }

    [Serializable]
    public class Entry
    {
        public string Heading { get; set; }
        public int TextLength { get; set; }
        public string Summary { get; set; }

        public override string ToString() =>
            $"  - Title: {Heading}\n    Characters: {TextLength}\n    Summary: {Summary}";
    }

    [Serializable]
    public class Publication
    {
        public string Name { get; set; }
        public string Distributor { get; set; }
        public DateTime IssueDate { get; set; }
        public int TotalPages { get; set; }
        public List<Entry> Content { get; set; } = new();

        public override string ToString()
        {
            string result = $"Title: {Name}\nPublisher: {Distributor}\nRelease Date: {IssueDate:yyyy-MM-dd}\nPages: {TotalPages}\nArticles:";
            if (Content.Count == 0)
                result += " (no articles)";
            else
                foreach (var e in Content)
                    result += "\n" + e;
            return result;
        }
    }

    internal class Executor
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Rational> rationalList = new();

            Console.Write("How many fractions would you like to enter? ");
            int total = int.Parse(Console.ReadLine());

            for (int i = 0; i < total; i++)
            {
                Console.WriteLine($"\nFraction #{i + 1}:");
                Console.Write("Numerator: ");
                int top = int.Parse(Console.ReadLine());

                Console.Write("Denominator: ");
                int bottom = int.Parse(Console.ReadLine());

                rationalList.Add(new Rational(top, bottom));
            }

            string fractionData = JsonSerializer.Serialize(rationalList);
            File.WriteAllText("fractions.json", fractionData);
            Console.WriteLine("\nFraction array has been serialized and saved to fractions.json.");

            string retrievedFractionData = File.ReadAllText("fractions.json");
            List<Rational> deserializedFractions = JsonSerializer.Deserialize<List<Rational>>(retrievedFractionData);

            Console.WriteLine("\nDeserialized fractions:");
            foreach (var r in deserializedFractions)
            {
                Console.WriteLine(r);
            }

            List<Publication> pubList = new();

            Console.Write("How many magazines would you like to create? ");
            int pubCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < pubCount; i++)
            {
                Console.WriteLine($"\nMagazine #{i + 1}:");
                Publication pub = new();

                Console.Write("Magazine title: ");
                pub.Name = Console.ReadLine();

                Console.Write("Publisher name: ");
                pub.Distributor = Console.ReadLine();

                Console.Write("Release date (yyyy-mm-dd): ");
                pub.IssueDate = DateTime.Parse(Console.ReadLine());

                Console.Write("Number of pages: ");
                pub.TotalPages = int.Parse(Console.ReadLine());

                Console.Write("How many articles? ");
                int articleCount = int.Parse(Console.ReadLine());

                for (int j = 0; j < articleCount; j++)
                {
                    Console.WriteLine($"\n  Article #{j + 1}:");
                    Entry e = new();

                    Console.Write("  Title: ");
                    e.Heading = Console.ReadLine();

                    Console.Write("  Character count: ");
                    e.TextLength = int.Parse(Console.ReadLine());

                    Console.Write("  Preview: ");
                    e.Summary = Console.ReadLine();

                    pub.Content.Add(e);
                }

                pubList.Add(pub);
            }

            string pubFile = "magazine_array.json";
            string pubJson = JsonSerializer.Serialize(pubList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(pubFile, pubJson);
            Console.WriteLine($"\nMagazine array saved to file {pubFile}.");

            string retrievedPubJson = File.ReadAllText(pubFile);
            List<Publication> rehydrated = JsonSerializer.Deserialize<List<Publication>>(retrievedPubJson);

            Console.WriteLine("\n📚 Deserialized magazines:");
            foreach (var p in rehydrated)
            {
                Console.WriteLine("\n-------------------------");
                Console.WriteLine(p);
            }
        }
    }
}
