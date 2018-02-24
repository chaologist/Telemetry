namespace Telemetry.Log4Net
open Telemetry.Sinks
module Sink=

    let logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    let Sink (string:string) =
        logger.Info (string)
