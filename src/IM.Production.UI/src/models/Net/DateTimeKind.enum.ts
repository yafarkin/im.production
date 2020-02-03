// Summary:
// Specifies whether a System.DateTime object represents a local time, a Coordinated
// Universal Time (UTC), or is not specified as either local time or UTC.
export enum DateTimeKind {
    // Summary:
    // The time represented is not specified as either local time or Coordinated Universal
    // Time (UTC).
    Unspecified = 0,
    //
    // Summary:
    // The time represented is UTC.
    Utc = 1,
    //
    // Summary:
    // The time represented is local time.
    Local = 2
}