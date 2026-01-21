namespace O24OpenAPI.W4S.API.Application.Helpers
{
    /// <summary>
    /// Defines the <see cref="WalletPeriodType" />
    /// </summary>
    public static class WalletPeriodType
    {
        /// <summary>
        /// The BuildPeriodRangeUtc
        /// </summary>
        /// <param name="nowUtc">The nowUtc<see cref="DateTime"/></param>
        /// <param name="periodType">The periodType<see cref="string"/></param>
        /// <returns>The <see cref="(DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo)"/></returns>
        public static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildPeriodRangeUtc(
            DateTime nowUtc,
            string periodType
        )
        {
            periodType = (periodType ?? "M").Trim().ToUpperInvariant();

            static DateTime StartOfDay(DateTime dt) =>
                new(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);

            static DateTime StartOfMonth(DateTime dt) =>
                new(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            static DateTime StartOfYear(DateTime dt) =>
                new(dt.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            static DateTime StartOfQuarter(DateTime dt)
            {
                var q = (dt.Month - 1) / 3;          // 0..3
                var startMonth = q * 3 + 1;          // 1,4,7,10
                return new(dt.Year, startMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            }

            static DateTime StartOfHalfYear(DateTime dt)
            {
                var startMonth = dt.Month <= 6 ? 1 : 7;
                return new(dt.Year, startMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            }

            return periodType switch
            {
                "D" => (
                    thisFrom: StartOfDay(nowUtc),
                    thisTo: StartOfDay(nowUtc).AddDays(1),
                    prevFrom: StartOfDay(nowUtc).AddDays(-1),
                    prevTo: StartOfDay(nowUtc)
                ),

                "M" => (
                    thisFrom: StartOfMonth(nowUtc),
                    thisTo: StartOfMonth(nowUtc).AddMonths(1),
                    prevFrom: StartOfMonth(nowUtc).AddMonths(-1),
                    prevTo: StartOfMonth(nowUtc)
                ),

                "Q" => (
                    thisFrom: StartOfQuarter(nowUtc),
                    thisTo: StartOfQuarter(nowUtc).AddMonths(3),
                    prevFrom: StartOfQuarter(nowUtc).AddMonths(-3),
                    prevTo: StartOfQuarter(nowUtc)
                ),

                "H" => (
                    thisFrom: StartOfHalfYear(nowUtc),
                    thisTo: StartOfHalfYear(nowUtc).AddMonths(6),
                    prevFrom: StartOfHalfYear(nowUtc).AddMonths(-6),
                    prevTo: StartOfHalfYear(nowUtc)
                ),

                "Y" => (
                    thisFrom: StartOfYear(nowUtc),
                    thisTo: StartOfYear(nowUtc).AddYears(1),
                    prevFrom: StartOfYear(nowUtc).AddYears(-1),
                    prevTo: StartOfYear(nowUtc)
                ),

                _ => (
                    thisFrom: StartOfMonth(nowUtc),
                    thisTo: StartOfMonth(nowUtc).AddMonths(1),
                    prevFrom: StartOfMonth(nowUtc).AddMonths(-1),
                    prevTo: StartOfMonth(nowUtc)
                )
            };
        }

        private static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildPeriodRangeUtc(
            DateTime nowUtc,
            string? periodType,
            string? periodUnit
        )
        {
            periodType = string.IsNullOrWhiteSpace(periodType) ? "M" : periodType.Trim().ToUpperInvariant();
            periodUnit = string.IsNullOrWhiteSpace(periodUnit) ? null : periodUnit.Trim();

            return periodType switch
            {
                "D" => BuildDaily(nowUtc, periodUnit),
                "M" => BuildMonthly(nowUtc, periodUnit),
                "Q" => BuildQuarterly(nowUtc, periodUnit),
                "H" => BuildHalfYear(nowUtc, periodUnit),
                "Y" => BuildYearly(nowUtc, periodUnit),
                _ => BuildMonthly(nowUtc, periodUnit)
            };
        }

        private static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildDaily(
            DateTime nowUtc,
            string? unit
        )
        {
            _ = nowUtc.Date;
            DateTime day;
            if (!string.IsNullOrWhiteSpace(unit))
            {
                if (!DateTime.TryParse(unit, out var parsed))
                    throw new ArgumentException("PeriodUnit for D must be yyyy-MM-dd", nameof(unit));

                day = DateTime.SpecifyKind(parsed.Date, DateTimeKind.Utc);
            }
            else
            {
                day = DateTime.SpecifyKind(nowUtc.Date, DateTimeKind.Utc);
            }

            var thisFrom = day;
            var thisTo = day.AddDays(1);

            var prevFrom = thisFrom.AddDays(-1);
            var prevTo = thisFrom;

            return (thisFrom, thisTo, prevFrom, prevTo);
        }

        private static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildMonthly(
            DateTime nowUtc,
            string? unit
        )
        {
            // unit: "1..12" => month in current year
            var year = nowUtc.Year;
            var month = nowUtc.Month;

            if (!string.IsNullOrWhiteSpace(unit))
            {
                if (!int.TryParse(unit, out var m) || m < 1 || m > 12)
                    throw new ArgumentException("PeriodUnit for M must be month number 1..12", nameof(unit));

                month = m;
            }

            var thisFrom = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var thisTo = thisFrom.AddMonths(1);

            var prevFrom = thisFrom.AddMonths(-1);
            var prevTo = thisFrom;

            return (thisFrom, thisTo, prevFrom, prevTo);
        }

        private static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildQuarterly(
            DateTime nowUtc,
            string? unit
        )
        {
            // unit: "1..4"
            var year = nowUtc.Year;
            var q = (nowUtc.Month - 1) / 3 + 1;

            if (!string.IsNullOrWhiteSpace(unit))
            {
                if (!int.TryParse(unit, out var qq) || qq < 1 || qq > 4)
                    throw new ArgumentException("PeriodUnit for Q must be quarter number 1..4", nameof(unit));

                q = qq;
            }

            var startMonth = (q - 1) * 3 + 1;
            var thisFrom = new DateTime(year, startMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            var thisTo = thisFrom.AddMonths(3);

            var prevFrom = thisFrom.AddMonths(-3);
            var prevTo = thisFrom;

            return (thisFrom, thisTo, prevFrom, prevTo);
        }

        private static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildHalfYear(
            DateTime nowUtc,
            string? unit
        )
        {
            // unit: "1" (Jan-Jun) or "2" (Jul-Dec)
            var year = nowUtc.Year;
            var h = nowUtc.Month <= 6 ? 1 : 2;

            if (!string.IsNullOrWhiteSpace(unit))
            {
                if (!int.TryParse(unit, out var hh) || hh < 1 || hh > 2)
                    throw new ArgumentException("PeriodUnit for H must be 1 or 2", nameof(unit));

                h = hh;
            }

            var startMonth = h == 1 ? 1 : 7;
            var thisFrom = new DateTime(year, startMonth, 1, 0, 0, 0, DateTimeKind.Utc);
            var thisTo = thisFrom.AddMonths(6);

            var prevFrom = thisFrom.AddMonths(-6);
            var prevTo = thisFrom;

            return (thisFrom, thisTo, prevFrom, prevTo);
        }

        private static (DateTime thisFrom, DateTime thisTo, DateTime prevFrom, DateTime prevTo) BuildYearly(
            DateTime nowUtc,
            string? unit
        )
        {
            // unit: "yyyy"
            var year = nowUtc.Year;

            if (!string.IsNullOrWhiteSpace(unit))
            {
                if (!int.TryParse(unit, out var y) || y < 1900 || y > 3000)
                    throw new ArgumentException("PeriodUnit for Y must be year yyyy", nameof(unit));

                year = y;
            }

            var thisFrom = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var thisTo = thisFrom.AddYears(1);

            var prevFrom = thisFrom.AddYears(-1);
            var prevTo = thisFrom;

            return (thisFrom, thisTo, prevFrom, prevTo);
        }

    }



}
