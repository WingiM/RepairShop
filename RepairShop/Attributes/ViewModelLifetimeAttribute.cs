using System;
using Microsoft.Extensions.DependencyInjection;

namespace RepairShop.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class ViewModelLifetimeAttribute : Attribute
{
    public required ServiceLifetime Lifetime { get; init; }
}