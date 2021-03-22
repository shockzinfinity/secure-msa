using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace Common.Logging
{
  public static class SeriLogger
  {
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
      (context, configuration) =>
      {
        var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
        var seqUri = context.Configuration.GetValue<string>("SeqConfiguration:Uri");
        var apiKey = context.Configuration.GetValue<string>("SeqConfiguration:ApiKey");

        configuration.Enrich.FromLogContext()
                     .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                     .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
                     .Enrich.WithMachineName()
                     .WriteTo.Debug()
                     .WriteTo.Console()
                     .WriteTo.Seq(seqUri, apiKey: apiKey)
                     .WriteTo.Elasticsearch(
                        new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(elasticUri))
                        {
                          IndexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                          AutoRegisterTemplate = true,
                          NumberOfShards = 2,
                          NumberOfReplicas = 1
                        })
                     .ReadFrom.Configuration(context.Configuration);
      };
  }
}