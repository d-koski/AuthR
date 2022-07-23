using AuthR.Common.Abstractions.Services;

namespace AuthR.Common.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}