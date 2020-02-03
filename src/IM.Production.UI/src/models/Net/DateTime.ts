import { TimeSpan } from "./TimeSpan";

export class DateTime {
    constructor(
        public now: DateTime,
        public today: DateTime,
        public utcNow: DateTime,
        public ticks: number,
        public second: number,
        public date: DateTime,
        public month: number,
        public minute: number,
        public millisecond: number,
        public kind: number,
        public hour: number,
        public dayOfYear: number,
        public dayOfWeek: number,
        public day: number,
        public timeOfDay: TimeSpan,
        public year: number
    )
    {
    }
}