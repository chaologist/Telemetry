namespace Telemetry.Tests
open Xunit
type testSteps= |Step1=0 | Step2=1

type Tests() =

    [<Fact>]
    member public this.TrivialTest() =
        ()
    
    [<Fact>]
    member public this.HappyTest()=
        let mutable c = 0
        let doSomething x =
            c<-c+1
            x
        let subject = new Telemetry.Sinks.Telemetry(Seq.empty)
        let test = subject.Wrap (testSteps.Step1, doSomething)
        let result = test 5

        Assert.Equal (1,c)
        Assert.Equal (5,result)
        ()
    [<Fact>]
    member public this.OneSink()=
        let mutable c = [||]
        let doSomething x =
            x
        let sink (msg:string)=
            c <-Array.append c [|msg|]

        let subject = new Telemetry.Sinks.Telemetry([|sink|])
        let test = subject.Wrap (testSteps.Step1, doSomething)
        let result = test 5

        System.Threading.Thread.Sleep(1000) //the actual logging is async in agents... so we need to wait to let it finish.  
                                            //TODO: come up with a better way to assure completion
        Assert.Equal (5,result)
        Assert.Equal (2,c.Length)  //2, one pre, one post.
        ()
    [<Fact>]
    member public this.MessageStartSerialize()=
        let mutable c = [||]
        let doSomething x =
            x
        let sink (msg:string)=
            c <-Array.append c [|msg|]

        let subject = new Telemetry.Sinks.Telemetry([|sink|])
        let test = subject.Wrap (testSteps.Step1, doSomething)
        let result = test 5

        System.Threading.Thread.Sleep(1000) //the actual logging is async in agents... so we need to wait to let it finish.  
                                            //TODO: come up with a better way to assure completion
        Assert.Equal (5,result)
        Assert.Equal ("{\"step\":\"Step1\",\"event\":{\"Case\":\"Start\"}}",c.[0])
        ()

    [<Fact>]
    member public this.TwoSinksOneBad()=
        let mutable c = [||]
        let doSomething x =
            x
        let sink (msg:string)=
            c <-Array.append c [|msg|]
        let badSink (msg:string)=
            raise (new System.Exception())

        let subject = new Telemetry.Sinks.Telemetry([|sink;badSink|])
        let test = subject.Wrap (testSteps.Step1, doSomething)
        let result= test 5
        let result2 = test result

        System.Threading.Thread.Sleep(1000) //the actual logging is async in agents... so we need to wait to let it finish.  
                                            //TODO: come up with a better way to assure completion
        Assert.Equal (5,result)
        Assert.Equal (4,c.Length)  //2, one pre, one post.
        ()
