using Hawi.Contracts;

namespace Hawi.Repository
{
    public class DateTimeBetweenRange: IDateTimeBetweenRange
    {
        public List<DateTime> GetDateTimeOccurrences(string dayName, DateTime startDateTime, DateTime endDateTime)
        {
            if (string.IsNullOrEmpty(dayName) || startDateTime >= endDateTime)
            {
                return new List<DateTime>();
            }

            if (!Enum.TryParse(dayName, true, out DayOfWeek dayOfWeek))
            {
                return new List<DateTime>();
            }

            List<DateTime> occurrences = new List<DateTime>();

            DateTime currentDateTime = startDateTime;
            while (currentDateTime <= endDateTime)
            {
                if (currentDateTime.DayOfWeek == dayOfWeek)
                {
                    occurrences.Add(currentDateTime);
                }
                currentDateTime = currentDateTime.AddDays(1);
            }

            return occurrences;
        }
    }
}
