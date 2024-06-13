using JMDict;

namespace JmdictGQL
{
    public class Query(Jmdict jmdict)
    {
        private readonly Jmdict _jmdict = jmdict;

        public IEnumerable<JmdictEntry?> EntriesByReading(string reading)
        {
            return _jmdict.Entries
                .Where(e => e.Readings?
                .FirstOrDefault(r => r.Kana == reading) != null);
        }
    }
}

