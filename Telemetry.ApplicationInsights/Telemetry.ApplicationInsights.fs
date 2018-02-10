namespace Telemetry.ApplicationInsights
open Telemetry.Sinks
module Sink=

    let client = new Microsoft.ApplicationInsights.TelemetryClient ()

    let Sink (string:string) =
        client.TrackTrace (string)
        