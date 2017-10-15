﻿namespace Telemetry

module private types =
    type Event = | Start | End of int64 | Exception of (System.Exception)
    type Message = {Step:System.Enum;Event:Event}
    type MessageSerializer = Message->string

module Sinks =
    open types
    open SinkAgent
    type TelemetrySink = string->unit

    type Telemetry(sinks: seq<TelemetrySink>) = 
        let sinkAgents = sinks |> Seq.map MakeAgent |> Array.ofSeq 
        let serialize (msg:Message)=
            Newtonsoft.Json.JsonConvert.SerializeObject msg
        let sinkMsg msg=
            let serializedMessage = serialize msg
            sinkAgents |> Array.iter (fun snk-> snk.Post serializedMessage)

        member __.Wrap (step:System.Enum,f:'a->'b) =
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