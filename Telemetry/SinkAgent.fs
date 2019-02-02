namespace Telemetry
module SinkAgent =
    open Types

    let MakeAgent (sink:string->unit)=
        MailboxProcessor.Start(fun inbox-> 

            let serialize (msg:Message)=
                Newtonsoft.Json.JsonConvert.SerializeObject (msg,Newtonsoft.Json.Converters.StringEnumConverter())

            // the message processing function
            let rec messageLoop() = async{
        
                // read a message
                let! msg = inbox.Receive()

                let serializedMessage = msg |> serialize
        
                // process a message
                sink serializedMessage

                // loop to top
                return! messageLoop()  
                }

            // start the loop 
            messageLoop() 
        )


