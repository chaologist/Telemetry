namespace Telemetry
module Sinks =
    open Types
    open SinkAgent
    type TelemetrySink = string->unit

    let Wrap (sinks: seq<TelemetrySink>) (step:System.Enum) (f:'a->'b)=
        let sinkAgents = sinks |> Seq.map MakeAgent |> Array.ofSeq 
        let sinkMsg msg=
            sinkAgents |> Array.iter (fun snk-> snk.Post msg)
        fun (someA)->
            try 
                sinkMsg {Step=step;Event=Start}
                let clock = System.Diagnostics.Stopwatch.StartNew()
                let result =f someA
                clock.Stop();
                sinkMsg {Step=step;Event=End(clock.ElapsedMilliseconds)}
                result
            with ex->
                sinkMsg {Step=step;Event=Exception(ex)}
                raise ex


    type Telemetry(sinks: seq<TelemetrySink>) = 
        member __.Wrap (step:System.Enum,f:'a->'b) =
            Wrap sinks step f
