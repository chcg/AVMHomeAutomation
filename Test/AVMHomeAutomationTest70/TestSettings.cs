﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVMHomeAutomationTest70
{
    public static class TestSettings
    {
        public const string Login = "Demo";
        public const string Password = "Demo-1234";
        public static TestDevice DeviceDect100Repeater = new()  { Ain = "11657 0196837", Name = "DECT Repeater",    Product = "FRITZ!DECT Repeater 100",    Manufacturer = "AVM", FirmwareVersion = "04.25" };
        public static TestDevice DeviceDect200Socket = new()    { Ain = "08761 0500005", Name = "Bodhran",          Product = "FRITZ!DECT 200",             Manufacturer = "AVM", FirmwareVersion = "04.25" };
        public static TestDevice DeviceDect210Socket = new()    { Ain = "11657 0143095", Name = "Weihnachtsbaum",   Product = "FRITZ!DECT 210",             Manufacturer = "AVM", FirmwareVersion = "04.25" };
        public static TestDevice DeviceDect301Radiator = new()  { Ain = "09995 0125605", Name = "Arbeitszimmer",    Product = "FRITZ!DECT 301",             Manufacturer = "AVM", FirmwareVersion = "05.02" };
        public static TestDevice DeviceDect400Switch = new()    { Ain = "13096 0015206", Name = "Taster",           Product = "FRITZ!DECT 400",             Manufacturer = "AVM", FirmwareVersion = "04.92" };
        public static TestDevice DeviceDect440Switch = new()    { Ain = "09995 0964673", Name = "Control",          Product = "FRITZ!DECT 404",             Manufacturer = "AVM", FirmwareVersion = "05.25" };
        public static TestDevice DeviceDect500Light = new();
        public static TestDevice DeviceHanFunDoorWindowContact = new() { Ain = "11934 0100883", Name = "Telekom", Product = "HAN-FUN", Manufacturer = "", FirmwareVersion = "31.20" };
        public const string UnknownDeviceAin = "111111111111";
    }

    // Radiator control
    // Switch
    // Socket
    // Additional devices
}
