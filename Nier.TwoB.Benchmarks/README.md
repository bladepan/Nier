# Benchmark results
the longer the string, the slower string Equals and HashCode.

env:

```
BenchmarkDotNet=v0.13.1, OS=ubuntu 21.10
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.201
  [Host]     : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
  DefaultJob : .NET 6.0.3 (6.0.322.12309), X64 RyuJIT
```

## hashcode benchmark
hashcode for 1024 strings each with 1024 characters.

|             Method |       Mean |     Error |    StdDev |
|------------------- |-----------:|----------:|----------:|
|     StringHashCode | 685.931 μs | 7.2269 μs | 6.7600 μs |
| TwoBStringHashCode |   1.993 μs | 0.0109 μs | 0.0085 μs |

## equals benchmark
equals for 1024 strings each with 1024 characters.

|                      Method |      Mean |     Error |    StdDev |
|---------------------------- |----------:|----------:|----------:|
|           StringEqualsFalse |  4.852 μs | 0.0075 μs | 0.0070 μs |
|       TwoBStringEqualsFalse |  4.522 μs | 0.0167 μs | 0.0130 μs |
|     StringEqualsSharePrefix | 80.203 μs | 1.5661 μs | 1.5382 μs |
| TwoBStringEqualsSharePrefix |  5.596 μs | 0.0599 μs | 0.0501 μs |
|            StringEqualsTrue | 79.794 μs | 1.1011 μs | 0.8597 μs |
|        TwoBStringEqualsTrue |  4.138 μs | 0.0726 μs | 0.0777 μs |

## dictionary benchmark

|      Method |      Mean |    Error |   StdDev |
|------------ |----------:|---------:|---------:|
|     DictGet | 332.30 μs | 4.755 μs | 3.971 μs |
| TwoBDictGet |  19.82 μs | 0.084 μs | 0.066 μs |
