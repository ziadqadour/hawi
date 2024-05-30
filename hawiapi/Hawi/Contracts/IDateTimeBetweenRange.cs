namespace Hawi.Contracts
{
    public interface IDateTimeBetweenRange
    {
        List<DateTime> GetDateTimeOccurrences(string dayName, DateTime startDateTime, DateTime endDateTime);
    }
}
