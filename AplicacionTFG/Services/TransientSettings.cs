using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionTFG.Services;
public static class TransientSettings
{
    private static readonly Dictionary<string, object> _values = new Dictionary<string, object>();

    public static object Get(string key) =>
        _values.TryGetValue(key, out var v) ? v : null;

    public static void Set(string key, object value) =>
        _values[key] = value;

    public static void Clear() =>
        _values.Clear();
}
