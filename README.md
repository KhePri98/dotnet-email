寄送電子郵件的背景服務，可以設定幾分鐘觸發一次、一次寄送幾封信
```
...
// Add background services
builder.Services.AddHostedService<TimedEmailSendingService>();
```

練習重點：
- 模仿[Clean Architecture](https://github.com/dotnet-architecture/eShopOnWeb)
- [用 Specification pattern 實現 Generic Repository](https://hackmd.io/@alanyang/HJ4lz8q2o)
- 實作`BackgroundService`類別，並能在內消化`Scoped`服務（[Consuming a scoped service in a background task](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-8.0&tabs=visual-studio#consuming-a-scoped-service-in-a-background-task)）
