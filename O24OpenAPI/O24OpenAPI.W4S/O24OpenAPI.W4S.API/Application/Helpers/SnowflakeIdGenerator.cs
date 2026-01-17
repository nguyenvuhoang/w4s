namespace O24OpenAPI.W4S.API.Application.Helpers;

public class SnowflakeTransactionNumberGenerator
{
    private static readonly DateTime _epoch = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private readonly object _lock = new();
    private readonly long _machineId;
    private const int MachineIdBits = 10;
    private const int SequenceBits = 12;

    private const long MaxMachineId = -1L ^ (-1L << MachineIdBits);
    private const long MaxSequence = -1L ^ (-1L << SequenceBits);

    private long _lastTimestamp = -1L;
    private long _sequence = 0L;

    public SnowflakeTransactionNumberGenerator(long machineId = 1)
    {
        if (machineId < 0 || machineId > MaxMachineId)
        {
            throw new ArgumentException($"Machine ID must be between 0 and {MaxMachineId}");
        }

        _machineId = machineId;
    }

    public string GenerateTransactionNumber()
    {
        long id = GenerateId();
        return id.ToString();
    }

    private long GenerateId()
    {
        lock (_lock)
        {
            long timestamp = GetCurrentTimestamp();

            if (timestamp < _lastTimestamp)
            {
                throw new InvalidOperationException("System clock moved backwards.");
            }

            if (timestamp == _lastTimestamp)
            {
                _sequence = (_sequence + 1) & MaxSequence;
                if (_sequence == 0)
                {
                    timestamp = WaitNextMillis(_lastTimestamp);
                }
            }
            else
            {
                _sequence = 0;
            }

            _lastTimestamp = timestamp;

            return (timestamp << (MachineIdBits + SequenceBits)) |
                   (_machineId << SequenceBits) |
                   _sequence;
        }
    }

    private static long GetCurrentTimestamp()
    {
        return (long)(DateTime.UtcNow - _epoch).TotalMilliseconds;
    }

    private static long WaitNextMillis(long lastTimestamp)
    {
        long timestamp = GetCurrentTimestamp();
        while (timestamp <= lastTimestamp)
        {
            timestamp = GetCurrentTimestamp();
        }
        return timestamp;
    }
}
