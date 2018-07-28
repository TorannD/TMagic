using RimWorld;
using System;
using Verse;
using AbilityUser;

namespace TorannMagic
{
    public class Verb_Rainmaker : Verb_UseAbility
    {
        public Type eventClass;

        protected override bool TryCastShot()
        {
            Map map = base.CasterPawn.Map;
 
            WeatherDef rainMakerDef = new WeatherDef();
            if(map.mapTemperature.OutdoorTemp < 0)
            {
                if (map.weatherManager.curWeather.defName == "SnowHard" || map.weatherManager.curWeather.defName == "SnowGentle")
                {
                    rainMakerDef = WeatherDef.Named("Clear");
                    map.weatherManager.TransitionTo(rainMakerDef);
                    return true;                    
                }
                else
                {
                    if (Rand.Chance(.5f))
                    {
                        rainMakerDef = WeatherDef.Named("SnowGentle");
                    }
                    else
                    {
                        rainMakerDef = WeatherDef.Named("SnowHard");
                    }
                    map.weatherDecider.DisableRainFor(0);
                    map.weatherManager.TransitionTo(rainMakerDef);
                    return true;
                }
            }
            else
            {
                if (map.weatherManager.curWeather.defName == "Rain" || map.weatherManager.curWeather.defName == "RainyThunderstorm" || map.weatherManager.curWeather.defName == "FoggyRain")
                {
                    rainMakerDef = WeatherDef.Named("Clear");
                    map.weatherManager.TransitionTo(rainMakerDef);
                    return true;
                    
                }
                else
                {
                    int rnd = Rand.RangeInclusive(1, 3);
                    switch (rnd)
                    {
                        case 1:
                            rainMakerDef = WeatherDef.Named("Rain");
                            break;
                        case 2:
                            rainMakerDef = WeatherDef.Named("RainyThunderstorm");
                            break;
                        case 3:
                            rainMakerDef = WeatherDef.Named("FoggyRain");
                            break;
                    }                    
                    map.weatherDecider.DisableRainFor(0);
                    map.weatherManager.TransitionTo(rainMakerDef);
                    return true;
                }
            } 
        }
    }
}
