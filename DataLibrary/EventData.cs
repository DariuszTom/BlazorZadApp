using DataLibrary.Model;
using System.Collections.Generic;
using System.Globalization;
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
            string sql = "SELECT * FROM Events;";
            return _db.LoadData<EventModel, dynamic>(sql, new { });
        }

        public Task InsertEvent(EventModel newEvent)
        {
            StringBuilder sqlPrep = new StringBuilder();
            sqlPrep.Clear();
            sqlPrep.Append(@"INSERT INTO Events (EventName, EventDesc, EventStartDate, EventEndDate) ");
            sqlPrep.Append($"VALUES ('{newEvent.EventName}', '{newEvent.EventDesc}', '{newEvent.EventStartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}', '{newEvent.EventEndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}')");

            return _db.Savedate(sqlPrep.ToString(), newEvent);
        }
    }
}