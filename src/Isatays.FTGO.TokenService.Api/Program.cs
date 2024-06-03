using Isatays.FTGO.TokenService.Api;
using Isatays.FTGO.TokenService.Api.Features.Extensions;
using Isatays.FTGO.TokenService.Api.Features.Middlewares;
using Serilog;

try
{
    var app = WebApplication.CreateBuilder().ConfigureBuilder().Build().ConfigureApp();

    app.UseMiddleware<LoggingMiddleware>();
    app.UseMiddleware<ExceptionHandleMiddleware>(); ;

    app.ConfigureTokenEndpoints();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "��������� ���� ��������� � ����������� ({ApplicationName})!");
}
finally
{
    Log.Information("{Msg}!", "������������� �������� ������������");
    await Log.CloseAndFlushAsync();
}
