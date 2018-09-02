using System;
using System.Collections.Generic;
using SmartHouseSystem.Model;

namespace SmartHouseSystem.Services
{
    public class LightService : ILightService
    {
        public List<StatusModel1> StatusModels { get; set; } = new List<StatusModel1>();
    }
}
