# Logging & Caching Tutorial

### This tutorial covers system logging and event tracking, focusing on:
- Seri log
- ILogger

### It also explores caching mechanisms to optimize performance and reduce reliance on SQL for caching, including:
- Redis:
  - Key:Value pairs
  - Commands: SET, GET, DEL, INCR, TTL/Expiration
  - StackExchange.Redis
  - IConnectionMultiplexer
- Caching in ASP.NET Core:
  - Cache Aside Pattern
  - Cache Invalidation

### Finally, the tutorial delves into error handling and monitoring:
- ILogger
- Log Levels
- Serilog:
  - Console Logging
  - File Logging
  - Custom Sink
  - Sending Errors to Telegram
  - Global Exception Middleware
