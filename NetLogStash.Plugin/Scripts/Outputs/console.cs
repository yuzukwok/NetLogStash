using NetLogStash;
using NetLogStash.Output;
using System;
using NetLogStash.Config;
using System.Collections.Generic;
/// <summary>
/// Writes events to Console. Format is: "[TimeStamp] [Type(:Id)] {0}" where {0} is all dynamic properties converted to JSON.
/// </summary>
public class consoleOutput : AbstractOutput
{
    public override void Execute(Event value)
    {
        //this.Log().Info("Execute");
        Console.WriteLine(value.AsString());
    }

   

    public override void Initialize(string name, Dictionary<string, ParaItem> para)
    {
        //this.Log().Info("Initialize");
    }
}