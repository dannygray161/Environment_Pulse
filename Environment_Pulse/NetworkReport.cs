using System;
using System.Collections.Generic;
using System.Text;

namespace Environment_Pulse
{
    public class NetworkReport
    {
        // ==========================================================
        // BLUEPRINT: NetworkReport.cs
        // GOAL: Create a digital "manifest" to hold our pulse data.
        // ==========================================================

        // Property 1: A string to hold the status (e.g., "Active", "Offline")
        // Name: Status
        public string sStatus { get; set; }

        // Property 2: A string for the data center location (e.g., "STL")
        // Name: EndPointLocation
        public string sEndPointLocation { get; set; }

        // Property 3: A long integer for the round-trip speed in milliseconds
        // Name: Latency
        public long sLatency { get; set; }

        // Property 4: A DateTime object to record the exact time of the check
        // Name: Timestamp
        public DateTime tTimeStamp { get; set; }

        // Property 5: A boolean (true/false) for the router's health
        // Name: IsLocalHardwareUp
        public bool lIsLocalHardwareUp { get; set; }

        // Property 6: A boolean (true/false) for the ISP's health
        // Name: IsIspBackboneUp
        public bool lIsISPBackboneUp { get; set; }

        // Property 7: A boolean to flag potential speed throttling
        // Name: ThrottlingWarning
        public bool lThrottleWarning { get; set; }

        // Property 8: A string to store the raw text from the API for debugging
        // Name: RawTraceData
        public string sRawTraceData { get; set; }
    }
}
