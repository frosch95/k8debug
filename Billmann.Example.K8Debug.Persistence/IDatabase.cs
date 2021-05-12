using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Billmann.Example.K8Debug.Entities;

namespace Billmann.Example.K8Debug.Persistence
{
    public interface IDatabase
    {
        Task Add(ToDoItem item);

        Task<ToDoItem> Find(Guid id);

        Task<IList<ToDoItem>> All();
    }
}