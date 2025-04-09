===================================  Health Checks
* Pasos para instalacion Health Checks
dotnet add package AspNetCore.HealthChecks.SqlServer

* Registrar Health Checks en Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        name: "SQL Server",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy
    );
* Instala si aun lo tienes instalado 
dotnet add package AspNetCore.HealthChecks.UI.Client

* Agrega el endpoint /health personalizado en Program.cs
debajo de "app.UseAuthorization();"

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

app.UseAuthorization();

app.MapHealthChecks("/health", new HealthCheckOptions
{
     ResponseWriter = async (context, report) =>
     {
         context.Response.ContentType = "application/json";
         var result = JsonSerializer.Serialize(new
         {
             status = report.Status.ToString(),
             checks = report.Entries.Select(e => new {
                 name = e.Key,
                 status = e.Value.Status.ToString(),
                 duration = e.Value.Duration.ToString(),
                 description = e.Value.Description,
                 tags = e.Value.Tags.ToArray()
             }),
             totalDuration = report.TotalDuration.ToString()
         });
         await context.Response.WriteAsync(result);
     }
});

* Listo ejecuta y prueba 
si tu url es: https://localhost:7255/swagger/index.html
tu Health Checks esta en https://localhost:7255/health 
te tiene que retornar en formato json ejemplo.
{
   "status":"Healthy",
   "checks":[
      {
         "name":"SQL Server",
         "status":"Healthy",
         "duration":"00:00:00.0096322",
         "description":null,
         "tags":[
            
         ]
      }
   ],
   "totalDuration":"00:00:00.0163747"
}

Con esto ya podemos ir a Prometheus
===================================  Prometheus
* Pasos para instalacion prometheus
dotnet add package prometheus-net.AspNetCore

* En program.cs
using Prometheus;

var app = builder.Build();

// Mide todas las peticiones HTTP
app.UseHttpMetrics();

// Mapea el endpoint /metrics
app.MapMetrics(); // Esto expone /metrics en formato Prometheus

* Test
https://localhost:#miNroPuerto/metrics

* Prometheus Settings
crear un file prometheus.yml

===================================  Grafana