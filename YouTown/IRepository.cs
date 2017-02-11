using System;
using System.Collections.Generic;

namespace YouTown
{
    public interface IRepository
    {
        IGameItem Get(int id);
        void Add(IGameItem itemToAdd);
    }

    public class Repository : IRepository
    {
        private readonly Dictionary<int, IGameItem> _itemById = new Dictionary<int, IGameItem>();

        public IGameItem Get(int id)
        {
            if (_itemById.ContainsKey(id))
            {
                return _itemById[id];
            }
            throw new ArgumentOutOfRangeException($"cannot find item in repo with id {id}");
        }

        public void Add(IGameItem itemToAdd)
        {
            if (_itemById.ContainsKey(itemToAdd.Id))
            {
                var item = Get(itemToAdd.Id);
                throw new ArgumentException($"duplicate entry! Tried adding item with id {itemToAdd.Id} of type {itemToAdd.GetType().Name}, but item of type {item.GetType().Name} is already present");
            }
            _itemById[itemToAdd.Id] = itemToAdd;
        }
    }

    public static class RepositoryExtensions
    {
        public static TGameItem Get<TGameItem>(this IRepository repository, int id)
            where TGameItem : IGameItem
        {
            var item = repository.Get(id);
            return (TGameItem) item;
        }

        public static TGameItem GetOrNull<TGameItem>(this IRepository repository, int? id)
            where TGameItem : class, IGameItem
        {
            if (!id.HasValue)
            {
                return null;
            }
            var item = repository.Get(id.Value);
            return (TGameItem)item;
        }

        public static void AddAll(this IRepository repo, IEnumerable<IGameItem> items)
        {
            foreach (var gameItem in items)
            {
                repo.Add(gameItem);
            }
        }

        public static void AddAll(this IRepository repo, params IGameItem[] items)
        {
            AddAll(repo, (IEnumerable<IGameItem>)items);
        }
    }
}