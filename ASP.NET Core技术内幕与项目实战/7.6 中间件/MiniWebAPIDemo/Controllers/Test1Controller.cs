﻿using Microsoft.Extensions.Caching.Memory;
using MiniWebAPIDemo.Models;
using System;

namespace MiniWebAPIDemo.Controllers;

public class Test1Controller
{
    private readonly IMemoryCache memoryCache;
    public Test1Controller(IMemoryCache memoryCache)
    {
        this.memoryCache = memoryCache;
    }
    public Person IncAge(Person p)
    {
        p.Age++;
        return p;
    }
    public object[] Get2(HttpContext ctx)
    {
        DateTime now = memoryCache.GetOrCreate("Now", e => DateTime.Now);
        string name = ctx.Request.Query["name"];
        return new object[] { "hello" + name, now };
    }
}
