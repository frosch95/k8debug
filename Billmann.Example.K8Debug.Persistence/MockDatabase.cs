using System;
using Billmann.Example.K8Debug.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Billmann.Example.K8Debug.Persistence
{
    public class MockDatabase: IDatabase
    {
        private readonly ILogger<MockDatabase> _logger;
        private readonly Dictionary<Guid, ToDoItem> _database;

        public MockDatabase(ILogger<MockDatabase> logger)
        {
            _logger = logger;
            _database = new Dictionary<Guid, ToDoItem>();
        }

        public Task Add(ToDoItem item)
        {
            _database.Add(item.Id, item);
            _logger.LogInformation("add item with id {Id} and found {Item}", item.Id, item);
            return Task.CompletedTask;
        }

        public Task<ToDoItem> Find(Guid id)
        {
            var foundItem = _database.TryGetValue(id, out var item);
            _logger.LogInformation("try to find item with id {Id} and found {Item}", id, item);
            return Task.FromResult(item);
        }

        public Task<IList<ToDoItem>> All()
        {
            IList<ToDoItem> items = _database.Values.OrderBy(x => x.Id).ToList();
            _logger.LogInformation("return {ItemsCount} items", items.Count);
            return Task.FromResult(items);
        }
    }
}
