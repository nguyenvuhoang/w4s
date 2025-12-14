using LinqToDB;
using O24OpenAPI.ControlHub.Constant;
using O24OpenAPI.ControlHub.Domain;
using O24OpenAPI.ControlHub.Models.Channel;
using O24OpenAPI.ControlHub.Services.Interfaces;
using O24OpenAPI.Core;
using O24OpenAPI.Core.Caching;

namespace O24OpenAPI.ControlHub.Services;

public class ChannelService(
 IRepository<Channel> channelRepo,
 IRepository<ChannelSchedule> scheduleRepo,
 IRepository<ChannelScheduleInterval> intervalRepo,
 IRepository<ChannelUserOverride> userOverride,
 IRepository<ChannelUserOverrideInterval> userOverrideInterval,
 IStaticCacheManager staticCacheManager
) : IChannelService
{
    private readonly IRepository<ChannelSchedule> _scheduleRepo = scheduleRepo;
    private readonly IRepository<Channel> _channelRepo = channelRepo;
    private readonly IRepository<ChannelScheduleInterval> _intervalRepo = intervalRepo;
    private readonly IRepository<ChannelUserOverride> _userOverrideRepo = userOverride;
    private readonly IRepository<ChannelUserOverrideInterval> _userOverrideIntervalRepo = userOverrideInterval;
    private readonly IStaticCacheManager _staticCacheManager = staticCacheManager;

    public async Task<List<ChannelVm>> GetChannelsWithWeeklyAsync(CancellationToken ct = default)
    {
        var channels = await _channelRepo.Table
            .OrderBy(c => c.SortOrder)
            .ThenBy(c => c.ChannelId)
            .Select(c => new ChannelVm
            {
                Id = c.Id,
                ChannelId = c.ChannelId,
                ChannelName = c.ChannelName,
                Description = c.Description,
                Status = c.Status,
                IsAlwaysOpen = c.IsAlwaysOpen,
                TimeZoneId = c.TimeZoneId
            })
            .ToListAsync(ct);

        if (channels.Count == 0)
        {
            return channels;
        }

        var channelIds = channels.Select(x => x.Id).ToArray();

        if (_scheduleRepo?.Table == null)
        {
            return [];
        }
        // lấy schedule trong tuần cho các channel
        var schedules = await _scheduleRepo.Table
            .Where(s => channelIds.Contains(s.ChannelIdRef))
            .Select(s => new
            {
                s.Id,
                s.ChannelIdRef,
                s.DayOfWeek,
                s.IsClosed
            })
            .ToListAsync(ct);

        if (schedules.Count == 0)
        {
            return channels;
        }

        var scheduleIds = schedules.Select(x => x.Id).ToArray();

        // lấy intervals
        var intervals = await _intervalRepo.Table
         .Where(iv => scheduleIds.Contains(iv.ChannelScheduleIdRef))
         .OrderBy(iv => iv.SortOrder)
         .Select(iv => new
         {
             iv.ChannelScheduleIdRef,
             iv.StartTime,
             iv.EndTime,
             iv.SortOrder
         })
         .ToListAsync(ct);

        var result = intervals.Select(iv => new
        {
            iv.ChannelScheduleIdRef,
            StartTime = TimeOnly.FromTimeSpan(iv.StartTime).ToString("HH:mm"),
            EndTime = TimeOnly.FromTimeSpan(iv.EndTime).ToString("HH:mm"),
            iv.SortOrder
        }).ToList();

        // group intervals theo scheduleId
        var intervalsBySchedule = intervals
            .GroupBy(x => x.ChannelScheduleIdRef)
            .ToDictionary(g => g.Key, g => g.ToList());

        // group schedules theo channelId
        var schedulesByChannel = schedules
            .GroupBy(s => s.ChannelIdRef)
            .ToDictionary(g => g.Key, g => g.ToList());

        static int NormalizeDow(DayOfWeek d) => ((int)d + 6) % 7;

        foreach (var ch in channels)
        {
            if (!schedulesByChannel.TryGetValue(ch.Id, out var scList) || scList == null)
            {
                ch.Weekly = EnsureSevenDays(new List<ChannelDayVm>());
                continue;
            }

            var weekly = scList
                .OrderBy(s => NormalizeDow(s.DayOfWeek)) // Thứ 2 -> CN
                .Select(s =>
                {
                    var day = new ChannelDayVm
                    {
                        DayOfWeek = (int)s.DayOfWeek,
                        DayName = DayName(s.DayOfWeek),
                        IsClosed = s.IsClosed
                    };

                    if (!s.IsClosed && intervalsBySchedule.TryGetValue(s.Id, out var ivs) && ivs != null)
                    {
                        var ordered = ivs.OrderBy(iv => iv.SortOrder)
                                         .ThenBy(iv => iv.StartTime);

                        foreach (var iv in ordered)
                        {
                            day.Intervals.Add(new ChannelIntervalVm
                            {
                                Start = iv.StartTime.ToString(@"hh\:mm"),
                                End = iv.EndTime.ToString(@"hh\:mm")
                            });
                        }
                    }

                    return day;
                })
                .ToList();

            weekly = EnsureSevenDays(weekly);

            ch.Weekly = weekly;

            var tz = TimeZoneInfo.FindSystemTimeZoneById(ch.TimeZoneId);
            var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);

            var todayIndex = (int)localNow.DayOfWeek;

            foreach (var d in weekly)
            {
                d.IsToday = d.DayOfWeek == todayIndex;
            }

            bool openNow = ch.IsAlwaysOpen;
            if (!openNow)
            {

                var today = weekly.First(w => w.DayOfWeek == todayIndex);
                if (!today.IsClosed && today.Intervals.Count > 0)
                {
                    var nowT = TimeOnly.FromDateTime(localNow.DateTime);
                    openNow = today.Intervals.Any(iv =>
                    {
                        var start = TimeOnly.Parse(iv.Start);
                        var end = TimeOnly.Parse(iv.End);
                        return end > start ? (nowT >= start && nowT <= end) : (nowT >= start || nowT <= end);
                    });
                }
            }
            ch.IsOpenNow = openNow;
        }

        return channels;
    }

    public async Task<ChannelVm> GetChannelByCodeAsync(string channelId, CancellationToken ct = default)
    {
        var list = await GetChannelsWithWeeklyAsync(ct);
        return list.FirstOrDefault(c => c.ChannelId.Equals(channelId, StringComparison.OrdinalIgnoreCase));
    }

    private static string DayName(DayOfWeek d)
    {
        return d switch
        {
            DayOfWeek.Monday => "Monday",
            DayOfWeek.Tuesday => "Tuesday",
            DayOfWeek.Wednesday => "Wednesday",
            DayOfWeek.Thursday => "Thursday",
            DayOfWeek.Friday => "Friday",
            DayOfWeek.Saturday => "Saturday",
            DayOfWeek.Sunday => "Sunday",
            _ => "Unknow"
        };
    }
    static int NormalizeDow(DayOfWeek d) => ((int)d + 6) % 7;

    static List<ChannelDayVm> EnsureSevenDays(List<ChannelDayVm> source)
    {
        var map = source
            .GroupBy(x => x.DayOfWeek)
            .ToDictionary(g => g.Key, g => g
                .OrderBy(x => x.Intervals?.Count ?? 0) // tuỳ ý, chỉ cần pick 1
                .First());

        // Duyệt Monday..
        var ordered = new List<ChannelDayVm>(7);
        var allDays = new[]
        {
            DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
        };

        foreach (var dow in allDays)
        {
            var key = (int)dow;
            if (!map.TryGetValue(key, out var existing) || existing == null)
            {
                // Mặc định ngày thiếu là nghỉ
                existing = new ChannelDayVm
                {
                    DayOfWeek = key,
                    DayName = DayName(dow),
                    IsClosed = true,
                    Intervals = []
                };
            }
            // Bảo đảm không null list
            existing.Intervals ??= [];
            ordered.Add(existing);
        }

        // Sắp xếp chuẩn Monday..Sunday
        return [.. ordered.OrderBy(d => NormalizeDow((DayOfWeek)d.DayOfWeek))];
    }

    public async Task<ChannelVm> UpdateChannelStatusAsync(string channelId, bool isOpen, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(channelId))
        {
            throw new ArgumentException("channelId is required", nameof(channelId));
        }

        var channel = await _channelRepo.Table
            .FirstOrDefaultAsync(c => c.ChannelId == channelId, ct)
            ?? throw new InvalidOperationException($"Channel '{channelId}' not found.");

        if (channel.Status == isOpen)
        {
            await InvalidateChannelActiveCacheAsync(channelId);

            var existed = await GetChannelByCodeAsync(channelId, ct);
            if (existed is not null)
            {
                return existed;
            }

            return new ChannelVm
            {
                Id = channel.Id,
                ChannelId = channel.ChannelId,
                ChannelName = channel.ChannelName,
                Description = channel.Description,
                Status = channel.Status,
                IsAlwaysOpen = channel.IsAlwaysOpen,
                TimeZoneId = channel.TimeZoneId,
                Weekly = []
            };
        }

        channel.Status = isOpen;
        await _channelRepo.Update(channel);

        await InvalidateChannelActiveCacheAsync(channelId);

        var updated = await GetChannelByCodeAsync(channelId, ct);
        if (updated is null)
        {
            return new ChannelVm
            {
                Id = channel.Id,
                ChannelId = channel.ChannelId,
                ChannelName = channel.ChannelName,
                Description = channel.Description,
                Status = channel.Status,
                IsAlwaysOpen = channel.IsAlwaysOpen,
                TimeZoneId = channel.TimeZoneId,
                Weekly = []
            };
        }

        return updated;
    }


    /// <summary>
    /// Is channel active now?
    /// </summary>
    /// <param name="channelId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="O24OpenAPIException"></exception>
    public async Task<bool> IsChannelActiveAsync(string channelId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(channelId))
        {
            throw new ArgumentException("ChannelId is required", nameof(channelId));
        }

        var channel = await _channelRepo.Table
            .FirstOrDefaultAsync(c => c.ChannelId == channelId, ct)
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

        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
        var todayEnum = localNow.DayOfWeek;
        var nowTime = localNow.TimeOfDay; // TimeSpan

        var schedule = await _scheduleRepo.Table
            .FirstOrDefaultAsync(s => s.ChannelIdRef == channel.Id && s.DayOfWeek == todayEnum, ct);

        if (schedule == null || schedule.IsClosed)
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.ScheduleClosedToday,
                "Schedule working time is closed today."
            );
        }

        var intervals = await _intervalRepo.Table
            .Where(iv => iv.ChannelScheduleIdRef == schedule.Id)
            .ToListAsync(ct);

        if (intervals.Count == 0)
        {
            throw new O24OpenAPIException(
                ChannelErrorCodes.NoIntervalsConfigured,
                "No working intervals configured."
            );
        }

        var isOpenNow = intervals.Any(iv =>
        {
            var start = iv.StartTime;
            var end = iv.EndTime;

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





    /// <summary>
    /// Can user login now?
    /// </summary>
    /// <param name="channelId"></param>
    /// <param name="userId"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="BusinessException"></exception>
    public async Task<bool> CanLoginAsync(string channelId, string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(channelId))
        {
            throw new ArgumentException(nameof(channelId));
        }

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException(nameof(userId));
        }

        var cacheKey = new CacheKey($"channel:canlogin:{channelId}:{userId}");
        var cached = await _staticCacheManager.Get<bool?>(cacheKey);
        if (cached.HasValue)
        {
            return cached.Value;
        }

        var channel = await _channelRepo.Table.FirstOrDefaultAsync(c => c.ChannelId == channelId, ct)
                     ?? throw new O24OpenAPIException($"Channel '{channelId}' not found.");

        var tz = TimeZoneInfo.FindSystemTimeZoneById(channel.TimeZoneId);
        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
        var nowDate = localNow.DateTime.Date;
        var nowTime = localNow.TimeOfDay;
        var todayIdx = (int)localNow.DayOfWeek;

        if (!channel.Status)
        {
            var ov = await _userOverrideRepo.Table
                .Where(x => x.ChannelIdRef == channel.Id && x.UserId == userId
                    && (!x.EffectiveFrom.HasValue || x.EffectiveFrom.Value.Date <= nowDate)
                    && (!x.EffectiveTo.HasValue || nowDate <= x.EffectiveTo.Value.Date)
                    && x.AllowWhenDisabled)
                .FirstOrDefaultAsync(ct);

            if (ov == null)
            {
                await _staticCacheManager.Set(cacheKey, false);
                return false;
            }

            bool ok = ov.IsAllowedAllDay || await IsInUserOverrideIntervalsAsync(ov.Id, nowTime, ct);
            await _staticCacheManager.Set(cacheKey, ok);
            return ok;
        }

        if (channel.IsAlwaysOpen)
        {
            await _staticCacheManager.Set(cacheKey, true);
            return true;
        }
        var todayEnum = (DayOfWeek)todayIdx;

        var schedule = await _scheduleRepo.Table
            .FirstOrDefaultAsync(s => s.ChannelIdRef == channel.Id && s.DayOfWeek == todayEnum, ct);

        bool openBySchedule = false;
        if (schedule is not null && !schedule.IsClosed)
        {
            var intervals = await _intervalRepo.Table
                .Where(iv => iv.ChannelScheduleIdRef == schedule.Id)
                .ToListAsync(ct);

            openBySchedule = intervals.Any(iv =>
            {
                var start = iv.StartTime;
                var end = iv.EndTime;
                return end > start ? (nowTime >= start && nowTime <= end)
                                   : (nowTime >= start || nowTime <= end);
            });
        }

        if (openBySchedule)
        {
            await _staticCacheManager.Set(cacheKey, true);
            return true;
        }

        var userOv = await _userOverrideRepo.Table
            .Where(x => x.ChannelIdRef == channel.Id && x.UserId == userId
                && (!x.EffectiveFrom.HasValue || x.EffectiveFrom.Value.Date <= nowDate)
                && (!x.EffectiveTo.HasValue || nowDate <= x.EffectiveTo.Value.Date))
            .FirstOrDefaultAsync(ct);

        if (userOv == null)
        {
            await _staticCacheManager.Set(cacheKey, false);
            return false;
        }

        bool allowed = userOv.IsAllowedAllDay || await IsInUserOverrideIntervalsAsync(userOv.Id, nowTime, ct);
        await _staticCacheManager.Set(cacheKey, allowed);
        return allowed;
    }

    private async Task<bool> IsInUserOverrideIntervalsAsync(int userOverrideId, TimeSpan nowTime, CancellationToken ct)
    {
        var ivs = await _userOverrideIntervalRepo.Table
            .Where(i => i.ChannelUserOverrideIdRef == userOverrideId)
            .OrderBy(i => i.SortOrder)
            .ToListAsync(ct);

        if (ivs.Count == 0)
        {
            return false;
        }

        return ivs.Any(iv =>
        {
            var start = iv.StartTime;
            var end = iv.EndTime;
            return end > start ? (nowTime >= start && nowTime <= end)
                               : (nowTime >= start || nowTime <= end);
        });
    }

    private static CacheKey BuildActiveCacheKey(string channelId)
    => new($"channel:active:{channelId}");

    private async Task InvalidateChannelActiveCacheAsync(string channelId)
    {
        var key = BuildActiveCacheKey(channelId);

        // Tuỳ IStaticCacheManager bạn đang dùng, 1 trong 2 (hoặc cả hai) method dưới sẽ tồn tại:
        try { await _staticCacheManager.Remove(key); } catch { /* ignore if not supported */ }
        try { await _staticCacheManager.RemoveByPrefix("channel:active:"); } catch { /* ignore */ }
    }

}
