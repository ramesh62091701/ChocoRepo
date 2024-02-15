using Akka.Actor;
using AkkaDemo2.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace AkkaDemo2.Actor
{
    public class MainActor : UntypedActor
    {

        public MainActor()
        {
           
        }
    
        protected override void OnReceive(object message)
        {
            Trace.WriteLine("Actor Started");

            switch (message)
            {
                case "Get":
                    Trace.WriteLine("Get command received");
                    break;

                case AddMessageCommand addMessageCommand:
                    Trace.WriteLine($"Add message command received: {addMessageCommand.Message}");
                    break;

                case "Clear":
                    Trace.WriteLine("Clear command received");

                    break;

                default:
                    Trace.WriteLine($"Unknown message received: {message}");
                    break;
            }
        }

        protected override void PreStart() => Trace.WriteLine("Actor Started!");
        protected override void PostStop() => Trace.WriteLine("Actor Stopped!");
    }

    public class AddMessageCommand
    {
        public string Message { get; }

        public AddMessageCommand(string message)
        {
            Message = message;
        }
    }
}