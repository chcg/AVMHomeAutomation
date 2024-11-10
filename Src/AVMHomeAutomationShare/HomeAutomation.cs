﻿namespace AVMHomeAutomation;

/// <summary>
/// AVM Home Automation service class.
/// </summary>
public sealed class HomeAutomation(string login, string password, Uri? host = null) : IDisposable
{
    private HomeAutomationService? service = new(login, password, host);

    public void Dispose()
    {
        if (this.service != null)
        {
            this.service.Dispose();
            this.service = null;
        }
        GC.SuppressFinalize(this);
    }


    #region Public Methods

    /// <summary>
    /// Returns the AIN / MAC list of all known sockets.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<string[]?> GetSwitchListAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return await service!.GetSwitchListAsync(cancellationToken);
    }
    
    /// <summary>
    /// Turns on the socket.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<bool> SetSwitchOnAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return await service!.SetSwitchOnAsync(ain, cancellationToken);
    }
    
    /// <summary>
    /// Switches off the socket.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<bool> SetSwitchOffAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return await service!.SetSwitchOffAsync(ain, cancellationToken);
    }

    /// <summary>
    /// Toggle the power socket on / off.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<bool> SetSwitchToggleAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return await service!.SetSwitchToggleAsync(ain, cancellationToken);
    }

    /// <summary>
    /// Determines the switching state of the socket.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Changes if the connection is lost the condition only with some Minutes delay to false.</remarks>
    public async Task<bool?> GetSwitchStateAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return await service.GetSwitchStateAsync(ain);
    }
    
    /// <summary>
    /// Determines the connection status of the actuator.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<bool> GetSwitchPresentAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getswitchpresent", ain))).ToBool();
    }

    /// <summary>
    /// Determines current power taken from the power outlet.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double?> GetSwitchPowerAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getswitchpower", ain))).ToPower();
    }

    /// <summary>
    /// Delivers the amount of energy taken from the socket since commissioning or resetting the energy statistics.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double?> GetSwitchEnergyAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getswitchenergy", ain))).ToPower();
    }

    /// <summary>
    /// Returns identifiers of the actor.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<string> GetSwitchNameAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getswitchname", ain))).TrimEnd();
    }

    /// <summary>
    /// Provides the basic information of all SmartHome devices.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<DeviceList> GetDeviceListInfosAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        DeviceList deviceList = (await this.client.GetStringAsync(BuildUrl("getdevicelistinfos"))).XmlToAs<DeviceList>(); 
        deviceList.Fill();
        return deviceList;
    }

    /// <summary>
    /// Provides the basic information of all SmartHome devices as XML.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<XmlDocument> GetDeviceListInfosXmlAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getdevicelistinfos"))).ToXml();
    }

    /// <summary>
    /// Last temperature information of the actuator.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double?> GetTemperatureAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gettemperature", ain))).ToNullableTemperature();
    }

    /// <summary>
    /// Setpoint temperature currently set for HKR.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double> GetHkrtSollAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gethkrtsoll", ain))).ToHkrTemperature();
    }

    /// <summary>
    /// Comfort temperature set for HKR timer.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double> GetHkrKomfortAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gethkrkomfort", ain))).ToHkrTemperature();
    }

    /// <summary>
    /// Economy temperature set for HKR timer.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double> GetHkrAbsenkAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gethkrabsenk", ain))).ToHkrTemperature();
    }

    /// <summary>
    /// HKR set temperature. The setpoint temperature is transferred with the "param" Get parameter.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="temperature">Temperature value °C (8°C - 28°C), On or Off.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<double> SetHkrtSollAsync(string ain, double temperature, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if ((temperature < 8 || temperature > 28) && (temperature != On) && (temperature != Off))
        {
            throw new ArgumentOutOfRangeException(nameof(temperature));
        }
        return (await this.client.GetStringAsync(BuildUrl("sethkrtsoll", ain, $"param={temperature.ToHkrTemperature()}"))).ToHkrTemperature();
    }

    /// <summary>
    /// Provides the basic statistics (temperature, voltage, power) of the actuator.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<DeviceStats> GetBasicDeviceStatsAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getbasicdevicestats", ain))).XmlToAs<DeviceStats>();
    }
    
    /// <summary>
    /// Provides the basic statistics (temperature, voltage, power) of the actuator as XML.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<XmlDocument> GetBasicDeviceStatsXmlAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getbasicdevicestats", ain))).ToXml();
    }
    
    /// <summary>
    /// Provides the basics information of all routines/triggers.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Needs FRITZ!OS 7.39 or higher.</remarks>
    public async Task<TriggerList> GetTriggerListInfosAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gettriggerlistinfos"))).XmlToAs<TriggerList>();
    }
    
    /// <summary>
    /// Provides the basics information of all routines/triggers as XML.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Needs FRITZ!OS 7.39 or higher.</remarks>
    public async Task<XmlDocument> GetTriggerListInfosXmlAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gettriggerlistinfos"))).ToXml();
    }

    /// <summary>
    /// Activate or deactivate trigger.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="active">True to activate, false to deactivate.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Needs FRITZ!OS 7.39 or higher.</remarks>
    public async Task<bool> SetTriggerActiveAsync(string ain, bool active, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("settriggeractive", ain, $"active={(active ? "1" : "0")}"))).ToBool();
    }

    /// <summary>
    /// Returns the basic information of all templates / templates.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<TemplateList> GetTemplateListInfosAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gettemplatelistinfos"))).XmlToAs<TemplateList>();
    }

    /// <summary>
    /// Returns the basic information of all templates / templates as XML.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<XmlDocument> GetTemplateListInfosXmlAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("gettemplatelistinfos"))).ToXml();
    }

    /// <summary>
    /// Apply template, the ain parameter is evaluated.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<int> ApplyTemplateAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("applytemplate", ain))).ToInt();
    }

    /// <summary>
    /// Device/actuator/lamp switch on/off or toggle. 
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="onOff">Switch on, off or toggle.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<OnOff> SetSimpleOnOffAsync(string ain, OnOff onOff, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("setsimpleonoff", ain, $"onoff={((int)onOff)}"))).ToOnOff();
    }

    /// <summary>
    /// Set dimming, height, brightness or level.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="level">Level (0 - 255) to set.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<int> SetLevelAsync(string ain, int level, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (level < 0 || level > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }
        return (await this.client.GetStringAsync(BuildUrl("setlevel", ain, $"level={level}"))).ToInt();
    }

    /// <summary>
    /// Set dimming, height, brightness or level level in percent.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="level">Level in percent (0 - 100) to set.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<int> SetLevelPercentageAsync(string ain, int level, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (level < 0 || level > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(level));
        }
        return (await this.client.GetStringAsync(BuildUrl("setlevelpercentage", ain, $"level={level}"))).ToInt();
    }

    /// <summary>
    /// Adjust hue saturation color. The HSV color space is used with the hue saturation mode supports.
    /// Of the brightness (value) can be over setlevel/setlevelpercentage be configured, the hue and saturation values are here configurable.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="hue">Hue value of the color.</param>
    /// <param name="saturation">Saturation value of the color.</param>
    /// <param name="duration">Speed of the change.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<int> SetColorAsync(string ain, int hue, int saturation, TimeSpan? duration = null, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (hue < 0 || hue > 359)
        {
            throw new ArgumentOutOfRangeException(nameof(hue));
        }
        if (saturation < 0 || saturation > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(saturation));
        }
        return (await this.client.GetStringAsync(BuildUrl("setcolor", ain, $"hue={hue}&saturation={saturation}&duration={duration.ToDeciseconds()}"))).ToInt();
    }

    /// <summary>
    /// Adjust hue saturation color. 
    /// Of the brightness (value) can be over setlevel/setlevelpercentage be configured, the hue and saturation values are here configurable.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="hue">Hue value of the color.</param>
    /// <param name="saturation">Saturation value of the color.</param>
    /// <param name="duration">Speed of the change.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<int> SetUnmappedColorAsync(string ain, int hue, int saturation, TimeSpan? duration = null, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (hue < 0 || hue > 359)
        {
            throw new ArgumentOutOfRangeException(nameof(hue));
        }
        if (saturation < 0 || saturation > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(saturation));
        }
        return (await this.client.GetStringAsync(BuildUrl("setunmappedcolor", ain, $"hue={hue}&saturation={saturation}&duration={duration.ToDeciseconds()}"))).ToInt();
    }

    /// <summary>
    /// Adjust color temperature.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="temperature">Color temperature in °Kelvin (2700° bis 6500°)</param>
    /// <param name="duration">Speed of the change.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<int> SetColorTemperatureAsync(string ain, int temperature, TimeSpan? duration = null, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (temperature < 2700 || temperature > 6500)
        {
            throw new ArgumentOutOfRangeException(nameof(temperature));
        }
        return (await this.client.GetStringAsync(BuildUrl("setcolortemperature", ain, $"temperature={temperature}&duration={duration.ToDeciseconds()}"))).ToInt();
    }

    /// <summary>
    /// Creates a color template for lamps.
    /// </summary>
    /// <param name="name">Name of the template to create.</param>
    /// <param name="levelPercentage">Level Percentage of the light.</param>
    /// <param name="hue">Hue value.</param>
    /// <param name="saturation">Saturation value.</param>
    /// <param name="ains">List of lamp devices.</param>
    /// <param name="colorpreset">User color preset or not.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">On of the argument are out of range.</exception>
    public async Task<int> AddColorLevelTemplateAsync(string name, int levelPercentage, int hue, int saturation, IEnumerable<string> ains, bool colorpreset = false, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (levelPercentage < 0 || levelPercentage > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(levelPercentage));
        }
        if (hue < 0 || levelPercentage > 359)
        {
            throw new ArgumentOutOfRangeException(nameof(hue));
        }
        if (saturation < 0 || saturation > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(saturation));
        }
        string ainlist = ains.Select((v, i) => $"child_{i + 1}={v}").Aggregate("", (a, b) => $"{a}&{b}");
        string req = $"webservices/homeautoswitch.lua?switchcmd=addcolorleveltemplate&sid={this.sessionId}&name={name}&levelPercentage={levelPercentage}&hue={hue}&saturation={saturation}&{ainlist}" + (colorpreset ? "&colorpreset=true" : "");
        return (await this.client.GetStringAsync(req)).ToInt();
    }

    /// <summary>
    /// Creates a color template for lamps
    /// </summary>
    /// <param name="name">Name of the template to create.</param>
    /// <param name="levelPercentage">Level Percentage of the light.</param>
    /// <param name="temperature">Color temperature of the light.</param>
    /// <param name="ains">List of lamp devices.</param>
    /// <param name="colorpreset">>User color preset or not.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException">On of the argument are out of range.</exception>
    public async Task<int> AddColorLevelTemplateAsync(string name, int levelPercentage, int temperature, IEnumerable<string> ains, bool colorpreset = false, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (levelPercentage < 0 || levelPercentage > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(levelPercentage));
        }
        if (temperature < 2700 || temperature > 6500)
        {
            throw new ArgumentOutOfRangeException(nameof(temperature));
        }
        string ainlist = ains.Select((v, i) => $"child_{i + 1}={v}").Aggregate("", (a, b) => $"{a}&{b}"); 
        string req = $"webservices/homeautoswitch.lua?switchcmd=addcolorleveltemplate&sid={this.sessionId}&name=name&levelPercentage={levelPercentage}&temperature={temperature}&{ainlist}" + (colorpreset ? "&colorpreset=true" : "");
        return (await this.client.GetStringAsync(req)).ToInt();
    }

    /// <summary>
    /// Provides a proposal for the color selection values.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<ColorDefaults> GetColorDefaultsAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getcolordefaults"))).XmlToAs<ColorDefaults>();
    }

    /// <summary>
    /// Provides a proposal for the color selection values as XML.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<XmlDocument> GetColorDefaultsXmlAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getcolordefaults"))).ToXml();
    }

    /// <summary>
    /// Activate HKR Boost with end time for the disable: endtimestamp = null.
    /// The end time may not exceed to 24 hours in the future lie.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="endtimestamp">End time to set.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<DateTime?> SetHkrBoostAsync(string ain, DateTime? endtimestamp = null, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (endtimestamp.HasValue && (endtimestamp < DateTime.Now || endtimestamp > DateTime.Now + new TimeSpan(24, 0, 0)))
        {
            throw new ArgumentOutOfRangeException(nameof(endtimestamp));
        }
        return (await this.client.GetStringAsync(BuildUrl("sethkrboost", ain, $"endtimestamp={endtimestamp.ToUnixTime()}"))).ToNullableDateTime();
    }
    
    /// <summary>
    /// HKR window open mode activate with end time for the disable: endtimestamp = null.
    /// The end time may not exceed to 24 hours in the future lie.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="endtimestamp">End time to set.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public async Task<DateTime?> SetHkrWindowOpenAsync(string ain, DateTime? endtimestamp = null, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (endtimestamp.HasValue && (endtimestamp < DateTime.Now || endtimestamp > DateTime.Now + new TimeSpan(24, 0, 0)))
        {
            throw new ArgumentOutOfRangeException(nameof(endtimestamp));
        }
        return (await this.client.GetStringAsync(BuildUrl("sethkrwindowopen", ain, $"endtimestamp={endtimestamp.ToUnixTime()}"))).ToNullableDateTime();
    }

    /// <summary>
    /// Close, open or stop the blind.
    /// Blinds have the HANFUN unit type Blind (281).
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="target">Target to set.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<Target> SetBlindAsync(string ain, Target target, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("setblind", ain, $"target={target.ToString().ToLower()}"))).ToTarget();
    }
    
    /// <summary>
    /// Change device or group name.
    /// Attention: the user session must have the smart home and app right.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="name">New name, maximum 40 characters.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <remarks>Requires the "Restricted FRITZ!Box settings for apps" permission.</remarks>
    public async Task<string> SetNameAsync(string ain, string name, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        if (name.Length > 40)
        {
            throw new ArgumentOutOfRangeException(nameof(name));
        }
        return (await this.client.GetStringAsync(BuildUrl("setname", ain, $"name={name}"))).TrimEnd();
    }
    
    /// <summary>
    /// json metadata of the template or change/set empty string
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <param name="metaData">Metadata to set.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>New in Fritz!OS 7.39</remarks>
    public async Task SetMetaDataAsync(string ain, MetaData metaData, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        await this.client.GetStringAsync(BuildUrl("setmetadata", ain, $"metadata={metaData.AsToJson()}"));
    }

    /// <summary>
    /// Start DECT ULE device registration.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Requires the "Restricted FRITZ!Box settings for apps" permission.</remarks>
    public async Task StartUleSubscriptionAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        await this.client.GetStringAsync(BuildUrl("startulesubscription"));
    }

    /// <summary>
    /// Query DECT-ULE device registration status.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Requires the "Restricted FRITZ!Box settings for apps" permission.</remarks>
    public async Task<SubscriptionState> GetSubscriptionStateAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getsubscriptionstate"))).XmlToAs<SubscriptionState>();
    }

    /// <summary>
    /// Query DECT-ULE device registration status as XML.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    /// <remarks>Requires the "Restricted FRITZ!Box settings for apps" permission.</remarks>
    public async Task<XmlDocument> GetSubscriptionStateXmlAsync(CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getsubscriptionstate"))).ToXml();
    }

    /// <summary>
    /// Provides the basic information of one SmartHome devices.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<Device> GetDeviceInfosAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getdeviceinfos", ain))).XmlToAs<Device>();
    }

    /// <summary>
    /// Provides the basic information of one SmartHome devices.
    /// </summary>
    /// <param name="ain">Identification of the actor or template.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    public async Task<XmlDocument> GetDeviceInfosXmlAsync(string ain, CancellationToken cancellationToken = default)
    {
        WebServiceException.ThrowIfNullOrNotConnected(this.service);

        return (await this.client.GetStringAsync(BuildUrl("getdeviceinfos", ain))).ToXml();
    }

    /// <summary>
    /// Create a bug report file.
    /// </summary>
    public void CreateBugReportFile()
    {
,
        string fileName = $"BugReport-{DateTime.Now:yyy-MM-dd-HH-mm-ss}.xml";
        using (var file = File.CreateText(fileName))
        {
            file.WriteLine("<Report>");
            file.WriteLine(this.client.GetStringAsync(BuildUrl("getdevicelistinfos")).Result);
            file.WriteLine(this.client.GetStringAsync(BuildUrl("gettemplatelistinfos")).Result);
            file.WriteLine(this.client.GetStringAsync(BuildUrl("getcolordefaults")).Result);
            try
            {
                file.WriteLine(this.client.GetStringAsync(BuildUrl("gettriggerlistinfos")).Result);
            }
            catch
            { }
            file.WriteLine("</Report>");
        }
    }
            
    #endregion
}
