namespace Telemetry
open System.Runtime.Serialization
module Types =
    [<DataContract>]
    type Event = | Start | End of int64 | Exception of (System.Exception)
    [<DataContract>]
    type Message = {
        [<field: DataMember(Name="step") >]
         Step:System.Enum;
        [<field: DataMember(Name="event") >]
        Event:Event
     }
    type MessageSerializer = Message->string

