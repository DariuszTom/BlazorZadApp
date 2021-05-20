using DataLibrary.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLibrary
{
    public interface IEventData
    {
        Task<List<EventModel>> GetEvent();

        Task InsertEvent(EventModel newEvent);
    }
}