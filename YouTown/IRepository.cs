using System;
using System.Collections.Generic;

namespace YouTown
{
    public interface IRepository
    {
        IGameItem Get(int id);
    }

    public class Repository : IRepository
    {
        private Dictionary<int, IGameItem> _itemById = new Dictionary<int, IGameItem>();

        public IGameItem Get(int id)
        {
            if (_itemById.ContainsKey(id))
            {
                return _itemById[id];
            }
            throw new ArgumentOutOfRangeException();
        }

        public void Add(IGameItem item)
        {
            if (_itemById.ContainsKey(item.Id))
            {
                throw new ArgumentException("duplaicate entry");
            }
            _itemById[item.Id] = item;
        }
    }

    public static class RepositoryExtensions
    {
        public static TGameItem Get<TGameItem>(this IRepository repository, int id)
        {
            var item = repository.Get(id);
            return (TGameItem) item;
        }
    }
}