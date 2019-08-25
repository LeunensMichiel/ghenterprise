using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Data
{
    public class EventService : BaseService
    {
        public EventService()
        {
        }

        public async Task<bool> SaveEventAsync(Event eventItem)
        {
            return await PostAsync("Event", eventItem);
        }

        public async Task<List<Event>> GetEventsAsync()
        {
            return await GetAsync<List<Event>>("Event");
        }

        public async Task<Event> GetEventAsync(string Id)
        {
            return await GetAsync<Event>($"Event?Event_ID={Id}");
        }
    }
}
