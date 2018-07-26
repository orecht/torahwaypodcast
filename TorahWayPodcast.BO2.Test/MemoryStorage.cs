
using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;

namespace TorahWayPodcast.BO2.Test
{
    public class MemoryStorage : IStorage<Shiurim>
    {
        private Shiurim _shiurim;

        public Shiurim Read()
        {
            return _shiurim;
        }

        public void Write(Shiurim shiurim)
        {
            _shiurim = shiurim;
        }
    }
}
