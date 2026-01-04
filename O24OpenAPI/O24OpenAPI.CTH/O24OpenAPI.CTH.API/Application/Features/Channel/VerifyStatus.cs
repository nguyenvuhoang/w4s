using LinKit.Core.Cqrs;
using LinqToDB;
using O24OpenAPI.APIContracts.Constants;
using O24OpenAPI.Core;
using O24OpenAPI.CTH.Constant;
using O24OpenAPI.CTH.Domain.AggregatesModel.ChannelAggregate;
using O24OpenAPI.Framework.Attributes;
using O24OpenAPI.Framework.Models;

namespace O24OpenAPI.CTH.API.Application.Features.Channel;

public class VerifyStatusCommand : BaseTransactionModel, ICommand<bool> { }

[CqrsHandler]
public class VerifyStatusHandler(
    IChannelRepository channelRepository,
    IChannelScheduleRepository channelScheduleRepository,
    IChannelScheduleIntervalRepository channelScheduleIntervalRepository
) : ICommandHandler<VerifyStatusCommand, bool>
{
    [WorkflowStep(WorkflowStepCode.CTH.WF_STEP_CTH_VERIFY_CHANNEL_STATUS)]
    public async Task<bool> HandleAsync(
        VerifyStatusCommand request,
        CancellationToken cancellationToken = default
    )
    {
        string channelId = request.ChannelId;
        if (string.IsNullOrWhiteSpace(channelId))
        {
            throw new ArgumentException("ChannelId is required", nameof(channelId));
        }

        Domain.AggregatesModel.ChannelAggregate.Channel channel =
            await channelRepository.Table.FirstOrDefaultAsync(
                c => c.ChannelId == channelId,
                cancellationToken
            )
            ?? throw new O24OpenAPIException(
                ChannelErrorCodes.ChannelNotFound,
                $"Channel '{channelId}' not found."
            );

        if (!channel.Status)
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.ChannelDisabled,
                $"The Channel have been closed now. Please login in next time"
            );
        }

        if (channel.IsAlwaysOpen)
        {
            return true;
        }

        TimeZoneInfo tz;
        try
        {
            tz = TimeZoneInfo.FindSystemTimeZoneById(channel.TimeZoneId);
        }
        catch
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.InvalidTimezone,
                $"Invalid timezone '{channel.TimeZoneId}'."
            );
        }

        DateTimeOffset localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
        DayOfWeek todayEnum = localNow.DayOfWeek;
        TimeSpan nowTime = localNow.TimeOfDay; // TimeSpan

        ChannelSchedule schedule = await channelScheduleRepository.Table.FirstOrDefaultAsync(
            s => s.ChannelIdRef == channel.Id && s.DayOfWeek == todayEnum,
            cancellationToken
        );

        if (schedule == null || schedule.IsClosed)
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.ScheduleClosedToday,
                "Schedule working time is closed today."
            );
        }

        List<ChannelScheduleInterval> intervals = await channelScheduleIntervalRepository
            .Table.Where(iv => iv.ChannelScheduleIdRef == schedule.Id)
            .ToListAsync(cancellationToken);

        if (intervals.Count == 0)
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.NoIntervalsConfigured,
                "No working intervals configured."
            );
        }

        bool isOpenNow = intervals.Any(iv =>
        {
            TimeSpan start = iv.StartTime;
            TimeSpan end = iv.EndTime;

            if (end > start)
            {
                return nowTime >= start && nowTime <= end;
            }
            else if (end < start)
            {
                return nowTime >= start || nowTime <= end;
            }
            else
            {
                return false;
            }
        });

        if (!isOpenNow)
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.ClosedAtThisTime,
                "Outside business hours."
            );
        }

        return true;
    }
}
