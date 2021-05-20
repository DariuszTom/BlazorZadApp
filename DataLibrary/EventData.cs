using DataLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class EventData : IEventData
    {
        private readonly ISQLDataAccess _db;
        public EventData(ISQLDataAccess db)
        {
            _db = db;
        }
        public Task<List<EventModel>> GetEvent()
        {
            string sql = "SELECT * FROM EVENTS";
            return _db.LoadData<EventModel, dynamic>(sql, new { });
        }
        public Task InsertEvent(EventModel newEvent)
        {
            string sql = @"INSERT INTO EVENTS (Name, Description, StartDate, EndDate) 
                        VALUES (@EventName, @EventDesc, @EventStartDate, @EventEndDate)";
            return _db.Savedate(sql, newEvent);

        }
    }
}
