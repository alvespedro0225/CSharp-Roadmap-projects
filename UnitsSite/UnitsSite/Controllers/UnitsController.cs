using Microsoft.AspNetCore.Mvc;
using UnitsSite.Models;

namespace UnitsSite.Controllers;

public class UnitsController : Controller
{
    public IActionResult Weight()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Weight(int fromUnit, int toUnit, int oldValue)
    {
        UnitModel context;
        if (fromUnit == toUnit)
        {
            context = new UnitModel
            {
                FromUnit = WeightUnits[fromUnit],
                ToUnit = WeightUnits[toUnit],
                Value = oldValue,
                Result = oldValue
            };
            return View(context);
        }
        var result = oldValue / WeightValues[fromUnit] * WeightValues[toUnit];
        context = new UnitModel
        {
            ToUnit = WeightUnits[toUnit],
            FromUnit = WeightUnits[fromUnit],
            Value = oldValue,
            Result = result
        };
        return View(context);
    }

    public IActionResult Temperature()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Temperature(int fromUnit, int toUnit, int oldValue)
    {
        // Kelvin: 0, Fahr: 1, Cel: 2
        UnitModel context;
        if (fromUnit == toUnit)
        {
            context = new UnitModel
            {
                FromUnit = TemperatureUnits[fromUnit],
                ToUnit = TemperatureUnits[toUnit],
                Value = oldValue,
                Result = oldValue
            };
            return View(context);
        }
        var result = (fromUnit + toUnit) switch
        {
            1 => FahrenheitAndKelvin(fromUnit, toUnit, oldValue),
            2 => CelsiusAndKelvin(fromUnit, toUnit, oldValue),
            3 => FahrenheitAndCelsius(fromUnit, toUnit, oldValue),
            _ => throw new ArgumentException("Invalid unit")
        };
        context = new UnitModel
        {
            FromUnit = TemperatureUnits[fromUnit],
            ToUnit = TemperatureUnits[toUnit],
            Value = oldValue,
            Result = result
        };
        return View(context);
    }

    public IActionResult Length()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Length(int fromUnit, int toUnit, int oldValue)
    {
        UnitModel context;
        if (fromUnit == toUnit)
        {
            context = new UnitModel
            {
                FromUnit = LengthUnits[fromUnit],
                ToUnit = LengthUnits[toUnit],
                Value = oldValue,
                Result = oldValue
            };
            return View(context);
        }
        var result = oldValue / LengthValues[fromUnit] * LengthValues[toUnit];
        context = new UnitModel
        {
            ToUnit = LengthUnits[toUnit],
            FromUnit = LengthUnits[fromUnit],
            Value = oldValue,
            Result = result
        };
        return View(context);
    }

    private double FahrenheitAndCelsius(int fromUnit, int toUnit, double value)
    {
        if (fromUnit == 2)
        {
            return value * 9.0 / 5.0 + 32.0;
        }
        return (value - 32.0) * 5.0 / 9.0;
    }

    private double FahrenheitAndKelvin(int fromUnit, int toUnit, double value)
    {
        if (fromUnit == 0)
        {
            return (value - 273.15) * 9.0 / 5.0 + 32.0;
        }
        return (value - 32.0) * 5.0 / 9.0 +273.15;
    }

    private double CelsiusAndKelvin(int fromUnit, int toUnit, double value)
    {
        if (fromUnit == 0)
            return value - 273.15;
        return value + 273.15;
    }
    private double[] WeightValues { get; } =
    [
        100_000.0,
        1_000.0,
        1.0,
        35.274,
        2.20462
    ];

    private string[] WeightUnits { get; } =
    [
        "mg",
        "g",
        "kg",
        "ounce",
        "pound"
    ];

    private string[] TemperatureUnits { get; } =
    [
        "Kelvin",
        "Fahrenheit",
        "Celsius"
    ];

    private double[] LengthValues { get; } =
    [
        1_000.0,
        100.0,
        1.0,
        0.001,
        39.3_701,
        3.32_084,
        1.09_361,
        0.000_621_371
    ];
    private string[] LengthUnits { get; } =
    [
        "mm",
        "cm",
        "m",
        "km",
        "inch",
        "foot",
        "yard",
        "mile"
    ];
}

