using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApiHitter
{
	class Program
	{
		static HttpClient client = new HttpClient();
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.WriteLine("Waiting for WebApi to be ready...");
			Thread.Sleep(10000);
			Console.WriteLine("Ready to Post...");
			RunAsync().GetAwaiter().GetResult();
		}

		static List<string> ParseXmlFromTextFile()
		{
			string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var serializer = new JsonSerializer();
			List<string> xmlMessages;
			using (StreamReader fileStream = File.OpenText($"{currentDirectory}/Input/XmlToPost.txt"))
			{
				xmlMessages = (List<string>)serializer.Deserialize(fileStream, typeof(List<string>));
			}

			return xmlMessages;
		}

		static async Task<Uri> PostXmlPayloadAsync()
		{
			int count = 0;
			HttpResponseMessage response = null;
			var xmlMessagesInput = ParseXmlFromTextFile();
			foreach (var payload in xmlMessagesInput)
			{
				var httpContent = new StringContent(payload, Encoding.UTF8, "application/xml");
				response = await client.PostAsync("/PostAwesomeXml", httpContent);
				if (response.IsSuccessStatusCode)
				{
					count++;
					Console.WriteLine($"Payload POST is successful. Reason is {response.ReasonPhrase} and Status is {response.StatusCode}. Posted {count} XmlPayloads");
				}
				else
				{
					Console.WriteLine(payload);
				}
			}
			return response.Headers.Location;
		}

		static async Task RunAsync()
		{
			client.BaseAddress = new Uri("http://localhost:32817");
			client.DefaultRequestHeaders.Accept.Clear();

			try
			{
				var url = await PostXmlPayloadAsync();
				Console.WriteLine($"Created at {url}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}
	}
}
