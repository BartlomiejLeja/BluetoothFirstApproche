using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public interface ILightService
    {
        List<StatusModel1> StatusModels { get; set; }
    }
}
