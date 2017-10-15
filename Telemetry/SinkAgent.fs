module SinkAgent

    let MakeAgent (sink:string->unit)=
        MailboxProcessor.Start(fun inbox-> 

            // the message processing function
            let rec messageLoop() = async{
        
                // read a message
                let! msg = inbox.Receive()
        
                // process a message
                sink msg

                // loop to top
                return! messageLoop()  
                }

            // start the loop 
            messageLoop() 
        )


