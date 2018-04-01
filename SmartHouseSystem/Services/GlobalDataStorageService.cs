using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHouseSystem.Services
{
   public class GlobalDataStorageService:IGlobalDataStorageService
    {
        public int BulbOnTimeInMinutes { get; set; }
    }
}
