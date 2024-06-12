using System.IO.Compression;
using JMDict;

namespace JmdictFetch
{
    public class JmdictFetch
    {
        // TODO be able to reuse this across functions
        private static readonly DictParser dictParser = new();

        public static async Task<Jmdict?> GetJmdict()
        {
            string baseURL = "http://ftp.edrdg.org/pub/Nihongo/JMdict_e.gz";

            try
            {
                using HttpClient client = new();
                using HttpResponseMessage res = await client.GetAsync(baseURL);
                res.EnsureSuccessStatusCode(); // Throw if not a success code.

                // Read the content as a stream
                using Stream responseStream = await res.Content.ReadAsStreamAsync();

                // Create a MemoryStream to hold the decompressed data
                using MemoryStream decompressedStream = new();

                // Decompress the response stream
                using (GZipStream gzipStream = new(responseStream, CompressionMode.Decompress))
                {
                    await gzipStream.CopyToAsync(decompressedStream);
                }

                // Rewind the stream to the beginning
                decompressedStream.Seek(0, SeekOrigin.Begin);

                // Pass the decompressed stream to the parser method directly
                return dictParser.ParseXml<Jmdict>(decompressedStream);
            }
            catch (Exception exception)
            {
                Console.WriteLine("An error occurred: " + exception.Message);
                return null;
            }
        }

    }
}