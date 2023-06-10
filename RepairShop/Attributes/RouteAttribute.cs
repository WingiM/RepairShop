using System;

namespace RepairShop.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class RouteAttribute : Attribute
{
    public required string Route { get; init; }
}