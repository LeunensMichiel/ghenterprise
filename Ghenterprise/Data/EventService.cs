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
            return await GetAsync<List<Event>>("Event", true);
        }

        public async Task<Event> GetEventsById(string Event_ID)
        {
            return await GetAsync<Event>($"Event?Event_ID={Event_ID}", true);
        }

        public async Task<bool> UpdateEvent(Event updateEvent)
        {
            return await PutAsync("Event", updateEvent);
        }

        public async Task<bool> DeleteEvent(string event_id)
        {
            return await DeleteAsync($"Event?Event_ID={event_id}");
        }

        public async Task<List<Event>> GetEventsOfOwner()
        {
            return await GetAsync<List<Event>>("Event/Owner", true);
        } 

        public async Task<Event> GetEventAsync(string Id)
        {
            return await GetAsync<Event>($"Event?Event_ID={Id}");
        }
    }
}
