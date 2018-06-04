using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using NLog;

namespace NLogAppInsightsTargetTest
{
	class Program
	{
		static void Main(string[] args)
		{
			//LogToAIWithoutUsingNLog();
			LogToAIByUsingNLog();
		}

		static void LogToAIWithoutUsingNLog()
		{
			var client = new TelemetryClient();

			//logging a custom event using the AI API
			client.TrackEvent("AppInsights is now ready for logging");

			//use the AI APIs to log tracing information
			client.TrackTrace("Trace tracking started,", SeverityLevel.Verbose, new Dictionary<string, string> { ["1"] = "jason pease" });

			try
			{
				throw new Exception("an exception");
			}
			catch (Exception e)
			{
				//use the AI API to capture exceptions
				client.TrackException(e);
			}

			//You may need to call Flush() to ensure that your AI data is sent to Azure
			client.Flush();

			//For projects that shutdown, we need to give the TelemetryClient enough time to send the data
			//In this instance, we pause for 1sec
			Thread.Sleep(1000);
		}

		static void LogToAIByUsingNLog()
		{
			var client = new TelemetryClient();

			//logging a custom event using the AI API
			client.TrackEvent("AppInsights is now ready for logging");

			//use the AI APIs to log tracing information
			client.TrackTrace("Trace tracking started,", SeverityLevel.Verbose, new Dictionary<string, string> { ["1"] = "jason pease" });

			try
			{
				throw new Exception("an exception");
			}
			catch (Exception e)
			{
				//use the AI API to capture exceptions
				client.TrackException(e);
			}

			LoadRunningProcess(client);

			// using nLog to push data to AI
			var logger = LogManager.GetLogger("Example");

			logger.Trace("Using nLog to write a 'trace' message");
			logger.Debug("Using nLog to write a 'debug' message");
			logger.Info("Using nLog to write a 'info' message");
			logger.Warn("Using nLog to write a 'warning' message");
			logger.Error("Using nLog to write an 'error' message");
			logger.Fatal("Using nLog to write a 'fatal' message");

			//You may need to call Flush() to ensure that your AI data is sent to Azure
			client.Flush();

			//For projects that shutdown, we need to give the TelemetryClient enough time to send the data
			//In this instance, we pause for 1sec
			Thread.Sleep(1000);
		}

		static void LoadRunningProcess(TelemetryClient client)
		{
			var longRunningOperation = client.StartOperation<DependencyTelemetry>("Long Running Ajax CAll");

			Thread.Sleep(5000);

			client.StopOperation<DependencyTelemetry>(longRunningOperation);
		}
	}
}
