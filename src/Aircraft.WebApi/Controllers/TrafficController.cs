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
        private readonly static ConcurrentQueue<string> State = new ConcurrentQueue<string>();
        private readonly static ConcurrentDictionary<string, int> View = new ConcurrentDictionary<string, int>();
        private readonly ILogger<TrafficController> _logger;

        public TrafficController(ILogger<TrafficController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<int> Get()
        {
            var keys = TrafficController.State.ToArray();
            var temporal = new Dictionary<string, int>();
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

        [HttpGet("view")]
        public IEnumerable<int> GetSort()
        {
            var values = TrafficController.View.Values.Distinct().ToArray();            
            Array.Sort(values); 
            return values.Reverse().Take(20);
        }       
        [HttpPost("reset")]
        public int Reset(){
            TrafficController.State.Clear();
            TrafficController.View.Clear();
            return 0;
        }
        

        [HttpPost]
        public Tuple<string,int> Post(){
            var key = DateTime.Now.ToString("hh:mm:ss.fff");
            TrafficController.State.Enqueue(key);           
            
            TrafficController.View.AddOrUpdate(key, 
                1, 
                (key, value) => {                     
                    return value + 1;                    
                } 
            );
            return new Tuple<string,int>(key, TrafficController.View.GetValueOrDefault(key));

        }
    }
}
