---
title: Реализация свойства типов для определения указанного члена класса
topic: "C++.Стандартная библиотека.Метапрограммирование"
tags: "cpp,clang"
date: 17.01.2024
path: posts/2024/post-1.md
---

# Post 1

## 1. A

aaa

```cpp
template<typename T>
auto foo(const std::vector<T>& v) -> decltype(auto);
```

```csharp
class DataStore<T>
{
    public T Data { get; set; }
}
```

## 2. B

bbb
