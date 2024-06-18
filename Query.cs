using JMDict;

namespace JmdictGQL
{
    public class Query(Jmdict jmdict, Kanjidic kanjidic)
    {
        private readonly Jmdict _jmdict = jmdict;
        private readonly Kanjidic _kanjidic = kanjidic;


        public IEnumerable<JmdictEntry?> EntriesByReading(string reading)
        {
            return _jmdict.Entries
                .Where(e => e.Readings?
                .FirstOrDefault(r => r.Kana == reading) != null);
        }

        public KanjidicCharacter? KanjiEntryByKanji(string kanji)
        {
            return _kanjidic.Characters.FirstOrDefault(c => c.Literal.Equals(kanji));
        }
    }
}

