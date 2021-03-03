using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Aircraft.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrafficController : ControllerBase
    {
        private readonly static ConcurrentQueue<long> State = new ConcurrentQueue<long>();        
        private readonly ILogger<TrafficController> _logger;

        public TrafficController(ILogger<TrafficController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<int> Get()
        {
            var keys = TrafficController.State.ToArray();
            var temporal = new Dictionary<long, int>();
            foreach (var item in keys)
            {
                if( temporal.ContainsKey(item)){
                    temporal[item] += 1;
                }
                else{
                    temporal[item] = 1;
                }
            } 
            var values = temporal.Values.Distinct().ToArray();
            Array.Sort(values);
            return values.Reverse();
        }        

        [HttpPost("reset")]
        public int Reset(){
            TrafficController.State.Clear();            
            return 0;
        }
        

        [HttpPost]
        public long Post(){
            
            var key = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            TrafficController.State.Enqueue(key);                                    
            return key;
        }
    }
}
