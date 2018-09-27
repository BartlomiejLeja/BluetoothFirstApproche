namespace SmartHouseSystem.Services
{
    public interface IChartService
    {
        void ChartHandler(bool isStatisticsServiceOn, ILightService lightService);
    }
}
