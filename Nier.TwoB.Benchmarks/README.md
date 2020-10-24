# Benchmark results
compute hashcode for 1024 strings each with 1024 characters.

```
BenchmarkDotNet=v0.12.1, OS=macOS Catalina 10.15.7 (19H2) [Darwin 19.6.0]
Intel Core i7-4850HQ CPU 2.30GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.403
  [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
  DefaultJob : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
```

|             Method |       Mean |     Error |    StdDev |
|------------------- |-----------:|----------:|----------:|
|     StringHashCode | 847.979 us | 2.2238 us | 1.9713 us |
| TwoBStringHashCode |   1.957 us | 0.0343 us | 0.0367 us |
