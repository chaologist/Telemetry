namespace Telemetry.ApplicationInsights
open Telemetry.Sinks
open System;
open System.IO;
open Microsoft.Extensions.Configuration;
module Sink=

    let configuration = (new ConfigurationBuilder()).SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Telemetry.ApplicationInsights.json").Build()
    let client = new Microsoft.ApplicationInsights.TelemetryClient ()

    let Sink (string:string) =
        client.TrackTrace (string)

    do
        client.InstrumentationKey<-configuration.["iKey"]
        